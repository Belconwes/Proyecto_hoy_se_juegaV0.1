using Microsoft.AspNetCore.Mvc;
using ProyectoHsj_Beta.Models;
using System.Threading.Tasks;
using ProyectoHsj_Beta.DTO;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ProyectoHsj_Beta.ViewsModels;
using ProyectoHsj_Beta.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using static ProyectoHsj_Beta.DTO.ReservationPostDTO;
namespace ProyectoHsj_Beta.Controllers
{
    public class Reservas : Controller
    {
        private readonly HoySeJuegaContext _context;
        private readonly MercadoPagoService _MercadoPagoService;
        public Reservas(HoySeJuegaContext context, MercadoPagoService mercadoPagoService)
        {
            _context = context;
            _MercadoPagoService = mercadoPagoService;
        }

        // GET: Reservas/Create

        // POST: Reservas/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ReservaViewModel reserva)
        {
            if (reserva.Fecha == null || reserva.Fecha == DateTime.MinValue)
            {
                return BadRequest("Fecha no válida.");
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("Usuario no autenticado.");
            }
            int idUsuario = int.Parse(userIdClaim);

            // Buscar el horario específico seleccionado por el usuario
            var horario = await _context.HorarioDisponibles
                .FirstOrDefaultAsync(h => h.IdHorarioDisponible == reserva.IdHorarioDisponible && h.DisponibleHorario == true);

            if (horario == null)
            {
                Console.WriteLine("Paso por el null de horario");
                return BadRequest("No hay horario disponible para esa fecha.");
            }

            var nuevaReserva = new Reserva
            {
                FechaReserva = reserva.Fecha,
                IdHorarioDisponible = horario.IdHorarioDisponible,
                IdEstadoReserva = 1, // Activa
                IdUsuario = idUsuario
            };

            // Marcar el horario como no disponible
            horario.DisponibleHorario = false;
            Console.WriteLine($"este es el id de fecha: {horario.IdHorarioDisponible} ");
            _context.HorarioDisponibles.Update(horario);

            _context.Reservas.Add(nuevaReserva);
            await _context.SaveChangesAsync();
            return Json(new { success = true, idReserva = nuevaReserva.IdReserva });
        }
        public async Task<IActionResult> Index()
        {
            ViewData["ID_horario_disponible"] = new SelectList(await _context.HorarioDisponibles.ToListAsync(), "Id", "HoraInicio");
            return View();
        }

        public async Task<IActionResult> GetReservas()
        {
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)); // Asegúrate de obtener el ID correctamente
            var isAdmin = User.IsInRole("admin");

            // Cargamos las reservas desde la base de datos
            var reservasDb = await _context.Reservas
                .Where(r => r.FechaReserva.HasValue)
                .ToListAsync();
            var reservas = reservasDb.Select(r => new Evento
            {
                id = r.IdReserva,
                title = "Reserva " + r.IdReserva,
                start = r.FechaReserva.Value.ToString("yyyy-MM-ddTHH:mm"),
                end = r.FechaReserva.Value.AddHours(1).ToString("yyyy-MM-ddTHH:mm"),
                creadorId = r.IdUsuario,
                color = ObtenerColorEstado(r.IdEstadoReserva),
                estado = ObtenerEstado(r.IdEstadoReserva)
            })
            .Where(e => e.estado != "CANCELADA" || e.creadorId == currentUserId || isAdmin) // Filtra según visibilidad
            .ToList();

            var horariosDisponibles = await _context.HorarioDisponibles
                .Where(h => h.DisponibleHorario == true)
                .Select(h => new Evento
                {
                    id = h.IdHorarioDisponible,
                    title = "Disponible",
                    start = new DateTime(
                        h.FechaHorario.Year,
                        h.FechaHorario.Month,
                        h.FechaHorario.Day,
                        h.HoraInicio.Hour,
                        h.HoraInicio.Minute,
                        0).ToString("yyyy-MM-ddTHH:mm"),
                    end = new DateTime(
                        h.FechaHorario.Year,
                        h.FechaHorario.Month,
                        h.FechaHorario.Day,
                        h.HoraFin.Hour,
                        h.HoraFin.Minute,
                        0).ToString("yyyy-MM-ddTHH:mm"),
                    color = "green",
                    estado = "Disponible"
                })
                .ToListAsync();

            // Ahora puedes concatenar ambas listas sin problemas
            var eventos = reservas.Concat(horariosDisponibles).ToList();

