using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations.Schema;

namespace SertaCup_site.Models
{
    [Table("jogos")]
    public class Game
    {
        public int Id { get; set; }
        public string Chave {  get; set; }
        public DateTime hora_prevista { get; set; }
        public int equipa1 { get; set; }
        public int equipa2 { get; set; }
        public byte golos_equipa1 { get; set; }
        public byte golos_equipa2 { get; set; }
        public DateTime? hora_inicio { get; set; }
        public DateTime? hora_inicio_2parte { get; set; }
        public bool primeira_parte_terminada { get; set; }
        public bool começado { get; set; }
        public bool terminado { get; set; }
        public int tempo_pausado_1 { get; set; }
        public int tempo_pausado_2 { get; set; }
        public bool interrompido { get; set; }
        public int campo {  get; set; }
        public int? grupo {  get; set; }

        public string?situacao_precaria { get; set; }

    }
}