using System.Threading.Tasks;

namespace Xamarin.Community.BR.Abstractions
{
    public interface INavegacaoService
    {
        Task NavegarAsync(string url);
        Task NavegarAsync(string url, bool animado);
    }
}
