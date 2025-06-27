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

        public string? hora_inicio { get; set; }
        public string? hora_inicio_2parte { get; set; }
        public bool? primeira_parte_terminada { get; set; }
        public bool? começado { get; set; }
        public bool? terminado { get; set; }

        // Valores numéricos para lógica
        public int golos1 { get; set; }
        public int golos2 { get; set; }

        // Hora como DateTime para lógica e ordenação
        public DateTime horaPrevista { get; set; }
        public DateTime? horaInicio { get; set; }
        public DateTime? horaInicio2Parte { get; set; }

        // Estados lógicos
        public bool primeiraParteTerminada { get; set; }
        public bool terminou { get; set; }
        public bool interrompido { get; set; }
        public int tempoPausado1 { get; set; }
        public int tempoPausado2 { get; set; }

        // Campo como número se precisares
        public int campoNumero { get; set; }

        // Info extra
        public string? situacao_precaria { get; set; }
    }
}
