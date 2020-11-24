using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Numerics;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Svg.Skia;
using Xamarin.Community.BR.Abstractions;
using Xamarin.Community.BR.Extensions;
using Xamarin.Community.BR.Helpers;
using Xamarin.Community.BR.Renderers.Mapa;
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
            e.Surface.Canvas.Clear();

            var engine = new SKEngine(e.Surface, e.Info);
            var contexto = new RenderContext(e.Surface.Canvas, null, e.Info);
            var svgRenderer = new SvgRenderer(Constantes.SVG.Mapa.CONTORNO, engine);

            svgRenderer.Renderizar(contexto);

            if (ItemSource?.Any() != true)
                return;

            var limites = svgRenderer.Limites;
            var xparam = (16f * 100) / 408;
            var yparam = (16f * 100) / 410;

            var pontoInicial = new PontoReferencia(
                new Vector2(limites.Esquerda, limites.Cima),
                new GeoLocalizacao(5.271799f, -73.982810f));

            var pontoFinal = new PontoReferencia(
                new Vector2(limites.Direita - xparam, limites.Baixo - yparam),
                new GeoLocalizacao(-33.750620f, -34.793140f));

            var pontoInicialGlobal = pontoInicial.Localizacao.ToGlobalXY(pontoInicial, pontoFinal);
            var pontoFinalGlobal = pontoFinal.Localizacao.ToGlobalXY(pontoInicial, pontoFinal);

            var pinRendererCollection = ItemSource.Select(
                p => p.ToRenderer(pontoInicial, pontoFinal, pontoInicialGlobal, pontoFinalGlobal, engine));

            foreach (var render in pinRendererCollection)
            {
                render.Renderizar(contexto);
            }
        }

        private void DesenharPins(SKCanvas canvas, int largura, int altura, SKRect rect)
        {
            if (ItemSource?.Any() != true)
                return;

            var xparam = (16f * 100) / 408;
            var yparam = (16f * 100) / 410;

            var pontoInicial = new PontoReferencia(
                new Vector2(rect.Left, rect.Top),
                new GeoLocalizacao(5.271799f, -73.982810f));

            var pontoFinal = new PontoReferencia(
                new Vector2(rect.Right - xparam, rect.Bottom - yparam),
                new GeoLocalizacao(-33.750620f, -34.793140f));

            var pontoInicialGlobal = pontoInicial.Localizacao.ToGlobalXY(pontoInicial, pontoFinal);
            var pontoFinalGlobal = pontoFinal.Localizacao.ToGlobalXY(pontoInicial, pontoFinal);

            foreach (var pin in ItemSource)
            {
                var geoLocalizacao = pin.GetGeolocalizacao();
                var pos = geoLocalizacao.ToScreenXY(pontoInicial,
                                                    pontoFinal,
                                                    pontoInicialGlobal,
                                                    pontoFinalGlobal);

                using (var paint = new SKPaint())
                {
                    paint.Color = SKColor.Parse("#313639");
                    paint.Style = SKPaintStyle.Fill;
                    paint.IsAntialias = true;

                    using (var path = new SKPath())
                    {
                        var metadeLargura = Constantes.Pin.LARGURA / 2;
                        var raioPosY = pos.Y - (Constantes.Pin.ALTURA * 0.6f);

                        path.MoveTo(pos.X, pos.Y);
                        path.ArcTo(Constantes.Pin.RAIO, new SKPoint(pos.X - metadeLargura, raioPosY));
                        path.ArcTo(metadeLargura, new SKPoint(pos.X + metadeLargura, raioPosY));
                        path.ArcTo(Constantes.Pin.RAIO, new SKPoint(pos.X, pos.Y));
                        path.Close();

                        canvas.DrawPath(path, paint);

                        paint.Color = SKColor.Parse("#FFFFFF");
                        paint.TextAlign = SKTextAlign.Center;
                        paint.TextSize = 35f;

                        canvas.DrawText("1", pos.X, raioPosY * 1.01f, paint);
                    }
                }
            }
        }

        private void ItemSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            canvas.InvalidateSurface();
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
