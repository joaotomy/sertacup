using System.ComponentModel.DataAnnotations.Schema;

namespace SertaCup_site.Models
{

    [Table("marcadores_jogo")]
    public class Marcador
    {
        public int Id { get; set; }
        public int jogo_id { get; set; }
        public int marcador { get; set; }
        public int equipa { get; set; }

    }


}
