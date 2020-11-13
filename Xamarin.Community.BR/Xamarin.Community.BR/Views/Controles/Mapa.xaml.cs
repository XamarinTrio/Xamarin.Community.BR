using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
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

        public static readonly BindableProperty PosicaoXProperty = BindableProperty.Create(
            nameof(PosicaoX), 
            typeof(float),
            typeof(Mapa),
            -51.088751f,
            propertyChanged: OnItemSourceChanged);

        public static readonly BindableProperty PosicaoYProperty = BindableProperty.Create(
            nameof(PosicaoY),
            typeof(float),
            typeof(Mapa),
            -29.940163f,
            propertyChanged: OnItemSourceChanged);

        public float PosicaoY
        {
            get => (float)GetValue(PosicaoYProperty);
            set => SetValue(PosicaoYProperty, value);
        }

        public float PosicaoX
        {
            get => (float)GetValue(PosicaoXProperty);
            set => SetValue(PosicaoXProperty, value);
        }

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
            if (posicaoX != 0 || posicaoY != 0)
                ReposicionarCanvas();

            if (escala != 1)
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
            DesenharPins(canvas, largura, altura);
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

        private void DesenharPins(SKCanvas canvas, int largura, int altura)
        {
            if (ItemSource?.Any() != true)
                return;

            foreach (var pin in ItemSource)
            {
                //Regra de três
                //1080 => 600f
                //largura => larguraParam
                var larguraParam = (largura * 600f) / 1080f;

                //Regra de três
                //1705f => 1270f
                //altura => alturaParam
                var alturaParam = (altura * 1270f) / 1705f;

                //Regra de três
                //51.088751f => larguraParam
                //50.0000 => canvasposx
                var pinPosX = (larguraParam * Math.Abs(PosicaoX)) / 51.088751f;

                //Regra de três
                //29.940163f - alturaParam
                //23.00000 - canvasPosY
                var pinPosY = (Math.Abs(PosicaoY) * alturaParam) / 29.940163f;

                using (var paint = new SKPaint())
                {
                    paint.Color = SKColor.Parse("#313639");
                    paint.Style = SKPaintStyle.Fill;
                    paint.IsAntialias = true;

                    using (var path = new SKPath())
                    {
                        var metadeLargura = Constantes.Pin.LARGURA / 2;
                        var raioPosY = pinPosY - (Constantes.Pin.ALTURA * 0.6f);

                        path.MoveTo(pinPosX, pinPosY);
                        path.ArcTo(Constantes.Pin.RAIO, new SKPoint(pinPosX - metadeLargura, raioPosY));
                        path.ArcTo(metadeLargura, new SKPoint(pinPosX + metadeLargura, raioPosY));
                        path.ArcTo(Constantes.Pin.RAIO, new SKPoint(pinPosX, pinPosY));
                        path.Close();

                        canvas.DrawPath(path, paint);
                    }
                }
            }
        }

        private void ItemSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            canvas.InvalidateSurface();
        }

        private static void OnTamanhoPinChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is Mapa mapa)
                mapa.canvas.InvalidateSurface();
        }

        private static void OnItemSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is Mapa mapa)
            {
                if (oldValue is INotifyCollectionChanged oldCollection)
                {
                    oldCollection.CollectionChanged -= mapa.ItemSourceCollectionChanged;
                }

                if (newValue is INotifyCollectionChanged newCollection)
                {
                    newCollection.CollectionChanged += mapa.ItemSourceCollectionChanged;
                }

                mapa.canvas.InvalidateSurface();
            }
        }
    }
}
