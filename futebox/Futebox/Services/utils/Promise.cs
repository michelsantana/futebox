using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.Services.utils
{
    public class Promise
    {
        public static void All(params Action[] fns)
        {
            Task.WaitAll(fns.Select(_ => Task.Run(_)).ToArray());
        }
    }
}
