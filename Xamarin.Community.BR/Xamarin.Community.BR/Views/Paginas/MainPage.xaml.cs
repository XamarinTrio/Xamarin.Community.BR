using System.ComponentModel;
using Xamarin.Forms;

namespace Xamarin.Community.BR.Views.Paginas
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            navegacao.IniciarAnimacao();
            mapa.ResetarMapa();
        }
    }
}
