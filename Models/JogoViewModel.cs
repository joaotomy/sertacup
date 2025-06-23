namespace SertaCup_site.Models
{
    public class JogoViewModel
    {
        public string Id { get; set; }
        public string equipa1 { get; set; }
        public string equipa2 { get; set; }
        public string golos_equipa1 { get; set; }
        public string golos_equipa2 { get; set; }
        public string grupo { get; set; }
        public string Campo { get; set; }
        public string Hora { get; set; }
        public string Estado { get; set; }
        public string Tipo { get; set; }
        public string? Fase { get; set; }

        public List<string> MarcadoresEquipa1 { get; set; } = new();
        public List<string> MarcadoresEquipa2 { get; set; } = new();


    }
}
