using System;
using System.Numerics;
using Xamarin.Community.BR.Abstractions;
using Xamarin.Community.BR.Abstractions.Renderers;
using Xamarin.Community.BR.Helpers;

namespace Xamarin.Community.BR.Renderers.Mapa
{
    public class SvgRenderer : IRenderer
    {
        private readonly IEngine2D _engine;
        private readonly string _nomeSvg;

        public Retangulo Limites { get; private set; }

        public SvgRenderer(string nomeSvg, IEngine2D engine)
        {
            _engine = engine;
            _nomeSvg = nomeSvg;
        }

        public void Renderizar(RenderContext contexto)
        {
            var largura = contexto.Info.Width;
            var altura = contexto.Info.Height;

            using (var stream = ResourcesHelper.CarregarRecurso(_nomeSvg))
            {
                using (var svgDoc = _engine.LoadSvg(stream))
                {
                    var limitesSvg = svgDoc.Limites;

                    var xRatio = largura / limitesSvg.Largura();
                    var yRatio = altura / limitesSvg.Altura();
                    var escala = Math.Min(xRatio, yRatio);

                    var posicaoX = (largura - (limitesSvg.Largura() * escala)) / 2;
                    var posicaoY = (altura - (limitesSvg.Altura() * escala)) / 2;

                    var matriz = new Transformacao(
                        new Vector2(escala, escala),
                        new Vector2(posicaoX, posicaoY));

                    _engine.DesenharSvg(svgDoc, matriz);

                    Limites = new Retangulo(
                        limitesSvg.Esquerda,
                        limitesSvg.Cima + 3.5f,
                        limitesSvg.Direita - 12,
                        limitesSvg.Baixo - 7,
                        matriz);
                }
            }
        }
    }
}
