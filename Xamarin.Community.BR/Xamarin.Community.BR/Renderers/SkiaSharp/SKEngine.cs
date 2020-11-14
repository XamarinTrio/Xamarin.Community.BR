using System.IO;
using System.Numerics;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Community.BR.Abstractions.Renderers;
using Xamarin.Community.BR.Extensions;
using Xamarin.Community.BR.Renderers.Mapa;
using Xamarin.Community.BR.Renderers.SkiaSharp;
using Xamarin.Forms;

namespace Xamarin.Community.BR.Renderers
{
    public sealed class SKEngine : IEngine2D
    {
        private readonly SKCanvas _canvas;
        private readonly SKSurface _surface;
        private readonly SKImageInfo _imageInfo;

        public SKEngine(SKSurface surface, SKImageInfo imageInfo)
        {
            _surface = surface;
            _imageInfo = imageInfo;
            _canvas = surface.Canvas;
        }

        public void DesenharSvg(ISvgElement svgDoc, Transformacao matriz)
        {
            using (var paint = new SKPaint())
            {
                var escala = matriz.Escala.Value;
                var posicao = matriz.Posicao.Value;
                var matrix = SKMatrix.CreateScaleTranslation(escala.X, escala.Y, posicao.X, posicao.Y);

                paint.IsAntialias = true;
                paint.ImageFilter = Color.FromHex("#66000000").ToSKDropShadow(6, 12f);
                paint.MaskFilter = SKMaskFilter.CreateBlur(SKBlurStyle.Normal, 16f);

                if (svgDoc is SKSvgElement svgElement)
                    _canvas.DrawPicture(svgElement.ToSkia(), ref matrix, paint);
            }
        }

        public void DesenharTexto(string texto, Vector2 centro, float tamanhoFonte, Color corTexto)
        {
            using (var paint = new SKPaint())
            {
                paint.TextSize = tamanhoFonte;
                paint.Color = corTexto.ToSKColor();
                paint.TextAlign = SKTextAlign.Center;

                _canvas.DrawText(texto, centro.X, centro.Y, paint);
            }
        }

        public void DesenharCaminho(Caminho caminho, Color cor)
        {
            using (var paint = new SKPaint())
            {
                paint.Color = cor.ToSKColor();
                paint.Style = SKPaintStyle.Fill;
                paint.IsAntialias = true;

                using (var path = new SKPath())
                {
                    foreach (var acao in caminho.Acoes)
                    {
                        if (acao is Mover mover)
                        {
                            path.MoveTo(mover.Posicao.X, mover.Posicao.Y);
                        }
                        else if (acao is Arco arco)
                        {
                            path.ArcTo(arco.Raio, new SKPoint(arco.Posicao.X, arco.Posicao.Y));
                        }
                        else if (acao is Fechar)
                        {
                            path.Close();
                        }
                    }

                    _canvas.DrawPath(path, paint);

                }
            }
        }

        public ISvgElement LoadSvg(Stream stream) => new SKSvgElement(stream);
    }
}
