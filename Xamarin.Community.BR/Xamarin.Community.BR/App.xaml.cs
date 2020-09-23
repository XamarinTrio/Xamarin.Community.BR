using System;
using Xamarin.Community.BR.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xamarin.Community.BR
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            TaskExtensions.DefinirAoDispararExcecaoPadrao(ex => Console.WriteLine(ex));

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
