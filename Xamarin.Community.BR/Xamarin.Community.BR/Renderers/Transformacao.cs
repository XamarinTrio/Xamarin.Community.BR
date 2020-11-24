using System.Numerics;

namespace Xamarin.Community.BR.Renderers
{
    public struct Transformacao
    {
        public Vector2? Escala { get; set; }
        public Vector2? Posicao { get; set; }

        public Transformacao(Vector2? escala = null, Vector2? posicao = null)
        {
            Escala = escala;
            Posicao = posicao;
        }
    }
}
