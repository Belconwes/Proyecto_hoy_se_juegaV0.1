namespace ProyectoHsj_Beta.Models
{
    public class Evento
    {
        public int id { get; set; }
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public int? creadorId { get; set; } // Puede ser null en el caso de los horarios disponibles
        public string color { get; set; }
        public string estado { get; set; }
    }
}
