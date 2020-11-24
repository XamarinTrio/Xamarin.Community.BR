using System;
using Xamarin.Community.BR.Renderers;

namespace Xamarin.Community.BR.Abstractions.Renderers
{
    public interface ISvgElement : IDisposable
    {
        Retangulo Limites { get; }
    }
}
