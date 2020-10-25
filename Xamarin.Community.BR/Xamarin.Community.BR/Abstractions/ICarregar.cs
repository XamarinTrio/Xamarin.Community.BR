using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Community.BR.Abstractions
{
    public interface ICarregar
    {
        Task CarregarAsync(CancellationToken token);
    }
}
