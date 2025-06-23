using System.ComponentModel.DataAnnotations.Schema;

namespace SertaCup_site.Models
{

    [Table("equipas")]
    public class Team
    {
        public int? Id { get; set; }
        public string Nome_Completo { get; set; }
        public string? Nome { get; set; }
        public string Sigla { get; set; }
        public string Cor_Principal { get; set; }
        public string? Cor_Secundaria { get; set; }
        public string Simbolo   { get; set; }

    }
}