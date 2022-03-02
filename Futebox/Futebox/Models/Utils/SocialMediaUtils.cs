using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.Models
{
    public class SocialMediaUtils
    {
        public static Dimensao Size(Enums.RedeSocialFinalidade social)
        {
            return new Dimensao(Width(social), Height(social));
        }

        public static int Width(Enums.RedeSocialFinalidade social)
        {
            switch (social)
            {
                case Enums.RedeSocialFinalidade.NENHUMA: return 1920;
                case Enums.RedeSocialFinalidade.YoutubeShorts: return 1080;
                case Enums.RedeSocialFinalidade.YoutubeVideo: return 1920;
                case Enums.RedeSocialFinalidade.InstagramVideo: return 1080;
                default: return 1920;
            }
        }

        public static int Height(Enums.RedeSocialFinalidade social)
        {
            switch (social)
            {
                case Enums.RedeSocialFinalidade.NENHUMA: return 1080;
                case Enums.RedeSocialFinalidade.YoutubeShorts: return 1920;
                case Enums.RedeSocialFinalidade.YoutubeVideo: return 1080;
                case Enums.RedeSocialFinalidade.InstagramVideo: return 1080;
                default: return 1080;
            }
        }
    }
}
