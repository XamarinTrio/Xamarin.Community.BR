using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Svg.Skia;
using Xamarin.Community.BR.Abstractions;
using Xamarin.Community.BR.Extensions;
using Xamarin.Community.BR.Helpers;
using Xamarin.Forms;

namespace Xamarin.Community.BR.Views.Controles
{
    public partial class Mapa : ContentView
    {
        public static readonly BindableProperty ItemSourceProperty =
            BindableProperty.Create(
                propertyName: nameof(ItemSource),
                returnType: typeof(IEnumerable<IPin>),
                declaringType: typeof(Mapa),
                propertyChanged: OnItemSourceChanged);

        private float posicaoX = 0;
        private float posicaoY = 0;

        private float posicaoAnteriorX = 0;
        private float posicaoAnteriorY = 0;

        private double escala = 1;

        public IEnumerable<IPin> ItemSource 
        {
            get => (IEnumerable<IPin>)GetValue(ItemSourceProperty);
            set => SetValue(ItemSourceProperty, value);
        }

        public Mapa()
        {
            InitializeComponent();

            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += OnPanUpdated;

            var pinchGesture = new PinchGestureRecognizer();
            pinchGesture.PinchUpdated += OnPinchUpdated;

            var tapGesture = new TapGestureRecognizer();
            tapGesture.NumberOfTapsRequired = 2;
            tapGesture.Tapped += OnTapped;

            GestureRecognizers.Add(panGesture);
            GestureRecognizers.Add(pinchGesture);
            GestureRecognizers.Add(tapGesture);
        }

        public void ResetarMapa()
        {
            if(posicaoX != 0 || posicaoY != 0)
                ReposicionarCanvas();

            if(escala != 1)
                EscalarCanvas();
        }

        private void OnTapped(object sender, EventArgs e) => ResetarMapa();

        private void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            switch (e.Status)
            {
                case GestureStatus.Started:
                    break;
                case GestureStatus.Running:
                    var novaEscala = escala * e.Scale;
                    EscalarCanvas(novaEscala);
                    break;
                case GestureStatus.Completed:
                    break;
                case GestureStatus.Canceled:
                    break;
                default:
                    break;
            }
        }

        private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    break;
                case GestureStatus.Running:
                    var panX = e.TotalX.ToSingle();
                    var panY = e.TotalY.ToSingle();

                    var novaPosX = posicaoX + (panX - posicaoAnteriorX);
                    var novaPosY = posicaoY + (panY - posicaoAnteriorY);

                    ReposicionarCanvas(novaPosX, novaPosY);

                    posicaoAnteriorX = panX;
                    posicaoAnteriorY = panY;
                    break;
                case GestureStatus.Completed:
                    posicaoAnteriorX = 0;
                    posicaoAnteriorY = 0;
                    break;
                case GestureStatus.Canceled:
                    break;
                default:
                    break;
            }
        }

        private void ReposicionarCanvas(float x = 0, float y = 0)
        {
            posicaoX = x;
            posicaoY = y;

            var largura = canvas.Width.ToSingle();
            var altura = canvas.Height.ToSingle();

            var fEscala = escala.ToSingle();
            var limiteHorizontal = largura - ((largura / fEscala) / 5);
            var limiteVertical = altura - ((altura / fEscala) / 3);

            posicaoX = Math.Max(Math.Min(posicaoX, limiteHorizontal), -limiteHorizontal);
            posicaoY = Math.Max(Math.Min(posicaoY, limiteVertical), -limiteVertical);

            canvas.TranslateTo(posicaoX, posicaoY, 0).TentarExecutar();
        }

        private void EscalarCanvas(double novaEscala = 1)
        {
            escala = Math.Min(Math.Max(1, novaEscala), 3.5d);
            canvas.ScaleTo(escala, 0).TentarExecutar();
        }

        private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            var largura = e.Info.Width;
            var altura = e.Info.Height;

            canvas.Clear();
            DesenharSvg(canvas, largura, altura, Constantes.SVG.Mapa.CONTORNO);
            DesenharPins();
        }

        private static void DesenharSvg(SKCanvas canvas, int largura, int altura, string nome)
        {
            using (var svg = new SKSvg())
            {
                using (var stream = ResourcesHelper.CarregarRecurso(nome))
                {
                    var svgDoc = svg.Load(stream);
                    var bounds = svg.Picture.CullRect;

                    var xRatio = largura / bounds.Width;
                    var yRatio = altura / bounds.Height;
                    var escala = Math.Min(xRatio, yRatio);

                    var posicaoX = (largura - (bounds.Width * escala)) / 2;
                    var posicaoY = (altura - (bounds.Height * escala)) / 2;
                    var matrix = SKMatrix.CreateScaleTranslation(escala, escala, posicaoX, posicaoY);

                    using (var paint = new SKPaint())
                    {
                        paint.IsAntialias = true;
                        paint.ImageFilter = Color.FromHex("#66000000").ToSKDropShadow(6, 12f);
                        paint.MaskFilter = SKMaskFilter.CreateBlur(SKBlurStyle.Normal, 16f);

                        canvas.DrawPicture(svgDoc, ref matrix, paint);
                    }
                }
            }
        }

        private void DesenharPins()
        {
            if (ItemSource?.Any() != true)
                return;

            foreach(var pin in ItemSource)
            {
                var posicaoX = pin.GetLatitude();
                var posicaoY = pin.GetLongitude();
            }
        }

        private void ItemSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            canvas.InvalidateSurface();
        }

        private static void OnItemSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if(bindable is Mapa mapa)
            {
                if(oldValue is INotifyCollectionChanged oldCollection)
                {
                    oldCollection.CollectionChanged -= mapa.ItemSourceCollectionChanged;
                }

                if(newValue is INotifyCollectionChanged newCollection)
                {
                    newCollection.CollectionChanged += mapa.ItemSourceCollectionChanged;
                }

                mapa.canvas.InvalidateSurface();
            }
        }
    }
}
