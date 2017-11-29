using System.Threading.Tasks;

namespace Echelon.Core.Infrastructure.Services.Windows
{
    public interface IWindowsService
    {
        Task Initialize();
        Task Shutdown();
    }
}