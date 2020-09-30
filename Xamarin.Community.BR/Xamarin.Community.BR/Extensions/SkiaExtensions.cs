using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;

namespace Xamarin.Community.BR.Extensions
{
    internal static class SkiaExtensions
    {
        private const int DEFAULT_XAXISROTATE = 0;

        internal static SKPath ArcTo(this SKPath path, float radius, SKPoint finalPoint)
        {
            path.ArcTo(
                new SKPoint(radius, radius),
                DEFAULT_XAXISROTATE,
                SKPathArcSize.Small, SKPathDirection.Clockwise,
                finalPoint);

            return path;
        }
    }
}
