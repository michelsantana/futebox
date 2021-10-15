using Futebox.DB;
using Futebox.Interfaces.DB;
using Futebox.Models;
using Futebox.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Futebox.Models.Enums;

namespace Futebox.Services
{
    public class GerenciamentoTimesService : IGerenciamentoTimesService
    {
        readonly ICacheHandler _cache;
        readonly ILogger<IGerenciamentoTimesService> _logger;
        readonly IFootstatsService _footstatsService;
        readonly ITimeRepositorio _timeRepositorio;

        readonly string tempPath = Path.GetTempPath();

        public GerenciamentoTimesService(ICacheHandler cache, ITimeRepositorio timeRepositorio, IFootstatsService footstatsService, ILogger<IGerenciamentoTimesService> logger)
        {
            _cache = cache;
            _footstatsService = footstatsService;
            _timeRepositorio = timeRepositorio;
            _logger = logger;
        }

        public List<FootstatsTime> ObterTimesServico()
        {
            var cacheName = nameof(FootstatsTime);
            var resultado = _cache.ObterConteudo<List<FootstatsTime>>(cacheName);
            if (resultado == null)
            {
                resultado = _footstatsService.ObterTimesServico();
                _cache.DefinirConteudo(cacheName, resultado, 48);
            }
            return resultado;
        }

        public List<Time> ObterTimesBancoDados()
        {
            var origem = $"{OrigemDado.footstats}";
            return _timeRepositorio.GetList(_ => _.origemDado == origem)?.ToList();
        }

        public Time SalvarTime(FootstatsTime footstatsTime)
        {
            _timeRepositorio.OpenTransaction();

            var idExterno = footstatsTime.id.ToString();

            var timeExistente = _timeRepositorio.GetList(_ => _.origem_ext_id == idExterno);
            if (timeExistente.Any()) return timeExistente.First();

            var time = ConverterParaTime(footstatsTime);
            time.logoBin = ObterBytesBrasaoTime(time.urlLogo);
            _timeRepositorio.Insert(ref time);

            _timeRepositorio.Commit();
            return time;
        }

        public bool RemoverTimeDB(string id)
        {
            _timeRepositorio.OpenTransaction();
            var timeExistente = _timeRepositorio.GetList(_ => _.id == id);
            if (!timeExistente.Any()) return true;
            _timeRepositorio.Delete(id);
            _timeRepositorio.Commit();
            return true;
        }

        private Time ConverterParaTime(FootstatsTime footstatsTime)
        {
            var time = new Time()
            {
                criacao = DateTime.Now,
                logoBin = null,
                cidade = footstatsTime.cidade,
                estado = footstatsTime.estado,
                grupo = footstatsTime.grupo,
                hasScout = footstatsTime.hasScout,
                idTecnico = footstatsTime.idTecnico,
                isTimeGrande = footstatsTime.isTimeGrande,
                nome = footstatsTime.nome,
                nomeAdaptadoWatson = footstatsTime.nome,
                origemDado = $"{OrigemDado.footstats}",
                origem_ext_equipe_id = footstatsTime?.sde?.equipe_id.ToString(),
                origem_ext_id = footstatsTime.id.ToString(),
                pais = footstatsTime.pais,
                selecao = footstatsTime.selecao,
                sigla = footstatsTime.sigla,
                tecnico = footstatsTime.tecnico,
                timeFantasia = footstatsTime.timeFantasia,
                torcedorNoPlural = footstatsTime.torcedorNoPlural,
                torcedorNoSingular = footstatsTime.torcedorNoSingular,
                urlLogo = footstatsTime.urlLogo
            };
            return time;
        }

        private string ObterBytesBrasaoTime(string urlLogo)
        {
            var downloadFileName = $"{tempPath}/footlogo.png";
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(new Uri(urlLogo), downloadFileName);
            }
            return Convert.ToBase64String(File.ReadAllBytes(downloadFileName));
        }
    }
}