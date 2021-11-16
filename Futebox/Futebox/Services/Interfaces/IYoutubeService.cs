using Futebox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Futebox.Services.Interfaces
{
    public interface IYoutubeService
    {
        bool IsLogged();
        void DoLogin();
        void DoLogout();
        Task Upload(Processo processo);
    }
}
