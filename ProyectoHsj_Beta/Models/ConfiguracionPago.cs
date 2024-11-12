namespace ProyectoHsj_Beta.Models
{
    public class ConfiguracionPago
    {
        public int IdConfiguracion { get; set; }
        public decimal MontoSena { get; set; }
        public DateTime FechaModificacion { get; set; } = DateTime.Now;
    }
}
