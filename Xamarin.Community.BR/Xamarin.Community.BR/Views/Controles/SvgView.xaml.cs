using System;
using SkiaSharp;
using Xamarin.Community.BR.Renderers;
using Xamarin.Forms;

namespace Xamarin.Community.BR.Views.Controles
{
    public partial class SvgView : NeoView
    {
        public static readonly BindableProperty PathProperty = BindableProperty.Create(
            propertyName: nameof(Path),
            returnType: typeof(string),
            declaringType: typeof(SvgView),
            defaultValue: null,
            propertyChanged: OnVisualPropertyChanged);

        public string Path
        {
            get => (string)GetValue(PathProperty);
            set => SetValue(PathProperty, value);
        }

        public SvgView()
        {
            InitializeComponent();
        }

        protected override SKPath CriarPath(float largura, float altura, float padding)
        {
            if (string.IsNullOrEmpty(Path))
                return default(SKPath);

            var path = SKPath.ParseSvgPathData(Path);
            var bounds = path.Bounds;

            var xRatio = largura / bounds.Width;
            var yRatio = altura / bounds.Height;

            path.Transform(SKMatrix.CreateScaleTranslation(xRatio, yRatio, padding, padding));

            return path;
        }
    }
}
