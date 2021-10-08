using Futebox.Interfaces.DB;
using Futebox.Models;
using Futebox.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Futebox.Models.Enums;

namespace Futebox.Services
{
    public class PartidasService : IPartidasService
    {
        readonly ICacheHandler _cache;
        readonly IFootstatsService _footstatsService;
        readonly ITimeRepositorio _timeRepositorio;
        public PartidasService(ICacheHandler cache, IFootstatsService footstatsService, ITimeRepositorio timeRepositorio)
        {
            _cache = cache;
            _footstatsService = footstatsService;
            _timeRepositorio = timeRepositorio;
        }

        public IEnumerable<PartidaVM> ObterPartidasHoje(bool skipCache = false)
        {
            var hoje = DateTime.Now.ToString("yyyyMMdd");
            var cacheName = $"{nameof(FootstatsPartida)}{hoje}";
            var resultado = _cache.ObterConteudo<List<FootstatsPartida>>(cacheName);
            if (resultado == null || resultado?.Count == 0 || skipCache)
            {
                resultado = _footstatsService.ObterPartidasHoje();
                _cache.DefinirConteudo(cacheName, resultado, 24);
            }

            var campeonatos = new int[] { (int)Campeonatos.BrasileiraoSerieA, (int)Campeonatos.BrasileiraoSerieB };
            resultado = resultado.FindAll(_ => campeonatos.Contains(_.idCampeonato));

            return resultado.Select(_ => ConverterEmPartidaVM(_));
        }

        public IEnumerable<PartidaVM> ObterPartidasDaRodada(Campeonatos campeonato, int rodada, bool skipCache = false)
        {
            var hoje = DateTime.Now.ToString("yyyyMMdd");
            var cacheName = $"{nameof(FootstatsPartida)}{hoje}";
            var resultado = _cache.ObterConteudo<List<FootstatsPartida>>(cacheName);
            if (resultado == null || resultado?.Count == 0 || skipCache)
            {
                resultado = _footstatsService.ObterPartidasDaRodada(campeonato, rodada);
                _cache.DefinirConteudo(cacheName, resultado, 24);
            }

            return resultado.Select(_ => ConverterEmPartidaVM(_));
        }

        public string ObterRoteiroDaPartida(int idPartida)
        {
            var partidas = ObterPartidasHoje();
            var partida = partidas.First(_ => _.idExterno == idPartida);
            return ObterRoteiroDaPartida(partida);
        }

        public string ObterRoteiroDaPartida(PartidaVM partida)
        {
            var empate = int.Parse(partida.golsMandante) == int.Parse(partida.golsVisitante);

            var vencedor = int.Parse(partida.golsMandante) > int.Parse(partida.golsVisitante) ? partida.timeMandante : partida.timeVisitante;
            var perdedor = int.Parse(partida.golsMandante) < int.Parse(partida.golsVisitante) ? partida.timeMandante : partida.timeVisitante;

            string mensagem =
                empate ? RoteiroDefaults.ObterTrechoEmpate(partida)
                    : RoteiroDefaults.ObterTrechoVencedor(partida, vencedor, perdedor);

            return $"{RoteiroDefaults.ObterSaudacao()}" +
                $"\n{mensagem}";
        }

        public Tuple<string, string> ObterAtributosDoVideo(PartidaVM partida)
        {
            var titulo = $"{partida.timeMandante.nome} x {partida.timeVisitante.nome} " +
                $"- {partida.dataPartida.ToString("dd/MM/yyyy")}" +
                $"- {partida.campeonato}! " +
                $"Quem venceu!? #shorts";
            var descricao = $"Fala meus parças, agora trarei #shorts com os finais de partida. Espero que gostem!" +
                $"#short\n#short\n{partida.campeonato}\n{partida.estadio}";
            return Tuple.Create(titulo, descricao);
        }

        public PartidaVM ConverterEmPartidaVM(FootstatsPartida footstatsPartida)
        {
            var idExternoMandante = footstatsPartida.idEquipeMandante.ToString();
            var idExternoVisitante = footstatsPartida.idEquipeVisitante.ToString();

            var timeMandante = _timeRepositorio.GetSingle(_ => _.origem_ext_id == idExternoMandante);
            var timeVisitante = _timeRepositorio.GetSingle(_ => _.origem_ext_id == idExternoVisitante);
            var dtPartida =
                new DateTime(footstatsPartida.dataDaPartida?.year ?? 1990, 1, 1)
                .AddDays((footstatsPartida.dataDaPartida?.dayOfYear ?? 1) - 1)
                .AddHours(footstatsPartida.dataDaPartida?.hour ?? 0)
                .AddMinutes(footstatsPartida.dataDaPartida?.minute ?? 0);
            return new PartidaVM()
            {
                idExterno = footstatsPartida.id,
                timeMandante = timeMandante,
                timeVisitante = timeVisitante,
                golsMandante = string.IsNullOrEmpty(footstatsPartida.placar.golsMandante) ? "0" : footstatsPartida.placar.golsMandante,
                golsVisitante = string.IsNullOrEmpty(footstatsPartida.placar.golsVisitante) ? "0" : footstatsPartida.placar.golsVisitante,
                dataHoraDaPartida = dtPartida.ToString("dddd, dd MMMM yyyy - HH:mm"),
                dataPartida = dtPartida,
                estadio = footstatsPartida.estadio,
                campeonato = ObterNomeDoCampeonato((Campeonatos)footstatsPartida.idCampeonato),
                rodada = footstatsPartida.rodada
            };
        }

        private string ObterNomeDoCampeonato(Campeonatos camp)
        {
            switch (camp)
            {
                case Campeonatos.BrasileiraoSerieA: return "Brasileirão Série A";
                case Campeonatos.BrasileiraoSerieB: return "Brasileirão Série B";
            }
            return null;
        }
    }
}
