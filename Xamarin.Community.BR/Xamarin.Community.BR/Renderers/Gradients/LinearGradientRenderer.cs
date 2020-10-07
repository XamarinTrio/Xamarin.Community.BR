﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Community.BR.Renderers.Gradients;

namespace Xamarin.Community.BR.Renderers
{
    public class LinearGradientRenderer
    {
        private readonly LinearGradient _gradient;

        public LinearGradientRenderer(LinearGradient gradient) =>
            _gradient = gradient;

        public SKShader BuildShader(RenderContext context)
        {
            var info = context.Info;

            var orderedStops = _gradient.Stops.OrderBy(x => x.RenderOffset).ToArray();
            var lastOffset = orderedStops.LastOrDefault()?.RenderOffset ?? 1;

            var colors = orderedStops.Select(x => x.Color.ToSKColor()).ToArray();
            var colorPos = orderedStops.Select(x => x.RenderOffset / lastOffset).ToArray();

            var (startPoint, endPoint) = GetGradientPoints(info.Width, info.Height, _gradient.Angle, lastOffset);

            return SKShader.CreateLinearGradient(
                startPoint,
                endPoint,
                colors,
                colorPos,
                _gradient.IsRepeating ? SKShaderTileMode.Repeat : SKShaderTileMode.Clamp);
        }

        private (SKPoint, SKPoint) GetGradientPoints(int width, int height, double rotation, float offset)
        {
            var angle = rotation / 360.0;

            var a = width * Math.Pow(Math.Sin(2 * Math.PI * ((angle + 0.75) / 2)), 2);
            var b = height * Math.Pow(Math.Sin(2 * Math.PI * ((angle + 0.0) / 2)), 2);
            var c = width * Math.Pow(Math.Sin(2 * Math.PI * ((angle + 0.25) / 2)), 2);
            var d = height * Math.Pow(Math.Sin(2 * Math.PI * ((angle + 0.5) / 2)), 2);

            var start = new SKPoint(
                (width - (float)a) * offset,
                (float)b * offset);

            var end = new SKPoint(
                (width - (float)c) * offset,
                (float)d * offset);

            return (start, end);
        }
    }
}
