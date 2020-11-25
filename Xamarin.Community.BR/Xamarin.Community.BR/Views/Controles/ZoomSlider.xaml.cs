
using System;
using Xamarin.Community.BR.Extensions;
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

        private void MarcadorPanUpdated(object sender, PanUpdatedEventArgs e)
        {

            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    break;
                case GestureStatus.Running:
                    var posicaoY = posicaoMarcador + (e.TotalY - posicaoAnteriorY);
                    if (posicaoY == 0)
                        return;

                    var limite = BarraSlide.Height / 2;
                    posicaoY = Math.Max(Math.Min(posicaoY, limite), -limite);
                    Marcador.TranslateTo(0, posicaoY, 0).TentarExecutar();

                    posicaoAnteriorY = e.TotalY;
                    posicaoMarcador = posicaoY;
                    break;
                case GestureStatus.Completed:
                case GestureStatus.Canceled:
                default:
                    posicaoAnteriorY = 0;
                    break;
            }
        }
    }
}
