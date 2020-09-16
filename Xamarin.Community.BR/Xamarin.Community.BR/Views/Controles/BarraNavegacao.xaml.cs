using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xamarin.Community.BR.Views.Controles
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BarraNavegacao : ContentView
    {
        public static readonly BindableProperty PesquisaCommandProperty =
            BindableProperty.Create(nameof(PesquisaCommand), typeof(ICommand), typeof(BarraNavegacao), defaultValue:default(ICommand), defaultBindingMode: BindingMode.OneTime);

        public static readonly BindableProperty PesquisaTextoProperty =
            BindableProperty.Create(nameof(PesquisaTexto), typeof(string), typeof(BarraNavegacao), defaultValue: string.Empty, defaultBindingMode: BindingMode.TwoWay, propertyChanged:AlterarTexto);

        private bool PesquisaHabilitada {get; set;}

        public ICommand PesquisaCommand
        {
            get => (ICommand)GetValue(PesquisaCommandProperty);
            set => SetValue(PesquisaCommandProperty, value);
        }

        public string PesquisaTexto {
            get => (string)GetValue(PesquisaTextoProperty);
            set => SetValue(PesquisaTextoProperty, value);
        }

        public BarraNavegacao()
        {
            InitializeComponent();
            pesquisaEntry.TextChanged += PesquisaEntryTextoAlterado;
        }

        public void IniciarAnimacao() =>
            xamarino.IniciarAnimacao();

        private void PesquisaEntryTextoAlterado(object sender, TextChangedEventArgs e)
        {
            PesquisaTexto = e.NewTextValue;
        }

        private async void XamarinoTapped(object sender, System.EventArgs e)
        {
            if (!PesquisaHabilitada)
                return;

            await Task.WhenAll(
                    globo.FadeTo(1),
                    pesquisaEntry.FadeTo(0),
                    xamarino.TranslateTo(0, 0, easing: Easing.SinOut));

            PesquisaHabilitada = pesquisaEntry.IsEnabled = false;
            PesquisaTexto = string.Empty;
            xamarino.Posicao = XamarinoPosicao.Frente;
        }

        private async void PesquisaTapped(object sender, System.EventArgs e)
        {
            if (PesquisaHabilitada)
            {
                if (PesquisaCommand == null)
                    return;

                if (PesquisaCommand.CanExecute(this))
                    PesquisaCommand.Execute(this);
            }
            else
            {
                var posicaoX = (globo.X - xamarino.X - (xamarino.Width / 2));
                PesquisaHabilitada = pesquisaEntry.IsEnabled = true;

                await Task.WhenAll(
                    globo.FadeTo(0),
                    pesquisaEntry.FadeTo(1),
                    xamarino.TranslateTo(posicaoX, 0, easing: Easing.SinOut));

                xamarino.Posicao = XamarinoPosicao.Esquerda;
            }
        }

        private static void AlterarTexto(BindableObject bindable, object valorAtual, object novoValor)
        {
            if(bindable is BarraNavegacao navegacao)
            {
                var pesquisaEntry = navegacao.pesquisaEntry;
                var novoTexto = (string)novoValor;
                if (pesquisaEntry.Text == novoTexto)
                    return;

                navegacao.pesquisaEntry.Text = (string)novoValor;
            }
        }
    }
}
