namespace SertaCup_site.Models
{
    public class GroupViewModel
    {
        public string Name { get; set; }
        public List<TeamViewModel> Teams { get; set; }
    }

    public class TeamViewModel
    {
        public int Position { get; set; }
        public string Name { get; set; }
        public int J { get; set; }
        public int V { get; set; }
        public int E { get; set; }
        public int D { get; set; }
        public int GD { get; set; }
        public int GM { get; set; }
        public int P { get; set; }
    }
}
