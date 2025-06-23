using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using SertaCup_site.Models;

namespace SertaCup_site.Models
{

    public class TorneioViewModel
    {
        public List<GroupViewModel> Grupos { get; set; }
        public List<JogoViewModel> Jogos { get; set; }
        public List<Player> Jogadores { get; set; }

        public TorneioViewModel()
        {
            Grupos = new List<GroupViewModel>();
            Jogos = new List<JogoViewModel>();
            Jogadores = new List<Player>();
        }
    }
}