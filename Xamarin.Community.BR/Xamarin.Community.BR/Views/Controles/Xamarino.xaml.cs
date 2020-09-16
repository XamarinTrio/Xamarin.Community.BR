using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace Xamarin.Community.BR.Views.Controles
{
    public partial class Xamarino : ContentView
    {
        private readonly PathGeometryConverter _conversor = new PathGeometryConverter();

        public static readonly BindableProperty PosicaoProperty = BindableProperty.Create(
            nameof(Posicao),
            typeof(XamarinoPosicao),
            typeof(Xamarino),
            XamarinoPosicao.Frente,
            BindingMode.OneWay,
            propertyChanged: AlterarPosicao);

        private static readonly BindableProperty EscalaSVGProperty = BindableProperty.Create(
            nameof(EscalaSVG),
            typeof(double),
            typeof(Xamarino),
            1.0,
            propertyChanged: AlterarEscalaSVG);

        private OlhosFrames olhos = OlhosFrames.Frente();

        private double EscalaSVG
        {
            get => (double)GetValue(EscalaSVGProperty);
            set => SetValue(EscalaSVGProperty, value);
        }

        public XamarinoPosicao Posicao
        {
            get => (XamarinoPosicao)GetValue(PosicaoProperty);
            set => SetValue(PosicaoProperty, value);
        }

        public Xamarino()
        {
            InitializeComponent();
            fundo.Data = (Geometry)_conversor.ConvertFromInvariantString(Constantes.SVG.Xamarino.FUNDO);
            AtualizarOlhos(olhos.Direito.Aberto, olhos.Esquerdo.Aberto);
        }

        public void IniciarAnimacao()
        {
            var animacao = new Animation(XamarinoAnimacao);
            animacao.Commit(this, "Xamarino_Piscando", rate: 32, length: 4000, repeat: () => true);
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (grid != null && grid.Height > 0)
                EscalaSVG = (grid.Height / 116.1);
        }

        private void XamarinoAnimacao(double frame)
        {
            if (frame > .99)
            {
                AtualizarOlhos(olhos.Direito.Fechado, olhos.Esquerdo.Fechado);
            }
            else if (frame > .95)
            {
                AtualizarOlhos(olhos.Direito.Piscando, olhos.Esquerdo.Piscando);
            }
            else
            {
                AtualizarOlhos(olhos.Direito.Aberto, olhos.Esquerdo.Aberto);
            }
        }

        private void AtualizarOlhos(string olhoDireito, string olhoEsquerdo)
        {
            if (string.IsNullOrWhiteSpace(olhoEsquerdo) || string.IsNullOrWhiteSpace(olhoDireito))
                return;

            var centroX = fundo.Width / 2;
            var geometria = (Geometry)_conversor.ConvertFromInvariantString(olhoDireito);
            olho_direito.Data = geometria;
            olho_direito.RenderTransform = new ScaleTransform(EscalaSVG, EscalaSVG, centroX, 0);

            geometria = (Geometry)_conversor.ConvertFromInvariantString(olhoEsquerdo);
            olho_esquerdo.Data = geometria;
            olho_esquerdo.RenderTransform = new ScaleTransform(EscalaSVG, EscalaSVG, centroX, 0);
        }

        private static void AlterarEscalaSVG(BindableObject bindable, object valorAtual, object novoValor)
        {
            if (bindable is Xamarino xamarino)
            {
                var novaEscala = (double)novoValor;
                var fundo = xamarino.fundo;
                var centroX = fundo.Width / 2;

                fundo.RenderTransform = new ScaleTransform(novaEscala, novaEscala, centroX, 0);
            }
        }

        private static void AlterarPosicao(BindableObject bindable, object valorAtual, object novoValor)
        {
            if (bindable is Xamarino xamarino)
            {
                var posicao = (XamarinoPosicao)novoValor;

                switch (posicao)
                {
                    case XamarinoPosicao.Esquerda:
                        xamarino.olhos = OlhosFrames.Esquerda();
                        break;
                    case XamarinoPosicao.Direita:
                        xamarino.olhos = OlhosFrames.Direita();
                        break;
                    case XamarinoPosicao.Cima:
                        xamarino.olhos = OlhosFrames.Cima();
                        break;
                    case XamarinoPosicao.Baixo:
                        xamarino.olhos = OlhosFrames.Baixo();
                        break;
                    case XamarinoPosicao.Frente:
                    default:
                        xamarino.olhos = OlhosFrames.Frente();
                        break;
                }
            }
        }
    }

    public struct OlhosFrames
    {
        public OlhoFrame Esquerdo { get; set; }

        public OlhoFrame Direito { get; set; }

        public static OlhosFrames Frente()
        {
            return new OlhosFrames()
            {
                Direito = new OlhoFrame()
                {
                    Aberto = Constantes.SVG.Xamarino.Frente.OLHO_DIREITO_ABERTO,
                    Piscando = Constantes.SVG.Xamarino.Frente.OLHO_DIREITO_PISCANDO,
                    Fechado = Constantes.SVG.Xamarino.Frente.OLHO_DIREITO_FECHADO
                },
                Esquerdo = new OlhoFrame()
                {
                    Aberto = Constantes.SVG.Xamarino.Frente.OLHO_ESQUERDO_ABERTO,
                    Piscando = Constantes.SVG.Xamarino.Frente.OLHO_ESQUERDO_PISCANDO,
                    Fechado = Constantes.SVG.Xamarino.Frente.OLHO_ESQUERDO_FECHADO
                }
            };
        }

        public static OlhosFrames Esquerda()
        {
            return new OlhosFrames()
            {
                Direito = new OlhoFrame()
                {
                    Aberto = Constantes.SVG.Xamarino.Esquerda.OLHO_DIREITO_ABERTO,
                    Piscando = Constantes.SVG.Xamarino.Esquerda.OLHO_DIREITO_PISCANDO,
                    Fechado = Constantes.SVG.Xamarino.Esquerda.OLHO_DIREITO_FECHADO
                },
                Esquerdo = new OlhoFrame()
                {
                    Aberto = Constantes.SVG.Xamarino.Esquerda.OLHO_ESQUERDO_ABERTO,
                    Piscando = Constantes.SVG.Xamarino.Esquerda.OLHO_ESQUERDO_PISCANDO,
                    Fechado = Constantes.SVG.Xamarino.Esquerda.OLHO_ESQUERDO_FECHADO
                }
            };
        }

        public static OlhosFrames Direita()
        {
            return new OlhosFrames()
            {
                Direito = new OlhoFrame()
                {
                    Aberto = Constantes.SVG.Xamarino.Direita.OLHO_DIREITO_ABERTO,
                    Piscando = Constantes.SVG.Xamarino.Direita.OLHO_DIREITO_PISCANDO,
                    Fechado = Constantes.SVG.Xamarino.Direita.OLHO_DIREITO_FECHADO
                },
                Esquerdo = new OlhoFrame()
                {
                    Aberto = Constantes.SVG.Xamarino.Direita.OLHO_ESQUERDO_ABERTO,
                    Piscando = Constantes.SVG.Xamarino.Direita.OLHO_ESQUERDO_PISCANDO,
                    Fechado = Constantes.SVG.Xamarino.Direita.OLHO_ESQUERDO_FECHADO
                }
            };
        }

        public static OlhosFrames Cima()
        {
            return new OlhosFrames()
            {
                Direito = new OlhoFrame()
                {
                    Aberto = Constantes.SVG.Xamarino.Cima.OLHO_DIREITO_ABERTO,
                    Piscando = Constantes.SVG.Xamarino.Cima.OLHO_DIREITO_PISCANDO,
                    Fechado = Constantes.SVG.Xamarino.Cima.OLHO_DIREITO_FECHADO
                },
                Esquerdo = new OlhoFrame()
                {
                    Aberto = Constantes.SVG.Xamarino.Cima.OLHO_ESQUERDO_ABERTO,
                    Piscando = Constantes.SVG.Xamarino.Cima.OLHO_ESQUERDO_PISCANDO,
                    Fechado = Constantes.SVG.Xamarino.Cima.OLHO_ESQUERDO_FECHADO
                }
            };
        }

        public static OlhosFrames Baixo()
        {
            return new OlhosFrames()
            {
                Direito = new OlhoFrame()
                {
                    Aberto = Constantes.SVG.Xamarino.Baixo.OLHO_DIREITO_ABERTO,
                    Piscando = Constantes.SVG.Xamarino.Baixo.OLHO_DIREITO_PISCANDO,
                    Fechado = Constantes.SVG.Xamarino.Baixo.OLHO_DIREITO_FECHADO
                },
                Esquerdo = new OlhoFrame()
                {
                    Aberto = Constantes.SVG.Xamarino.Baixo.OLHO_ESQUERDO_ABERTO,
                    Piscando = Constantes.SVG.Xamarino.Baixo.OLHO_ESQUERDO_PISCANDO,
                    Fechado = Constantes.SVG.Xamarino.Baixo.OLHO_ESQUERDO_FECHADO
                }
            };
        }
    }

    public struct OlhoFrame
    {
        public string Aberto { get; set; }

        public string Piscando { get; set; }

        public string Fechado { get; set; }

    }

    public enum XamarinoPosicao
    {
        Frente = 0,
        Esquerda = 1,
        Direita = 2,
        Cima = 3,
        Baixo = 4
    }
}
