using Futebox.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.Services.utils
{
    public static class Arrow
    {        
        public static void Try<T>(this T src, Action fn, Action<Exception> fnex = null) where T : IServiceTools
        {
            try
            {
                fn();
            }
            catch (Exception ex)
            {
                EyeLog.Log(ex);
                if (fnex != null) fnex(ex);
            }
        }
    }
}
