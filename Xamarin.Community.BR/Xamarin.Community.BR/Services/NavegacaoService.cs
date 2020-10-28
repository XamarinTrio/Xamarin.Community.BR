using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using TinyIoC;
using Xamarin.Community.BR.Abstractions;
using Xamarin.Community.BR.Helpers;
using Xamarin.Forms;

namespace Xamarin.Community.BR.Services
{
    public sealed class NavegacaoService : INavegacaoService
    {
        private NavigationPage _navegacao;
        private TinyIoCContainer _container;
        private CancellationTokenSource cancellationTokenSource;

        public NavegacaoService(TinyIoCContainer container)
        {
            _container = container;
        }

        public async Task NavegarAsync(string url) =>
            await NavegarInternoAsync(url, false).ConfigureAwait(false);

        public async Task NavegarAsync(string url, bool animar) =>
            await NavegarInternoAsync(url, animar).ConfigureAwait(false);

        private async Task NavegarInternoAsync(string url, bool animar)
        {
            if (!url.StartsWith(Constantes.Navegacao.URL_RELATIVA) &&
                !url.StartsWith(Constantes.Navegacao.URL_REMOCAO))
            {
                SubstituirRaiz(url);
                return;
            }

            var uri = new Uri(url);
            var segmentos = uri.Segments.Where(s => !Constantes.Navegacao.URL_RELATIVA.Equals(s));
            foreach (var segmento in segmentos)
            {
                var segmentoDecoded = HttpUtility.UrlDecode(segmento);
                var indiceQuery = segmentoDecoded.IndexOf(Constantes.Navegacao.DELIMITADOR_QUERY) + 1;
                var query = indiceQuery > 0
                    ? segmentoDecoded.Substring(indiceQuery)
                    : null;

                var pagina = segmentoDecoded.Replace($"{Constantes.Navegacao.DELIMITADOR_QUERY}{query}", string.Empty);
                var parametros = query?
                    .Split('&')
                    .Select(q=> q.Split('='))
                    .ToDictionary(p=> p[0], p=> p[1]);

                await EmpilharPaginaAsync(pagina, parametros, animar).ConfigureAwait(false);
            }
        }

        private void SubstituirRaiz(string url)
        {
            var uri = new Uri($"/{url}");
            var nomePagina = uri.Segments.First(s => !Constantes.Navegacao.URL_RELATIVA.Equals(s));
            var pagina = ResolverPagina(nomePagina);
            Application.Current.MainPage = _navegacao = new NavigationPage(pagina);

            DispararCarregar(pagina, ResolverCancellationToken());
        }

        private async Task EmpilharPaginaAsync(string nomePagina, IDictionary<string, string> parametros, bool animar)
        {
            var pagina = ResolverPagina(nomePagina, parametros);
            await _navegacao.PushAsync(pagina, animar).ConfigureAwait(false);
            DispararCarregar(pagina, ResolverCancellationToken());
        }

        private CancellationToken ResolverCancellationToken()
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource?.Dispose();
            cancellationTokenSource = new CancellationTokenSource();

            return cancellationTokenSource.Token;
        }

        private Page ResolverPagina(string url, IDictionary<string, string> parametros = null)
        {
            var tipoPagina = Type.GetType($"Xamarin.Community.BR.Views.Paginas.{url}");
            var pagina = (Page)_container.Resolve(tipoPagina);

            ViewModelLocator.LocalizaViewModel(pagina, _container);
            ResolverParametros(pagina, parametros);

            return pagina;
        }

        private static void ResolverParametros(Page pagina, IDictionary<string, string> parametros)
        {
            if (parametros is null)
                return;

            var vmType = pagina.BindingContext.GetType();
            foreach (var parametro in parametros)
            {
                var propriedade = vmType.GetProperty(parametro.Key);
                if (propriedade != null)
                {
                    propriedade.SetValue(pagina.BindingContext, parametro.Value);
                }
            }
        }

        private void DispararCarregar(Page pagina, CancellationToken token)
        {
            if (pagina is ICarregar paginaCarregar)
                Task.Run(() => paginaCarregar.CarregarAsync(token));

            if (pagina.BindingContext is ICarregar VMcarregar)
                Task.Run(() => VMcarregar.CarregarAsync(token));
        }
    }
}
