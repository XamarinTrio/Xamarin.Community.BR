using System;
using SkiaSharp;
using Xamarin.Community.BR.Extensions;
using Xamarin.Community.BR.Renderers;
using Xamarin.Forms;

namespace Xamarin.Community.BR.Views.Controles
{
    public partial class NeoFrame : NeoView
    {
        private const int DEFAULT_CORNER_RADIUS = 3;

        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(
            propertyName: nameof(CornerRadius),
            returnType: typeof(CornerRadius),
            declaringType: typeof(NeoFrame),
            defaultValue: new CornerRadius(DEFAULT_CORNER_RADIUS),
            propertyChanged: OnVisualPropertyChanged);

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public NeoFrame()
        {
            InitializeComponent();
        }

        protected override SKPath CriarPath(float largura, float altura, float padding)
        {
            var path = new SKPath();
            var fTopLeftRadius = Convert.ToSingle(CornerRadius.TopLeft);
            var fTopRightRadius = Convert.ToSingle(CornerRadius.TopRight);
            var fBottomLeftRadius = Convert.ToSingle(CornerRadius.BottomLeft);
            var fBottomRightRadius = Convert.ToSingle(CornerRadius.BottomRight);

            var startX = fTopLeftRadius + padding;
            var startY = padding;

            path.MoveTo(startX, startY);

            path.LineTo(largura - fTopRightRadius + padding, startY);
            path.ArcTo(fTopRightRadius,
                new SKPoint(largura + padding, fTopRightRadius + padding));

            path.LineTo(largura + padding, altura - fBottomRightRadius + padding);
            path.ArcTo(fBottomRightRadius,
                 new SKPoint(largura - fBottomRightRadius + padding, altura + padding));

            path.LineTo(fBottomLeftRadius + padding, altura + padding);
            path.ArcTo(fBottomLeftRadius,
                new SKPoint(padding, altura - fBottomLeftRadius + padding));

            path.LineTo(padding, fTopLeftRadius + padding);
            path.ArcTo(fTopLeftRadius, new SKPoint(startX, startY));

            path.Close();

            return path;
        }

        protected override void DesenharControle(RenderContext context)
        {
            base.DesenharControle(context);
        }
    }
}
