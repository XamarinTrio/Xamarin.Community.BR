using SkiaSharp;

namespace Xamarin.Community.BR.Renderers
{
    public struct RenderContext
    {
        public SKCanvas Canvas { get; }

        public SKPaint Paint { get; }

        public SKImageInfo Info { get; }

        public RenderContext(SKCanvas canvas, SKPaint paint, SKImageInfo info)
        {
            Canvas = canvas;
            Paint = paint;
            Info = info;
        }
    }
}
