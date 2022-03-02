using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using Futebox.DB.Mappers;
using Futebox.Models;

namespace Futebox
{
    public class RegisterMappings
    {
        public static void Register()
        {
            FluentMapper.EntityMaps.Clear();
            FluentMapper.Initialize(config =>
            {
                config.AddMap(new TimeMap());
                config.AddMap(new ProcessoMap());
                config.AddMap(new CampeonatoMap());
                //config.AddMap(new InstagramSubProcessoMap<SubProcessoInstagramVideo>());
                //config.AddMap(new YoutubeSubProcessoMap<SubProcessoYoutubeVideo>());
                //config.AddMap(new YoutubeSubProcessoMap<SubProcessoYoutubeShort>());
                config.ForDommel();
            });
        }
    }
}
