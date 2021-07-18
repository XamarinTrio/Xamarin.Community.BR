using System;
using Xamarin.Community.BR.Helpers;
using Xamarin.Forms;

namespace Xamarin.Community.BR.Views.Controles
{
    public partial class ZoomSlider : ContentView
    {
        public static readonly BindableProperty ValorProperty =
            BindableProperty.Create(
                propertyName: nameof(Valor),
                returnType: typeof(double),
                declaringType: typeof(ZoomSlider),
                defaultValue: 1d,
                propertyChanged: OnEscalaChanged);

        public static readonly BindableProperty PassoProperty =
            BindableProperty.Create(
                propertyName: nameof(Passo),
                returnType: typeof(double),
                declaringType: typeof(ZoomSlider),
                defaultValue: 1d);

        public static readonly BindableProperty MaximoProperty =
            BindableProperty.Create(
                propertyName: nameof(Maximo),
                returnType: typeof(double),
                declaringType: typeof(ZoomSlider),
                defaultValue: double.MaxValue);

        public static readonly BindableProperty MinimoProperty =
            BindableProperty.Create(
                propertyName: nameof(Minimo),
                returnType: typeof(double),
                declaringType: typeof(ZoomSlider),
                defaultValue: double.MinValue);

        private bool panHabilitado;

        private bool internalSet;

        private (double minimo, double maximo) limiteMarcador;

        public double Passo
        {
            get => (double)GetValue(PassoProperty);
            set => SetValue(PassoProperty, value);
        }

        public double Valor
        {
            get => (double)GetValue(ValorProperty);
            set => SetValue(ValorProperty, value);
        }

        public double Maximo
        {
            get => (double)GetValue(MaximoProperty);
            set => SetValue(MaximoProperty, value);
        }

        public double Minimo
        {
            get => (double)GetValue(MinimoProperty);
            set => SetValue(MinimoProperty, value);
        }

        public ZoomSlider() =>
            InitializeComponent();

        private void TouchEffect_TouchAction(object sender, TouchActionEventArgs args)
        {
            if (args is null)
                return;

            var retangulo = new Forms.Rectangle(
                new Point(Marcador.X, (Marcador.Y + Marcador.TranslationY)),
                Marcador.Bounds.Size);

            switch (args.Type)
            {
                case TouchActionType.Entered:
                    break;
                case TouchActionType.Pressed:
                    panHabilitado = retangulo.Contains(args.Location);
                    break;
                case TouchActionType.Moved:
                    if (!panHabilitado)
                        return;

                    if (args.Location.IsEmpty)
                        return;

                    var posicaoY = -(BarraSlide.Height - args.Location.Y);
                    var limites = PegarLimites();

                    posicaoY = Math.Max(Math.Min(posicaoY, limites.maximo), limites.minimo);
                    Marcador.TranslationY = posicaoY;
                    AlterarPosicaoMarcador();
                    break;
                case TouchActionType.Exited:
                    break;
                case TouchActionType.Released:
                case TouchActionType.Cancelled:
                default:
                    panHabilitado = false;
                    break;
            }
        }

        private void AlterarPosicaoMarcador()
        {
            try
            {
                internalSet = true;

                var limites = PegarLimites();
                var novaPosicaoMarcador = Math.Abs(Marcador.TranslationY);
                var valorEmPorcentagem = ((novaPosicaoMarcador) * 100)
                    / (Math.Abs(limites.minimo));

                var novaEscala = (valorEmPorcentagem * (Maximo - Minimo)) / 100;
                Valor = novaEscala + Minimo;
            }
            finally
            {
                internalSet = false;
            }
        }

        private void ReprocessarPosicaoMarcador()
        {
            if (internalSet)
                return;

            var limites = PegarLimites();
            var novaEscala = Valor - Minimo;
            var valorEmPorcentagem = novaEscala * 100 / (Maximo - Minimo);
            var novaPosicaoMarcador = (valorEmPorcentagem * Math.Abs(limites.minimo)) / 100;

            Marcador.TranslationY = -novaPosicaoMarcador;
        }

        private (double minimo, double maximo) PegarLimites()
        {
            var altura = BarraSlide.Height - BarraSlide.Margin.Top - BarraSlide.Margin.Bottom;
            var limite = (-altura, (altura - Marcador.Height / 2));
            if (limiteMarcador != limite)
                limiteMarcador = limite;

            return limiteMarcador;
        }

        private void BotaoZoomTapped(object sender, EventArgs e)
        {
            if(sender == BotaoIncremeto)
            {
                Valor = Math.Min(Math.Max((Valor + Passo), Minimo), Maximo);
            }
            else if (sender == BotaoDecremento)
            {
                Valor = Math.Min(Math.Max((Valor - Passo), Minimo), Maximo);
            }
        }

        private static void OnEscalaChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ZoomSlider slider)
                slider.ReprocessarPosicaoMarcador();
        }
    }
}
