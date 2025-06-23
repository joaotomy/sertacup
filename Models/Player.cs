using System.ComponentModel.DataAnnotations.Schema;

namespace SertaCup_site.Models
{

    [Table("jogadores")]
    public class Player
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Equipa_id { get; set; }

    }
}