            return Json(eventos);
        }

        private string ObtenerEstado(int estadoId) =>
            estadoId switch
            {
                1 => "PENDIENTE",
                2 => "CONFIRMADA",
                3 => "CANCELADA",
                _ => "DESCONOCIDO"
            };

        private string ObtenerColorEstado(int estadoId) =>
            estadoId switch
            {
                1 => "yellow",
                2 => "green",
                3 => "gray",
                _ => "black"
            };
        //Obtener y renderizar los horarios
        [HttpGet]
        public async Task<IActionResult> GetAvailableTimeSlots(DateTime fecha)
        {
            var horarios = await _context.HorarioDisponibles
                .Where(h => (h.DisponibleHorario ?? false) && h.FechaHorario == DateOnly.FromDateTime(fecha))
                .Select(h => new
                {
                    IdHorarioDisponible = h.IdHorarioDisponible, // Agrega el ID
                    HoraInicio = h.HoraInicio.ToString("HH:mm"),
                    HoraFin = h.HoraFin.ToString("HH:mm")
                })
                .ToListAsync();

            // Cambiar el return para incluir ID y las horas
            return Json(horarios);
        }
        [HttpPost]
        public async Task<IActionResult> PagarReserva(int reservaId, decimal monto)
        {
            Console.WriteLine("La id reserva es :" + reservaId);
            Console.WriteLine("El monto de pago es:" + monto);
            var pago = new Pago
            {
                IdReserva = reservaId,
                MontoPago = monto
            };

            var preferencia = await _MercadoPagoService.CrearPreferenciaDePago(pago);

            // Redirige al usuario al URL de Mercado Pago
            return Redirect(preferencia.InitPoint);
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);

            if (reserva == null)
            {
                return NotFound("Reserva no encontrada.");
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || int.Parse(userIdClaim) != reserva.IdUsuario)
            {
                return Forbid("No tienes permiso para cancelar esta reserva.");
            }

            // Marcar la reserva como cancelada
            reserva.IdEstadoReserva = 3;  // Estado: Cancelada
            _context.Reservas.Update(reserva);

            // Liberar el horario asociado
            var horario = await _context.HorarioDisponibles
                .FirstOrDefaultAsync(h => h.IdHorarioDisponible == reserva.IdHorarioDisponible);
            if (horario != null)
            {
                horario.DisponibleHorario = true;
                _context.HorarioDisponibles.Update(horario);
            }

            await _context.SaveChangesAsync();
            return Ok("Reserva cancelada exitosamente.");
        }

       
       


        [HttpDelete]
        public async Task<IActionResult> DeleteReserva(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);

            if (reserva == null)
            {
                return NotFound("No se encontró la reserva.");
            }

            if (reserva.IdEstadoReserva != 3) // Estado 2 = CANCELADA
            {
                return BadRequest("Solo se pueden eliminar reservas canceladas.");
            }
            // Guardar los datos en el historial antes de eliminar
            var historialReserva = new HistorialReserva
            {
                IdReserva = reserva.IdReserva,
                ID_usuario = reserva.IdUsuarioNavigation?.IdUsuario ?? 0,  // Revisa si IdUsuarioNavigation es null
                FechaReserva = reserva.FechaReserva ?? DateTime.Now,
                HoraInicio = reserva.IdHorarioDisponibleNavigation?.HoraInicio.ToTimeSpan() ?? TimeSpan.Zero,  // Verifica si IdHorarioDisponibleNavigation es null
                HoraFin = reserva.IdHorarioDisponibleNavigation?.HoraFin.ToTimeSpan() ?? TimeSpan.Zero,  // Verifica si IdHorarioDisponibleNavigation es null
                EstadoReserva = reserva.IdEstadoReservaNavigation?.NombreEstadoReserva ?? "Desconocido",  // Verifica si IdEstadoReservaNavigation es null
                FechaEliminacion = DateTime.Now,
                NombreUsuario = reserva.IdUsuarioNavigation?.NombreUsuario ?? "Desconocido",  // Verifica si IdUsuarioNavigation es null
                TelefonoUsuario = reserva.IdUsuarioNavigation?.TelefonoUsuario.ToString() ?? "N/A"  // Verifica si TelefonoUsuario es null
            };
            _context.HistorialReservas.Add(historialReserva);
            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();

            return Ok("Reserva eliminada correctamente.");
        }

    }
}
