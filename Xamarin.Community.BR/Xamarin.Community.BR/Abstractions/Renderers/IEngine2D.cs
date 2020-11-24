using System.IO;
using System.Numerics;
using Xamarin.Community.BR.Renderers;
using Xamarin.Community.BR.Renderers.Mapa;
using Xamarin.Forms;

namespace Xamarin.Community.BR.Abstractions.Renderers
{
    public interface IEngine2D
    {
        ISvgElement LoadSvg(Stream stream);

        void DesenharSvg(ISvgElement svgDoc, Transformacao matriz);

        void DesenharTexto(string v1, Vector2 vector2, float v2, Color corTexto);
        void DesenharCaminho(Caminho caminho, Color cor);
    }
}
