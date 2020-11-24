using System;
using System.IO;
using SkiaSharp;
using Svg.Skia;
using Xamarin.Community.BR.Abstractions.Renderers;

namespace Xamarin.Community.BR.Renderers.SkiaSharp
{
    public sealed class SKSvgElement : ISvgElement
    {
        private readonly SKSvg _svg;
        private readonly SKPicture _skPicuture;

        public Retangulo Limites { get; }

        public SKSvgElement(Stream resourceStream)
        {
            _svg = new SKSvg();
            _skPicuture = _svg.Load(resourceStream);

            var bounds = _svg.Picture.CullRect;

            Limites = new Retangulo(bounds.Left, bounds.Top, bounds.Right, bounds.Bottom);
        }


        public SKPicture ToSkia() => _skPicuture;

        public void Dispose()
        {
            _svg.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
