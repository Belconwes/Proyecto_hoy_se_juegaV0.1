using System;
using System.Collections.Generic;

namespace ProyectoHsj_Beta.Models;

public partial class Auditorium
{
    public int IdAuditoria { get; set; }

    public int IdUsuario { get; set; }

    public DateTime FechaAuditoria { get; set; }

    public string DescripcionDeAccion { get; set; } = null!;

    public int IdAccionRealizada { get; set; }

    public virtual AccionRealizadum IdAccionRealizadaNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
