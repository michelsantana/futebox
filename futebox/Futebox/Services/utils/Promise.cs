using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.Services.utils
{
    public class Promise
    {
        public static async Task All(params Action[] fns)
        {
            await Task.WhenAll(fns.Select(_ => Task.Run(_)).ToArray());
        }
        //public static async Tuple[] All(params Action[] fns)
        //{
        //    var 
        //    var tasks = fns.Select(_ => Task.Run(_)).ToList();
        //    while (fns.Count() > 0)
        //    {
        //        Task.WhenAny(tasks.ToArray());
        //    }
        //}
    }
}
