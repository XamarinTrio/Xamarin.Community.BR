using System.Threading.Tasks;

namespace Xamarin.Community.BR.Abstractions
{
    public interface INavegacaoService
    {
        void Init();
        Task NavegarAsync(string url);
    }
}
