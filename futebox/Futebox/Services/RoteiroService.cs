using Futebox.Services.Roteiros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.Services
{
    public class RoteiroService<T> where T : IRoteiro
    {
        public T Roteirizador { get; set; }
        public RoteiroService()
        {
            Roteirizador = Activator.CreateInstance<T>();
        }
    }
}
