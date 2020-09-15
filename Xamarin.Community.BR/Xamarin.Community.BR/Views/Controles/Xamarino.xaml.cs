
using System;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;
using Xamarin.Forms.Xaml;

namespace Xamarin.Community.BR.Views.Controles
{
    public partial class Xamarino : ContentView
    {
        private readonly PathGeometryConverter _conversor = new PathGeometryConverter();

        public static BindableProperty PosicaoProperty = BindableProperty.Create(
            nameof(Posicao),
            typeof(XamarinoPosicao),
            typeof(Xamarino),
            XamarinoPosicao.Frente,
            BindingMode.OneWay,
            propertyChanged: AlterarPosicao);

        public static BindableProperty OlhoDireitoAbertoProperty = BindableProperty.Create(
            nameof(OlhoDireitoAberto),
            typeof(string),
            typeof(Xamarino),
            SVG.XAMARINO_OLHO_DIREITO_ABERTO,
            BindingMode.OneWay);

        public static BindableProperty OlhoDireitoPiscandoProperty = BindableProperty.Create(
            nameof(OlhoDireitoPiscando),
            typeof(string),
            typeof(Xamarino),
            SVG.XAMARINO_OLHO_DIREITO_PISCANDO,
            BindingMode.OneWay);

        public static BindableProperty OlhoDireitoFechadoProperty = BindableProperty.Create(
            nameof(OlhoDireitoFechado),
            typeof(string),
            typeof(Xamarino),
            SVG.XAMARINO_OLHO_DIREITO_FECHADO,
            BindingMode.OneWay);

        public static BindableProperty OlhoEsquerdoAbertoProperty = BindableProperty.Create(
            nameof(OlhoEsquerdoAberto),
            typeof(string),
            typeof(Xamarino),
            SVG.XAMARINO_OLHO_ESQUERDO_ABERTO,
            BindingMode.OneWay);

        public static BindableProperty OlhoEsquerdoPiscandoProperty = BindableProperty.Create(
            nameof(OlhoEsquerdoPiscando),
            typeof(string),
            typeof(Xamarino),
            SVG.XAMARINO_OLHO_ESQUERDO_PISCANDO,
            BindingMode.OneWay);

        public static BindableProperty OlhoEsquerdoFechadoProperty = BindableProperty.Create(
            nameof(OlhoEsquerdoFechado),
            typeof(string),
            typeof(Xamarino),
            SVG.XAMARINO_OLHO_ESQUERDO_FECHADO,
            BindingMode.OneWay);

        private static void AlterarPosicao(BindableObject bindable, object valorAtual, object novoValor)
        {
            if(bindable is Xamarino xamarino)
            {
                var posicao = (XamarinoPosicao)novoValor;
                var resultado = VisualStateManager.GoToState(xamarino, posicao.ToString());

                switch (posicao) {
                    case XamarinoPosicao.Frente:
                        xamarino.OlhoDireitoAberto = SVG.XAMARINO_OLHO_DIREITO_ABERTO;
                        xamarino.OlhoDireitoPiscando = SVG.XAMARINO_OLHO_DIREITO_PISCANDO;
                        xamarino.OlhoDireitoFechado = SVG.XAMARINO_OLHO_DIREITO_FECHADO;

                        xamarino.OlhoEsquerdoAberto = SVG.XAMARINO_OLHO_ESQUERDO_ABERTO;
                        xamarino.OlhoEsquerdoPiscando = SVG.XAMARINO_OLHO_ESQUERDO_PISCANDO;
                        xamarino.OlhoEsquerdoFechado = SVG.XAMARINO_OLHO_ESQUERDO_FECHADO;
                        break;
                    case XamarinoPosicao.Esquerda:
                        xamarino.OlhoDireitoAberto =   SVG.XAMARINO_ESQUERDA_OLHO_DIREITO_ABERTO;
                        xamarino.OlhoDireitoPiscando = SVG.XAMARINO_ESQUERDA_OLHO_DIREITO_PISCANDO;
                        xamarino.OlhoDireitoFechado =  SVG.XAMARINO_ESQUERDA_OLHO_DIREITO_FECHADO;

                        xamarino.OlhoEsquerdoAberto =   SVG.XAMARINO_ESQUERDA_OLHO_ESQUERDO_ABERTO;
                        xamarino.OlhoEsquerdoPiscando = SVG.XAMARINO_ESQUERDA_OLHO_ESQUERDO_PISCANDO;
                        xamarino.OlhoEsquerdoFechado =  SVG.XAMARINO_ESQUERDA_OLHO_ESQUERDO_FECHADO;
                        break;
                    case XamarinoPosicao.Direita:
                        break;
                    case XamarinoPosicao.Cima:
                        break;
                    case XamarinoPosicao.Baixo:
                        break;
                    default:
                        break;
                }
            }
        }

        public string OlhoDireitoAberto
        {
            get => (string)GetValue(OlhoDireitoAbertoProperty);
            set => SetValue(OlhoDireitoAbertoProperty, value);
        }

        public string OlhoDireitoPiscando
        {
            get => (string)GetValue(OlhoDireitoPiscandoProperty);
            set => SetValue(OlhoDireitoPiscandoProperty, value);
        }

        public string OlhoDireitoFechado
        {
            get => (string)GetValue(OlhoDireitoFechadoProperty);
            set => SetValue(OlhoDireitoFechadoProperty, value);
        }

        public string OlhoEsquerdoAberto
        {
            get => (string)GetValue(OlhoEsquerdoAbertoProperty);
            set => SetValue(OlhoEsquerdoAbertoProperty, value);
        }

        public string OlhoEsquerdoPiscando
        {
            get => (string)GetValue(OlhoEsquerdoPiscandoProperty);
            set => SetValue(OlhoEsquerdoPiscandoProperty, value);
        }

        public string OlhoEsquerdoFechado
        {
            get => (string)GetValue(OlhoEsquerdoFechadoProperty);
            set => SetValue(OlhoEsquerdoFechadoProperty, value);
        }

        public XamarinoPosicao Posicao 
        { 
            get => (XamarinoPosicao)GetValue(PosicaoProperty);
            set => SetValue(PosicaoProperty, value);
        }

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
                AtualizarOlhos(OlhoDireitoFechado, OlhoEsquerdoFechado);
            }
            else if (frame > .95) {
                AtualizarOlhos(OlhoDireitoPiscando, OlhoEsquerdoPiscando);
            }
            else {
                AtualizarOlhos(OlhoDireitoAberto, OlhoEsquerdoAberto);
            }
        }

        private void AtualizarOlhos(string olhoDireito, string olhoEsquerdo)
        {
            if (string.IsNullOrWhiteSpace(olhoEsquerdo) || string.IsNullOrWhiteSpace(olhoDireito))
                return;

            var geometria = (Geometry)_conversor.ConvertFromInvariantString(olhoDireito);
            olho_direito.Data = geometria;

            geometria = (Geometry)_conversor.ConvertFromInvariantString(olhoEsquerdo);
            olho_esquerdo.Data = geometria;
        }
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
