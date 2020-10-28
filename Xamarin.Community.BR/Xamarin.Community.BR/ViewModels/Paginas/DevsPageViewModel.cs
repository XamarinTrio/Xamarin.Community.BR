using System.Windows.Input;
using Xamarin.Forms;

namespace Xamarin.Community.BR.ViewModels.Paginas
{
    public sealed class DevsPageViewModel : BaseViewModel
    {
        public ICommand PesquisaCommand { get; }

        private string texto;

        public string Texto
        {
            get => texto;
            set => SetProperty(ref texto, value);
        }

        public DevsPageViewModel()
        {
            PesquisaCommand = new Command(PesquisaCommandExecute);
        }

        private void PesquisaCommandExecute(object obj)
        {
            //Todo
        }
    }
}
