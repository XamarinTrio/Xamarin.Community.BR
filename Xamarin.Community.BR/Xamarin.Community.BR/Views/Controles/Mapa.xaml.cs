using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Svg.Skia;
using Xamarin.Community.BR.Abstractions;
using Xamarin.Community.BR.Extensions;
using Xamarin.Community.BR.Helpers;
using Xamarin.Community.BR.Renderers;
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

        public static readonly BindableProperty EscalaProperty =
            BindableProperty.Create(
                propertyName: nameof(Escala),
                returnType: typeof(double),
                declaringType: typeof(Mapa));

        private static Vector2? _pontoInicialGlobal;
        private static Vector2? _pontoFinalGlobal;

        private static PontoReferencia? _pontoInicial;
        private static PontoReferencia? _pontoFinal;

        private float posicaoX = 0;
        private float posicaoY = 0;

        private float posicaoAnteriorX = 0;
        private float posicaoAnteriorY = 0;

        private double escala = 1;

        public double Escala
        {
            get => (double)GetValue(EscalaProperty);
            set => SetValue(EscalaProperty, value);
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

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            switch (propertyName)
            {
                case nameof(Escala):
                    EscalarCanvas(Escala);
                    break;
                default:
                    break;
            }
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
            Escala = Math.Min(Math.Max(1, novaEscala), 3.5d);
            canvas.ScaleTo(Escala, 0).TentarExecutar();
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
            var pontoInicial = PegarPontoInicial(limites);
            var pontoFinal = PegarPontoFinal(limites);
            var pontoInicialGlobal = PegarPontoInicialGlobal(pontoInicial, pontoFinal);
            var pontoFinalGlobal = PegarPontoFinalGlobal(pontoInicial, pontoFinal);

            var pinRendererCollection = ItemSource
                .Select(p => p.ToRenderer(pontoInicial, pontoFinal, pontoInicialGlobal, pontoFinalGlobal, engine));

            foreach (var render in pinRendererCollection)
            {
                render.Renderizar(contexto);
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

        private static PontoReferencia PegarPontoInicial(Retangulo limites)
        {
            if (!_pontoInicial.HasValue)
            {
                _pontoInicial = new PontoReferencia(
                    new Vector2(limites.Esquerda, limites.Cima),
                    new GeoLocalizacao(
                        Constantes.GeoLocalizacao.Brasil.LATITUDE_EXTREMO_NORTE,
                        Constantes.GeoLocalizacao.Brasil.LONGITUDE_EXTREMO_OESTE));
            }

            return _pontoInicial.Value;
        }

        private static PontoReferencia PegarPontoFinal(Retangulo limites)
        {
            if (!_pontoFinal.HasValue)
            {
                var sombraRelativo = Constantes.SVG.Mapa.DESFOQUE_SOMBRA * 100;
                var xparam = sombraRelativo / Constantes.SVG.Mapa.LARGURA;
                var yparam = sombraRelativo / Constantes.SVG.Mapa.ALTURA;

                _pontoFinal = new PontoReferencia(
                    new Vector2(limites.Direita - xparam, limites.Baixo - yparam),
                    new GeoLocalizacao(
                        Constantes.GeoLocalizacao.Brasil.LATITUDE_EXTREMO_SUL,
                        Constantes.GeoLocalizacao.Brasil.LONGITUDE_EXTREMO_LESTE));
            }

            return _pontoFinal.Value;
        }

        private static Vector2 PegarPontoInicialGlobal(PontoReferencia pontoInicial, PontoReferencia pontoFinal)
        {
            if (!_pontoInicialGlobal.HasValue)
                _pontoInicialGlobal = pontoInicial.Localizacao.ToGlobalXY(pontoInicial, pontoFinal);

            return _pontoInicialGlobal.Value;
        }

        private static Vector2 PegarPontoFinalGlobal(PontoReferencia pontoInicial, PontoReferencia pontoFinal)
        {
            if (!_pontoFinalGlobal.HasValue)
                _pontoFinalGlobal = pontoFinal.Localizacao.ToGlobalXY(pontoInicial, pontoFinal);

            return _pontoFinalGlobal.Value;
        }
    }
}
