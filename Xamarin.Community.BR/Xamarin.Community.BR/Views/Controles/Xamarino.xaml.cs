
using Xamarin.Forms;
using Xamarin.Forms.Shapes;
using Xamarin.Forms.Xaml;

namespace Xamarin.Community.BR.Views.Controles
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Xamarino : ContentView
    {
        private const string OLHO_DIREITO_PISCANDO = "M223.5,67l-17.1-16.2c-1.3-1.2-3.1-1.9-5.1-1.9c-0.8,0-1.5,0.1-2.3,0.3c-2.5,0.7-4.2,2.6-4.2,4.6v32.1      c0,2,1.7,3.9,4.2,4.6c0.7,0.2,1.5,0.3,2.3,0.3c2,0,3.8-0.7,5-1.8l17.1-16c0.9-0.9,1.5-2,1.5-3.1C225,69,224.5,67.9,223.5,67z       M218.3,71l-14.5,13.6c-0.3,0.3-0.7,0.4-1.1,0.4c-0.2,0-0.3,0-0.5-0.1c-0.6-0.2-1-0.7-1-1.4V71.5l17.5-1.2      C218.6,70.5,218.5,70.8,218.3,71L218.3,71z";
        private const string OLHO_DIREITO_FECHADO = "M381,67L364,50.9c-1.3-1.2-3.1-1.9-5.1-1.9c-0.8,0-1.5,0.1-2.3,0.3c-2.5,0.7-4.2,2.6-4.2,4.6v32.1      c0,2,1.7,3.9,4.2,4.6c0.7,0.2,1.5,0.3,2.3,0.3c2,0,3.8-0.7,5-1.8l17.1-16c0.9-0.9,1.5-2,1.5-3.1C382.4,69,381.9,67.9,381,67z";
        private const string OLHO_DIREITO_ABERTO = "M37.5,53.9v32.1c0,2,1.7,3.9,4.2,4.6c0.7,0.2,1.5,0.3,2.3,0.3c2,0,3.8-0.7,5-1.8l17.1-16      c0.9-0.9,1.5-2,1.5-3.1c0-1.1-0.5-2.2-1.4-3.1L49,50.9C47.8,49.7,45.9,49,44,49c-0.8,0-1.5,0.1-2.3,0.3      C39.2,50,37.5,51.9,37.5,53.9z M43.7,56.4c0-0.6,0.4-1.2,1.1-1.4c0.5-0.2,1.2,0,1.6,0.3L60.8,69c0.3,0.3,0.4,0.6,0.4,1      s-0.2,0.7-0.4,1v0L46.3,84.6c-0.3,0.3-0.7,0.4-1.1,0.4c-0.2,0-0.3,0-0.5-0.1c-0.6-0.2-1-0.7-1-1.4V56.4z";

        private const string OLHO_ESQUERDO_PISCANDO = "M255.8,49.3C255.8,49.3,255.8,49.3,255.8,49.3c-0.7-0.2-1.5-0.3-2.3-0.3c-2,0-3.8,0.7-5,1.8l-17.1,16      c-0.9,0.9-1.5,2-1.5,3.1c0,1.1,0.5,2.2,1.4,3.1l17.1,16.2c1.8,1.7,4.7,2.3,7.3,1.5c2.5-0.7,4.2-2.6,4.2-4.6V53.9      C260,51.9,258.3,50.1,255.8,49.3z M252.7,85c-0.1,0-0.3,0.1-0.5,0.1c-0.4,0-0.8-0.2-1.1-0.4L236.7,71c-0.2-0.2-0.4-0.5-0.4-0.8      l17.5,1.2v12.2C253.7,84.2,253.3,84.8,252.7,85z";
        private const string OLHO_ESQUERDO_FECHADO = "M413.3,49.3L413.3,49.3c-0.7-0.2-1.5-0.3-2.3-0.3c-2,0-3.8,0.7-5,1.8l-17.1,16c-0.9,0.9-1.5,2-1.5,3.1      c0,1.1,0.5,2.2,1.4,3.1l17.1,16.2c1.8,1.7,4.7,2.3,7.3,1.5c2.5-0.7,4.2-2.6,4.2-4.6V53.9C417.5,51.9,415.8,50.1,413.3,49.3z";
        private const string OLHO_ESQUERDO_ABERTO = "M91,50.8L74,66.8c-0.9,0.9-1.5,2-1.5,3.1c0,1.1,0.5,2.2,1.4,3.1L91,89.1c1.8,1.7,4.7,2.3,7.3,1.5      c2.5-0.7,4.2-2.6,4.2-4.6V53.9c0-2-1.7-3.9-4.2-4.6c0,0,0,0,0,0C97.6,49.1,96.8,49,96,49C94.1,49,92.3,49.7,91,50.8z M96.3,83.6      c0,0.6-0.4,1.2-1.1,1.4c-0.1,0-0.3,0.1-0.5,0.1c-0.4,0-0.8-0.2-1.1-0.4L79.2,71c-0.3-0.3-0.4-0.6-0.4-1s0.2-0.7,0.4-1l14.5-13.6      c0.4-0.4,1-0.5,1.6-0.3c0.6,0.2,1.1,0.7,1.1,1.4V83.6z";

        private readonly PathGeometryConverter _conversor = new PathGeometryConverter();

        public Xamarino()
        {
            InitializeComponent();
        }

        public void IniciarAnimacao()
        {
            var animacao = new Animation(XamarinoAnimacao);
            animacao.Commit(this, "Xamarino_Piscando", rate: 32, length: 4000, repeat: () => true);
        }

        private void XamarinoAnimacao(double frame)
        {
            if (frame > .99) {
                AtualizarOlhos(OLHO_DIREITO_FECHADO, OLHO_ESQUERDO_FECHADO, -314);
            }
            else if (frame > .95) {
                AtualizarOlhos(OLHO_DIREITO_PISCANDO, OLHO_ESQUERDO_PISCANDO, -157);
            }
            else {
                AtualizarOlhos(OLHO_DIREITO_ABERTO, OLHO_ESQUERDO_ABERTO);
            }
        }

        private void AtualizarOlhos(string olhoDireito, string olhoEsquerdo, double posicaoX = 0)
        {
            var geometria = (Geometry)_conversor.ConvertFromInvariantString(olhoDireito);
            olho_direito.Data = geometria;
            olho_direito.RenderTransform = new TranslateTransform(posicaoX, 0);

            geometria = (Geometry)_conversor.ConvertFromInvariantString(olhoEsquerdo);
            olho_esquerdo.Data = geometria;
            olho_esquerdo.RenderTransform = olho_direito.RenderTransform;
        }
    }
}
