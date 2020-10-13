using System;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Svg.Skia;
using Xamarin.Community.BR.Extensions;
using Xamarin.Community.BR.Helpers;
using Xamarin.Forms;

namespace Xamarin.Community.BR.Views.Controles
{
    public partial class Mapa : ContentView
    {
        public Mapa()
        {
            InitializeComponent();
        }

        private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            var largura = e.Info.Width;
            var altura = e.Info.Height;

            DesenharSvg(canvas, largura, altura, Constantes.SVG.Mapa.CONTORNO);
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
    }
}
