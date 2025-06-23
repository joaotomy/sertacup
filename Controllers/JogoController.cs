using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SertaCup_site.Data;
using System.Collections.Generic;
using SertaCup_site.Models;
using System.Diagnostics;

namespace SertaCup_site.Controllers
{
    public class JogoController : Controller
    {
        private readonly ILogger<JogoController> _logger;
        private readonly ApplicationDbContext _context;

        public JogoController(ILogger<JogoController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Calendario()
        {
            var jogos = _context.Game.ToList();
            var viewModel = new List<JogoViewModel>();

            foreach (var jogo in jogos)
            {
                var equipa1 = _context.Team.FirstOrDefault(t => t.Id == jogo.equipa1);
                var equipa2 = _context.Team.FirstOrDefault(t => t.Id == jogo.equipa2);
                var grupo = _context.Grupos.FirstOrDefault(g =>
                    g.equipa1 == jogo.equipa1 || g.equipa2 == jogo.equipa1 || g.equipa3 == jogo.equipa1 || g.equipa4 == jogo.equipa1);

                string estado;
                string tipo;

                if (jogo.terminado)
                {
                    estado = "Final";
                    tipo = "finished";
                }
                else if (jogo.começado)
                {
                    estado = jogo.hora_inicio_2parte != null ? "2ª Parte" : "1ª Parte";
                    tipo = "playing";
                }
                else
                {
                    estado = jogo.hora_prevista.ToString("HH:mm");
                    tipo = "upcoming";
                }

                viewModel.Add(new JogoViewModel
                {
                    Id = (jogo.Id).ToString(),
                    equipa1 = equipa1?.Nome ?? "Equipa 1",
                    equipa2 = equipa2?.Nome ?? "Equipa 2",
                    golos_equipa1 = (jogo.golos_equipa1).ToString(),
                    golos_equipa2 = (jogo.golos_equipa2).ToString() ,
                    grupo = grupo?.grupo ?? "Grupo X",
                    Campo = "Campo 5",
                    Hora = (jogo.hora_prevista).ToString(),
                    Estado = estado,
                    Tipo = tipo
                });
            }

            return View(viewModel);
        }

        public IActionResult Torneio()
        {
            var classificacoes = _context.Classificacoes.ToList();
            var equipas = _context.Team.ToDictionary(e => e.Id, e => e.Nome);
            var grupos = _context.Grupos.ToDictionary(g => g.id, g => g.grupo);
            var jogos = _context.Game.ToList();

            var gruposViewModel = classificacoes
                .GroupBy(c => c.grupo)
                .OrderBy(g => g.Key)
                .Select(g => new GroupViewModel
                {
                    Name = grupos.ContainsKey(g.Key) ? grupos[g.Key] : $"Grupo {g.Key}",
                    Teams = g
                        .OrderByDescending(t => t.pontos ?? 0)
                        .Select((c, index) => new TeamViewModel
                        {
                            Position = index + 1,
                            Name = equipas.ContainsKey(c.equipa) ? equipas[c.equipa] : $"Equipa {c.equipa}",
                            J = c.jogos_feitos ?? 0,
                            V = c.vitorias ?? 0,
                            E = c.empate ?? 0,
                            D = c.derrotas ?? 0,
                            GD = (c.golos_marcados ?? 0) - (c.golos_sofridos ?? 0),
                            P = c.pontos ?? 0
                        }).ToList()
                }).ToList();

            var jogosViewModel = new List<JogoViewModel>();

            foreach (var jogo in jogos)
            {
                var equipa1 = equipas.ContainsKey(jogo.equipa1) ? equipas[jogo.equipa1] : "Equipa 1";
                var equipa2 = equipas.ContainsKey(jogo.equipa2) ? equipas[jogo.equipa2] : "Equipa 2";
                var grupoNome = jogo.grupo.HasValue && grupos.ContainsKey(jogo.grupo.Value) ? grupos[jogo.grupo.Value] : "Grupo X";

                string estado;
                string tipo;

                if (jogo.terminado)
                {
                    estado = "Final";
                    tipo = "finished";
                }
                else if (jogo.começado)
                {
                    estado = jogo.hora_inicio_2parte != null ? "2ª Parte" : "1ª Parte";
                    tipo = "playing";
                }
                else
                {
                    estado = jogo.hora_prevista.ToString("HH:mm");
                    tipo = "upcoming";
                }

                var marcadores = _context.Marcador
                .Where(m => m.jogo_id == jogo.Id)
                .Join(_context.Player,
                      m => m.marcador,
                      p => p.Id,
                      (m, p) => new { Nome = p.Nome, EquipaId = m.equipa })
                .Select(m => $"{m.Nome} ⚽")
                .ToList();

                jogosViewModel.Add(new JogoViewModel
                {
                    Id = jogo.Id.ToString(),
                    equipa1 = equipa1,
                    equipa2 = equipa2,
                    golos_equipa1 = jogo.golos_equipa1.ToString(),
                    golos_equipa2 = jogo.golos_equipa2.ToString(),
                    grupo = grupoNome,
                    Campo = "Campo " + jogo.campo,
                    Hora = jogo.hora_prevista.ToString("HH:mm"),
                    Estado = estado,
                    Tipo = tipo,
                    //Marcadores = marcadores
                });
            }

            var torneioViewModel = new TorneioViewModel
            {
                Grupos = gruposViewModel,
                Jogos = jogosViewModel
            };

            return View(torneioViewModel);
        }
    }
}
