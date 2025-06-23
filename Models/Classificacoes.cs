using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SertaCup_site.Models
{
    [Table("classificacoes")]
    public class Classificacao
    {

        [Key]
        public int Id { get; set; }

        public int equipa { get; set; }
        public int grupo { get; set; }
        public int? pontos { get; set; }
        public int? jogos_feitos { get; set; }
        public int? vitorias { get; set; }
        public int? derrotas { get; set; }
        public int? empate { get; set; }
        public int? golos_marcados { get; set; }
        public int? golos_sofridos { get; set; }
    }
}
