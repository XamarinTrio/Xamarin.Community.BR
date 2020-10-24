using TinyIoC;
using Xamarin.Community.BR.Abstractions;
using Xamarin.Community.BR.Services;
using Xamarin.Community.BR.ViewModels.Paginas;
using Xamarin.Community.BR.Views.Paginas;

namespace Xamarin.Community.BR
{
    public sealed class Startup
    {
        private readonly TinyIoCContainer _container;
        private readonly INavegacaoService _navegacao;

        public Startup()
        {
            _container = TinyIoCContainer.Current;
            _navegacao = new NavegacaoService(_container);
        }

        public void Init()
        {
            _navegacao.Init();
            _container.Register<INavegacaoService>(_navegacao);
            _container.Register<IPerfilService, PerfilService>();
            _container.Register<MainPageViewModel>();
            _container.Register<MainPage>();
        }

        public INavegacaoService PegarNavegacao()
        {
            return _navegacao;
        }
    }
}
