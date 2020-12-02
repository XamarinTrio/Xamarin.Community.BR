
using System;
using Xamarin.Community.BR.Extensions;
using Xamarin.Community.BR.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace Xamarin.Community.BR.Views.Controles
{
    public partial class ZoomSlider : ContentView
    {
        public static readonly BindableProperty ValorProperty =
            BindableProperty.Create(
                propertyName: nameof(Valor),
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

        private double posicaoMarcador = 0;
        private double posicaoAnteriorY = 0;
        private bool panHabilitado;

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

        public ZoomSlider()
        {
            InitializeComponent();
        }

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

                    var altura = BarraSlide.Height - BarraSlide.Margin.Top - BarraSlide.Margin.Bottom;
                    var posicaoY = -(BarraSlide.Height - args.Location.Y);
                    var limite = altura;

                    posicaoY = Math.Max(Math.Min(posicaoY, (limite - Marcador.Height / 2)), -limite);
                    Marcador.TranslationY = posicaoY;
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
    }
}
