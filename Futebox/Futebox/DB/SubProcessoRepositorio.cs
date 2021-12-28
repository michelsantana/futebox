using Futebox.DB.Interfaces;
using Futebox.Interfaces.DB;
using Futebox.Models;
using Futebox.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Futebox.DB
{
    public class SubProcessoRepositorio : ISubProcessoRepositorio
    {
        IRepositoryBase<SubProcessoYoutubeVideo> _ytv;
        IRepositoryBase<SubProcessoYoutubeShort> _yts;
        IRepositoryBase<SubProcessoInstagramVideo> _igv;
        public SubProcessoRepositorio(IRepositoryBase<SubProcessoYoutubeVideo> ytv, IRepositoryBase<SubProcessoYoutubeShort> yts, IRepositoryBase<SubProcessoInstagramVideo> igv) 
        {
            _ytv = ytv;
            _yts = yts;
            _igv = igv;
        }

        public SubProcessoYoutubeVideo Inserir(SubProcessoYoutubeVideo sub)
        {
            _ytv.Insert(ref sub);
            return sub;
        }

        public SubProcessoYoutubeShort Inserir(SubProcessoYoutubeShort sub)
        {
            _yts.Insert(ref sub);
            return sub;
        }

        public SubProcessoInstagramVideo Inserir(SubProcessoInstagramVideo sub)
        {
            _igv.Insert(ref sub);
            return sub;
        }

        public SubProcessoYoutubeVideo Update(SubProcessoYoutubeVideo sub)
        {
            _ytv.Update( sub);
            return sub;
        }

        public SubProcessoYoutubeShort Update(SubProcessoYoutubeShort sub)
        {
            _yts.Update( sub);
            return sub;
        }

        public SubProcessoInstagramVideo Update(SubProcessoInstagramVideo sub)
        {
            _igv.Update( sub);
            return sub;
        }

        public SubProcesso Update(SubProcesso sub)
        {
            if (sub.redeSocial == RedeSocialFinalidade.YoutubeVideo) _ytv.Update((SubProcessoYoutubeVideo)sub);
            if (sub.redeSocial == RedeSocialFinalidade.YoutubeShorts) _yts.Update((SubProcessoYoutubeShort)sub);
            if (sub.redeSocial == RedeSocialFinalidade.InstagramVideo) _igv.Update((SubProcessoInstagramVideo)sub);
            return sub;
        }

        public IEnumerable<SubProcesso> GetAll()
        {
            var yts = _yts.GetList(_ => _.redeSocial == RedeSocialFinalidade.YoutubeShorts);
            var ytv = _ytv.GetList(_ => _.redeSocial == RedeSocialFinalidade.YoutubeVideo);
            var igv = _igv.GetList(_ => _.redeSocial == RedeSocialFinalidade.InstagramVideo);
            var result = new List<SubProcesso>();
            result.AddRange(yts);
            result.AddRange(ytv);
            result.AddRange(igv);
            return result;
        }
        public IEnumerable<SubProcesso> GetList(string processoId)
        {
            var yts = _yts.GetList(_ => _.processoId == processoId && _.redeSocial == RedeSocialFinalidade.YoutubeShorts);
            var ytv = _ytv.GetList(_ => _.processoId == processoId && _.redeSocial == RedeSocialFinalidade.YoutubeVideo);
            var igv = _igv.GetList(_ => _.processoId == processoId && _.redeSocial == RedeSocialFinalidade.InstagramVideo);
            var result = new List<SubProcesso>();
            result.AddRange(yts);
            result.AddRange(ytv);
            result.AddRange(igv);
            return result;
        }

        public IEnumerable<SubProcesso> GetById(string id)
        {
            var yts = _yts.GetList(_ => _.id == id && _.redeSocial == RedeSocialFinalidade.YoutubeShorts);
            var ytv = _ytv.GetList(_ => _.id == id && _.redeSocial == RedeSocialFinalidade.YoutubeVideo);
            var igv = _igv.GetList(_ => _.id == id && _.redeSocial == RedeSocialFinalidade.InstagramVideo);
            var result = new List<SubProcesso>();
            result.AddRange(yts);
            result.AddRange(ytv);
            result.AddRange(igv);
            return result;
        }
    }
}
