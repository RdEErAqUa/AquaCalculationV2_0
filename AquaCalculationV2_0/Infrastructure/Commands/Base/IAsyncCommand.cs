using System.Threading.Tasks;
using System.Windows.Input;

namespace AquaCalculationV2_0.Infrastructure.Commands.Base
{
    internal interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object p);
    }
}
