using Futebox.Models;
using System.Threading.Tasks;

namespace Futebox.Services.Interfaces
{
    public interface IExecucaoProcessoService
    {
        Task<RobotResultApi> Executar(string processoId);
    }
}
