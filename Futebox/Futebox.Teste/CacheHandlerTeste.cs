using Futebox.Models;
using Futebox.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace Futebox.Teste
{
    public class CacheHandlerTeste
    {
        [Fact]
        public void DefinirCache_DeveSerValido_QuandoArquivoSalvo()
        {
            var _cache = new CacheHandler();
            var time = new Time() { nome = "Time" };
            var nomeDoCache = "TesteTimeCache";

            _cache.DefinirConteudo(nomeDoCache, time);

            Assert.True(File.Exists($"{_cache.ObterPastaCache()}/{nomeDoCache}.txt"));
        }

        [Fact]
        public void DefinirCache_DeveSerValido_QuandoConteudoSalvoForIgualAoRecuperado()
        {
            var _cache = new CacheHandler();
            var time = new Time() { nome = "Time" };
            var nomeDoCache = "TesteTimeCache";

            _cache.DefinirConteudo(nomeDoCache, time);
            var resultado = _cache.ObterConteudo<Time>(nomeDoCache);

            Assert.True(File.Exists($"{_cache.ObterPastaCache()}/{nomeDoCache}.txt"));
            Assert.Equal(resultado.nome, time.nome);
        }

        [Fact]
        public void DefinirCache_DeveNulo_QuandoCacheExpirado()
        {
            var _cache = new CacheHandler();
            var time = new Time() { nome = "Time" };
            var nomeDoCache = "TesteTimeCache";

            _cache.DefinirConteudo(nomeDoCache, time, -1);
            var resultado = _cache.ObterConteudo<Time>(nomeDoCache);

            Assert.True(File.Exists($"{_cache.ObterPastaCache()}/{nomeDoCache}.txt"));
            Assert.Null(resultado);
        }
    }
}
