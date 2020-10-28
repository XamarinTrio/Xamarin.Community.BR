using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Community.BR.Abstractions;
using Xamarin.Community.BR.Extensions;
using Xamarin.Forms;

namespace Xamarin.Community.BR.ViewModels.Paginas
{
    public sealed class MainPageViewModel : BaseViewModel, ICarregar
    {
        private readonly IPerfilService _perfilService;
        private readonly INavegacaoService _navegacaoService;

        private IEnumerable<IAmACommunityMember> listaProgramadores;

        public ObservableCollection<IPin> Pins { get; }

        public ICommand PesquisaCommand { get; }

        public ICommand DevsPageCommand { get; }

        public MainPageViewModel(
            IPerfilService perfilService,
            INavegacaoService navegacaoService)
        {
            _perfilService = perfilService;
            _navegacaoService = navegacaoService;

            Pins = new ObservableCollection<IPin>();
            PesquisaCommand = new Command(PesquisaCommandExecute, (obj) => !EstaOcupado);
            DevsPageCommand = new Command(DevsPageCommandExecute, (obj) => !EstaOcupado);
        }

        public async Task CarregarAsync(CancellationToken token) =>
            await ExecutarTarefa(PopularListaProgramadoresAsync, token).ConfigureAwait(false);

        private Task PopularListaProgramadoresAsync()
        {
            listaProgramadores = _perfilService.PegarTodos();

            foreach (var programador in listaProgramadores)
            {
                var pin = programador.ToPin();
                Pins.Add(pin);
            }

            return Task.CompletedTask;
        }

        private void DevsPageCommandExecute(object obj)
        {
            _navegacaoService.NavegarAsync("/DevsPage?Texto=Olá da query string", true).TentarExecutar();
        }

        private void PesquisaCommandExecute(object obj)
        {
            //TODO: criar pesquisa
        }
    }
}
