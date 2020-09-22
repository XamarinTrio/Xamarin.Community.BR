using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Community.BR.Views.Controles;
using Xamarin.Forms;

namespace Xamarin.Community.BR
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
        }
    }
}
