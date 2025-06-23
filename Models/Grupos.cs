using System.ComponentModel.DataAnnotations.Schema;

namespace SertaCup_site.Models
{
    [Table("grupos")]
    public class Grupo
    {
        public int id { get; set; }
        public string grupo { get; set; }
        public byte equipa1 { get; set; }
        public byte equipa2 { get; set; }
        public byte equipa3 { get; set; }
        public byte equipa4 { get; set; }
    }
}
