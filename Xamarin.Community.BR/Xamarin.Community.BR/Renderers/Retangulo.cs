using System;
using System.Numerics;

namespace Xamarin.Community.BR.Renderers
{
    public struct Retangulo
    {
        public Transformacao? Transformacao { get; set; }
        public float Esquerda { get; set; }
        public float Cima { get; set; }
        public float Direita { get; set; }
        public float Baixo { get; set; }

        public Retangulo(float esquerda, float cima, float direita, float baixo, Transformacao? transformacao = null)
        {
            Transformacao = transformacao;
            Esquerda = esquerda;
            Direita = direita;
            Baixo = baixo;
            Cima = cima;

            AplicarTransformacao();
        }

        private void AplicarTransformacao()
        {
            if (!Transformacao.HasValue)
                return;

            var posicao = Transformacao?.Posicao;
            if (posicao.HasValue)
            {
                Esquerda += posicao.Value.X;
                Direita += posicao.Value.X;
                Cima += posicao.Value.Y;
                Baixo += posicao.Value.Y;
            }

            var escala = Transformacao?.Escala;
            if (escala.HasValue)
            {
                Direita *= escala.Value.X;
                Baixo *= escala.Value.Y;
            }
        }

        public float Largura() => Math.Abs(Direita - Esquerda);

        public float Altura() => Math.Abs(Baixo - Cima);
    }
}
