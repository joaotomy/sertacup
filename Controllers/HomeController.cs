using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SertaCup_site.Models;
using SertaCup_site.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;

namespace SertaCup_site.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }



        public IActionResult Torneio()
        {


            var gruposDb = _context.Grupos.ToList();
            var classificacoesDb = _context.Classificacoes.ToList();
            var equipasDb = _context.Team.ToDictionary(e => e.Id, e => e.Nome);
            var jogosDb = _context.Game.ToList();

            // Construir os grupos
            var gruposVM = new List<GroupViewModel>();

            foreach (var g in gruposDb)
            {
                var teams = classificacoesDb
                    .Where(c => c.grupo == g.id)
                    .OrderByDescending(c => c.pontos)
                    .ThenByDescending(c => c.vitorias)
                    .ThenByDescending(c => (c.golos_marcados ?? 0) - (c.golos_sofridos ?? 0))
                    .Select((c, index) => new TeamViewModel
                    {
                        Position = index + 1,
                        Name = equipasDb.ContainsKey(c.equipa) ? equipasDb[c.equipa] : "Desconhecida",
                        J = c.jogos_feitos ?? 0,
                        V = c.vitorias ?? 0,
                        E = c.empate ?? 0,
                        D = c.derrotas ?? 0,
                        GD = (c.golos_marcados ?? 0) - (c.golos_sofridos ?? 0),
                        GM = c.golos_marcados ?? 0,
                        P = c.pontos ?? 0
                    }).ToList();

                gruposVM.Add(new GroupViewModel
                {
                    Name = g.grupo,
                    Teams = teams
                });
            }

            // Construir os jogos
            var jogosVM = new List<JogoViewModel>();
            var calendario = new List<Game>();

            foreach (var j in jogosDb)
            {
                /*
                if (j.equipa1 == 0 || j.equipa2 == 0)
                {
                    continue;
                }*/

                var marcadoresBrutos = _context.Marcador
    .Where(m => m.jogo_id == j.Id)
    .Join(_context.Player,
          m => m.marcador,
          p => p.Id,
          (m, p) => new { Nome = p.Nome, EquipaId = m.equipa })
    .ToList();

                var marcadoresEquipa1 = marcadoresBrutos
                    .Where(m => m.EquipaId == j.equipa1)
                    .GroupBy(m => m.Nome)
                    .Select(g => $"{g.Key} {string.Concat(Enumerable.Repeat("⚽", g.Count()))}")
                    .ToList();

                var marcadoresEquipa2 = marcadoresBrutos
                    .Where(m => m.EquipaId == j.equipa2)
                    .GroupBy(m => m.Nome)
                    .Select(g => $"{g.Key} {string.Concat(Enumerable.Repeat("⚽", g.Count()))}")
                    .ToList();

                jogosVM.Add(new JogoViewModel
                {
                    Id = j.Id.ToString(),
                    equipa1 = equipasDb.ContainsKey(j.equipa1) ? equipasDb[j.equipa1] : "Desconhecida",
                    equipa2 = equipasDb.ContainsKey(j.equipa2) ? equipasDb[j.equipa2] : "Desconhecida",
                    golos_equipa1 = j.golos_equipa1.ToString(),
                    golos_equipa2 = j.golos_equipa2.ToString(),
                    grupo = gruposDb.FirstOrDefault(g => g.id == j.grupo) is var grupoObj && grupoObj != null
                            ? grupoObj.grupo
                            : "0",
                    golos1 = j.golos_equipa1,
                    golos2 = j.golos_equipa2,
                    horaPrevista = j.hora_prevista,
                    horaInicio = j.hora_inicio,
                    horaInicio2Parte = j.hora_inicio_2parte,
                    primeiraParteTerminada = j.primeira_parte_terminada,
                    terminou = j.terminado,
                    campoNumero = j.campo,
                    situacao_precaria = j.situacao_precaria,
                    Campo = "Campo " + j.campo,
                    Hora = j.hora_prevista.ToString("HH:mm"),
                    Estado = GetEstadoJogo(j),
                    Tipo = "Fase de Grupos",
                    MarcadoresEquipa1 = marcadoresEquipa1,
                    MarcadoresEquipa2 = marcadoresEquipa2,
                    hora_inicio = j.hora_inicio.ToString(),
                    começado = j.começado,
                });
            }



            var viewModel = new TorneioViewModel
            {
                Grupos = gruposVM,
                Jogos = jogosVM,
            };

            return View(viewModel);
        }

        // Função auxiliar para descrever o estado do jogo
        private string GetEstadoJogo(Game j)
        {
            if (j.terminado) return "Resultado Final";
            if (j.começado && !j.primeira_parte_terminada) return "1ªP";
            if (j.começado && j.primeira_parte_terminada && j.hora_inicio_2parte == null) return "Intervalo";
            if (j.primeira_parte_terminada && !j.terminado && j.hora_inicio_2parte != null) return "2ªP";
            if (!j.começado) return j.hora_prevista.ToString("HH:mm");
            return "";
        }


        public IActionResult Page2()
        {
            var teams = _context.Team.ToList(); // Fetch all rows from the Games table
            return View(teams);
        }

        public IActionResult Page3()
        {
            var games = _context.Game.ToList(); // Fetch all rows from the Games table
            return View(games);
        }

        public IActionResult Page4()
        {
            return View();
        }

        // Show the game info and button
        public IActionResult EditGoals(int id, string chave)
        {
            var game = _context.Game.FirstOrDefault(g => g.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            var equipa1 = _context.Team.FirstOrDefault(t => t.Id == game.equipa1);
            var equipa2 = _context.Team.FirstOrDefault(t => t.Id == game.equipa2);

            ViewBag.Equipa1Name = equipa1?.Nome ?? "Unknown";
            ViewBag.Equipa2Name = equipa2?.Nome ?? "Unknown";

            return View(game);
        }

        public IActionResult StartGame(int id)
        {
            var game = _context.Game.FirstOrDefault(g => g.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            if (game.hora_inicio == null)
            {
                game.começado = true;
                game.hora_inicio = DateTime.Now;

                try
                {
                    _context.SaveChanges();
                    return RedirectToAction("StartGame", new { id = id });
                }
                catch
                {
                    return Content("Failed to start the game. Try again.");
                }
            }

            var equipa1 = _context.Team.FirstOrDefault(t => t.Id == game.equipa1);
            var equipa2 = _context.Team.FirstOrDefault(t => t.Id == game.equipa2);

            ViewBag.Equipa1Name = equipa1?.Nome ?? "Unknown";
            ViewBag.Equipa2Name = equipa2?.Nome ?? "Unknown";
            ViewBag.HoraInicio = (game.hora_inicio_2parte ?? game.hora_inicio)?.ToString("yyyy-MM-ddTHH:mm:ss");

            int tempoPausado = game.hora_inicio_2parte == null
                ? game.tempo_pausado_1
                : game.tempo_pausado_2;

            ViewBag.TempoPausado = tempoPausado;

            ViewBag.JogadoresEquipa1 = _context.Player
                .Where(p => p.Equipa_id == game.equipa1)
                .ToList();

            ViewBag.JogadoresEquipa2 = _context.Player
                .Where(p => p.Equipa_id == game.equipa2)
                .ToList();


            return View(game);
        }

        [HttpPost]
        public IActionResult AddGoalToTeam1(int id, int id_jogador)
        {
            var game = _context.Game.FirstOrDefault(g => g.Id == id);
            if (game == null) return NotFound();

            game.golos_equipa1 += 1;

            var marcador = new Marcador
            {
                jogo_id = id,
                marcador = id_jogador,
                equipa = game.equipa1
            };

            _context.Marcador.Add(marcador);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPost]
        public IActionResult AddGoalToTeam2(int id, int id_jogador)
        {
            var game = _context.Game.FirstOrDefault(g => g.Id == id);
            if (game == null) return NotFound();

            game.golos_equipa2 += 1;

            var marcador = new Marcador
            {
                jogo_id = id,
                marcador = id_jogador,
                equipa = game.equipa2
            };

            _context.Marcador.Add(marcador);
            _context.SaveChanges();

            return Ok();
        }

        [HttpGet]
        public IActionResult HandleGameInput(int id, string chave)
        {
            var game = _context.Game.FirstOrDefault(g => g.Id == id && g.Chave == chave || g.Chave == "adminserta");
            if (game == null) return NotFound();

            if (game.começado)
                return RedirectToAction("StartGame", new { id });

            return RedirectToAction("EditGoals", new { id });
        }


        [HttpPost]
        public IActionResult TerminarPrimeiraParte(int id)
        {
            var game = _context.Game.FirstOrDefault(g => g.Id == id);
            if (game == null) return NotFound();

            game.primeira_parte_terminada = true;
            _context.SaveChanges();

            return RedirectToAction("StartGame", new { id });
        }


        [HttpPost]
        public IActionResult ComecarSegundaParte(int id)
        {
            var game = _context.Game.FirstOrDefault(g => g.Id == id);
            if (game == null) return NotFound();

            game.hora_inicio_2parte = DateTime.Now;
            _context.SaveChanges();

            return RedirectToAction("StartGame", new { id });
        }


        [HttpPost]
        public IActionResult AtualizarTempoPausado(int id, int tempoPausado)
        {
            var game = _context.Game.FirstOrDefault(g => g.Id == id);
            if (game == null) return NotFound();

            if (game.hora_inicio_2parte == null)
                game.tempo_pausado_1 = tempoPausado;
            else
                game.tempo_pausado_2 = tempoPausado;

            _context.SaveChanges();

            return Ok();
        }


        [HttpPost]
        public IActionResult TerminarJogo(int id)
        {
            var game = _context.Game.FirstOrDefault(g => g.Id == id);
            if (game == null) return NotFound();

            var equipa1Stats = _context.Classificacoes.FirstOrDefault(c => c.equipa == game.equipa1);
            var equipa2Stats = _context.Classificacoes.FirstOrDefault(c => c.equipa == game.equipa2);

            if (equipa1Stats != null && equipa2Stats != null)
            {

                // Jogos feitos
                equipa1Stats.jogos_feitos += 1;
                equipa2Stats.jogos_feitos += 1;

                // Golos marcados e sofridos
                equipa1Stats.golos_marcados += game.golos_equipa1;
                equipa1Stats.golos_sofridos += game.golos_equipa2;
                equipa2Stats.golos_marcados += game.golos_equipa2;
                equipa2Stats.golos_sofridos += game.golos_equipa1;

                if (game.golos_equipa1 > game.golos_equipa2)
                {
                    // Vitória equipa1
                    equipa1Stats.vitorias += 1;
                    equipa1Stats.pontos += 3;
                    equipa2Stats.derrotas += 1;
                }
                else if (game.golos_equipa2 > game.golos_equipa1)
                {
                    // Vitória equipa2
                    equipa2Stats.vitorias += 1;
                    equipa2Stats.pontos += 3;
                    equipa1Stats.derrotas += 1;
                }
                else
                {
                    // Empate
                    equipa1Stats.empate += 1;
                    equipa2Stats.empate += 1;
                    equipa1Stats.pontos += 1;
                    equipa2Stats.pontos += 1;
                }

                _context.Classificacoes.Update(equipa1Stats);
                _context.Classificacoes.Update(equipa2Stats);
            }

            game.terminado = true;
            _context.SaveChanges();

            return RedirectToAction("StartGame", new { id });
        }


        /*
        [HttpPost]
        public IActionResult AdicionarMarcador(int jogoId, int equipaId, int jogadorId)
        {
            var marcador = new Marcador
            {
                jogo_id = jogoId,
                equipa = equipaId,
                marcador = jogadorId
            };

            _context.Marcador.Add(marcador);
            _context.SaveChanges();

            return RedirectToAction("Torneio");
        }*/


        public IActionResult Soon()
        {
            return View();
        }

        public IActionResult Teste()
        {
            var model = new List<LiveModel>();
            var jogos = _context.Game.ToList();

            var equipasDb = _context.Team.ToDictionary(e => e.Id, e => e.Nome);
            var gruposDb = _context.Grupos.ToList();

            foreach (var jogo in jogos)
            {
                DateTime dateTime = DateTime.Now;
                string tempoJogo = "";

                string estado = GetEstadoJogo(jogo);

                if (estado == "1ªP")
                {
                    tempoJogo = (DateTime.Now - jogo.hora_inicio!.Value).ToString(@"mm\:ss");
                }
                else if (estado == "2ªP")
                {
                    tempoJogo = (DateTime.Now - jogo.hora_inicio_2parte!.Value).ToString(@"mm\:ss");
                }
                else
                {
                    tempoJogo = (DateTime.Now - DateTime.Now).ToString(@"mm\:ss");
                }
                model.Add(new LiveModel
                {
                    Id = jogo.Id.ToString(),
                    equipa1 = equipasDb.ContainsKey(jogo.equipa1) ? equipasDb[jogo.equipa1] : "Desconhecida",
                    equipa2 = equipasDb.ContainsKey(jogo.equipa1) ? equipasDb[jogo.equipa1] : "Desconhecida",
                    golos_equipa1 = jogo.golos_equipa1.ToString(),
                    golos_equipa2 = jogo.golos_equipa2.ToString(),
                    Tempo = tempoJogo,
                    Estado = estado,
                    Hora = jogo.hora_prevista.ToString(),
                    grupo = gruposDb.FirstOrDefault(g => g.id == jogo.grupo) is var grupoObj && grupoObj != null
                            ? grupoObj.grupo
                            : "0",
                    Fase = jogo.situacao_precaria

                });
            }
            return View();
        }

        public IActionResult Historia()
        {
            return View();
        }
        public IActionResult Info()
        {
            return View();
        }
        public IActionResult Patrocinadores()
        {
            return View();
        }

        public IActionResult Index()
        {
            //return View(model);
            return View("Soon");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
