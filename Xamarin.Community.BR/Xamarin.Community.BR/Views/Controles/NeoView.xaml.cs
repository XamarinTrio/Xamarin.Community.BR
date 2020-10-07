using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Community.BR.Extensions;
using Xamarin.Community.BR.Renderers;
using Xamarin.Community.BR.Renderers.Gradients;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xamarin.Community.BR.Views.Controles
{
    [ContentProperty(nameof(InnerView))]
    public abstract partial class NeoView : ContentView
    {
        public static readonly BindableProperty InnerViewProperty = BindableProperty.Create(
            propertyName: nameof(InnerView),
            returnType: typeof(View),
            declaringType: typeof(NeoView),
            defaultValue: null,
            propertyChanged: OnInnerViewChanged);

        public static readonly BindableProperty ShadowBlurProperty = BindableProperty.Create(
            propertyName: nameof(ShadowBlur),
            returnType: typeof(double),
            declaringType: typeof(NeoView),
            defaultValue: 10.0,
            propertyChanged: OnVisualPropertyChanged);

        public static readonly BindableProperty ElevationProperty = BindableProperty.Create(
            propertyName: nameof(Elevation),
            returnType: typeof(double),
            declaringType: typeof(NeoView),
            defaultValue: .6,
            propertyChanged: OnVisualPropertyChanged);

        public static readonly BindableProperty ShadowDistanceProperty = BindableProperty.Create(
            propertyName: nameof(ShadowDistance),
            returnType: typeof(double),
            declaringType: typeof(NeoView),
            defaultValue: 9.0,
            propertyChanged: OnVisualPropertyChanged);

        public static readonly BindableProperty DarkShadowColorProperty = BindableProperty.Create(
            propertyName: nameof(DarkShadowColor),
            returnType: typeof(Color),
            declaringType: typeof(NeoView),
            defaultValue: Color.Black,
            propertyChanged: OnVisualPropertyChanged);

        public static readonly BindableProperty DrawOuterShadowProperty = BindableProperty.Create(
            propertyName: nameof(DrawOuterShadow),
            returnType: typeof(bool),
            declaringType: typeof(NeoView),
            defaultValue: true,
            propertyChanged: OnVisualPropertyChanged);

        public static readonly BindableProperty DrawInnerShadowProperty = BindableProperty.Create(
            propertyName: nameof(DrawInnerShadow),
            returnType: typeof(bool),
            declaringType: typeof(NeoView),
            defaultValue: false,
            propertyChanged: OnVisualPropertyChanged);

        public static readonly new BindableProperty BackgroundColorProperty = BindableProperty.Create(
           propertyName: nameof(ContentView.BackgroundColor),
           returnType: typeof(Color),
           declaringType: typeof(NeoView),
           defaultValue: Color.Transparent,
           propertyChanged: OnVisualPropertyChanged);

        public static readonly BindableProperty BackgroundGradientProperty = BindableProperty.Create(
            propertyName: nameof(BackgroundGradient),
            returnType: typeof(Gradient),
            declaringType: typeof(NeoView),
            defaultValue: null,
            propertyChanged: OnVisualPropertyChanged);

        public View InnerView
        {
            get => (View)GetValue(InnerViewProperty);
            set => SetValue(InnerViewProperty, value);
        }

        public bool DrawOuterShadow
        {
            get => (bool)GetValue(DrawOuterShadowProperty);
            set => SetValue(DrawOuterShadowProperty, value);
        }

        public bool DrawInnerShadow
        {
            get => (bool)GetValue(DrawInnerShadowProperty);
            set => SetValue(DrawInnerShadowProperty, value);
        }

        public new Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }

        public Gradient BackgroundGradient
        {
            get => (Gradient)GetValue(BackgroundGradientProperty);
            set => SetValue(BackgroundGradientProperty, value);
        }

        public double Elevation
        {
            get => (double)GetValue(ElevationProperty);
            set => SetValue(ElevationProperty, value);
        }

        public double ShadowDistance
        {
            get => (double)GetValue(ShadowDistanceProperty);
            set => SetValue(ShadowDistanceProperty, value);
        }

        public double ShadowBlur
        {
            get => (double)GetValue(ShadowBlurProperty);
            set => SetValue(ShadowBlurProperty, value);
        }

        public Color DarkShadowColor
        {
            get => (Color)GetValue(DarkShadowColorProperty);
            set => SetValue(DarkShadowColorProperty, value);
        }

        public NeoView()
        {
            InitializeComponent();
        }

        private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var surface = e.Surface;
            var canvas = surface.Canvas;

            canvas.Clear();

            using (var paint = new SKPaint())
            {
                var context = new RenderContext(canvas, paint, e.Info);

                if (DrawOuterShadow)
                {
                    SetPaintDefaults(context);
                    DesenharSombraExterna(context);
                }

                SetPaintDefaults(context);
                DesenharControle(context);

                if (DrawInnerShadow)
                {
                    SetPaintDefaults(context);
                    DesenharSombraInterna(context);
                }
            }
        }

        protected void SetPaintDefaults(RenderContext context)
        {
            context.Paint.IsAntialias = true;
            context.Paint.Style = SKPaintStyle.Fill;
            SetPaintColor(context);
        }

        protected virtual void SetPaintColor(RenderContext context)
        {
            if (BackgroundGradient != null)
                context.Paint.Shader = BackgroundGradient.BuildShader(context);
            else
                context.Paint.Color = BackgroundColor.ToSKColor();
        }

        protected virtual void DesenharSombraInterna(RenderContext context)
        {
            var fShadowDistance = Convert.ToSingle(ShadowDistance);
            var darkShadow = Color.FromRgba(DarkShadowColor.R, DarkShadowColor.G, DarkShadowColor.B, Elevation);
            var padding = Convert.ToSingle(ShadowBlur * 2);

            context.Paint.MaskFilter = SKMaskFilter.CreateBlur(SKBlurStyle.Normal, Convert.ToSingle(ShadowBlur));

            var diameter = padding * 2;
            var largura = context.Info.Width - diameter;
            var altura = context.Info.Height - diameter;
            using (var path = CriarPath(largura, altura, padding))
            {
                context.Canvas.ClipPath(path, antialias: true);
                context.Paint.ImageFilter = darkShadow.ToSKDropShadow(-fShadowDistance);
                context.Canvas.DrawPath(path, context.Paint);
            }
        }

        protected virtual void DesenharControle(RenderContext context)
        {
            var padding = Convert.ToSingle(ShadowBlur * 2);

            var diameter = padding * 2;
            var largura = context.Info.Width - diameter;
            var altura = context.Info.Height - diameter;

            using (var path = CriarPath(largura, altura, padding))
            {
                context.Paint.MaskFilter = null;
                context.Canvas.DrawPath(path, context.Paint);
            }
        }

        protected virtual void DesenharSombraExterna(RenderContext context)
        {
            var fShadowDistance = Convert.ToSingle(ShadowDistance);
            var darkShadow = Color.FromRgba(DarkShadowColor.R, DarkShadowColor.G, DarkShadowColor.B, Elevation);
            var padding = Convert.ToSingle(ShadowBlur * 2);

            context.Paint.MaskFilter = SKMaskFilter.CreateBlur(SKBlurStyle.Normal, Convert.ToSingle(ShadowBlur));

            var diameter = padding * 2;
            var largura = context.Info.Width - diameter;
            var altura = context.Info.Height - diameter;
            using (var path = CriarPath(largura, altura, padding))
            {
                context.Paint.ImageFilter = darkShadow.ToSKDropShadow(fShadowDistance);
                context.Canvas.DrawPath(path, context.Paint);
            }
        }

        private static void OnInnerViewChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is NeoView neoView)
            {
                if (newValue is View child)
                    neoView.rootView.Children.Add(child, 0, 0);
            }
        }

        protected abstract SKPath CriarPath(float largura, float altura, float padding);

        protected static void OnVisualPropertyChanged(BindableObject bindable, object oldValue, object newValue) =>
            ((NeoView)bindable).canvas.InvalidateSurface();
    }
}
