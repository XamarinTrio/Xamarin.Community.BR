using System;
using System.Threading;
using System.Threading.Tasks;
using TinyIoC;
using Xamarin.Community.BR.Abstractions;
using Xamarin.Community.BR.Helpers;
using Xamarin.Forms;

namespace Xamarin.Community.BR.Services
{
    public class NavegacaoService : INavegacaoService
    {
        private TinyIoCContainer _container;
        private CancellationTokenSource cancellationTokenSource;

        public NavegacaoService(TinyIoCContainer container)
        {
            _container = container;
        }

        public void Init()
        {
            Application.Current.MainPage = new NavigationPage();
        }

        public async Task NavegarAsync(string url)
        {
            if(Application.Current.MainPage is NavigationPage nav)
            {
                var tipoPagina = Type.GetType($"Xamarin.Community.BR.Views.Paginas.{url}");
                var pagina = (Page)_container.Resolve(tipoPagina);

                ViewModelLocator.LocalizaViewModel(pagina, _container);

                cancellationTokenSource?.Cancel();
                cancellationTokenSource?.Dispose();
                cancellationTokenSource = new CancellationTokenSource();

                await nav.PushAsync(pagina).ConfigureAwait(false);

                if(pagina.BindingContext is ICarregar carregar)
                {
                    await carregar.CarregarAsync(cancellationTokenSource.Token).ConfigureAwait(false);
                }
            }
        }
    }
}
