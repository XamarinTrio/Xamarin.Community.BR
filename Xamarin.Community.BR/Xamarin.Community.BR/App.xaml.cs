using System;
using Xamarin.Community.BR.Abstractions;
using Xamarin.Community.BR.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xamarin.Community.BR
{
    public partial class App : Application
    {
        private INavegacaoService _navegacaoService;

        public App()
        {
            InitializeComponent();
            TaskExtensions.DefinirAoDispararExcecaoPadrao(ex => Console.WriteLine(ex));

            var startup = new Startup();
            startup.Init();

            _navegacaoService = startup.PegarNavegacao();

        }

        protected override void OnStart()
        {
            _navegacaoService.NavegarAsync("MainPage").TentarExecutar();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
