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

        protected override SKPath CriarPath(float largura, float altura, float padding) =>
            SKPath.ParseSvgPathData(Path);
    }
}
