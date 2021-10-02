using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Futebox.DB.Mappers;

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
                config.ForDommel();
            });
        }
    }
}
