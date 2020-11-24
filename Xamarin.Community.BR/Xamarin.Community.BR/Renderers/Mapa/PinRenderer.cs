using System.Collections.Generic;
using System.Numerics;
using Xamarin.Community.BR.Abstractions;
using Xamarin.Community.BR.Abstractions.Renderers;
using Xamarin.Forms;

namespace Xamarin.Community.BR.Renderers.Mapa
{
    public class PinRenderer : IRenderer
    {
        private readonly Vector2 _posicao;

        private readonly IEngine2D _engine;

        public Color Cor { get; set; } = Color.Black;

        public Color CorTexto { get; set; } = Color.White;

        public PinRenderer(Vector2 posicao, IEngine2D engine)
        {
            _posicao = posicao;
            _engine = engine;
        }

        public void Renderizar(RenderContext contexto)
        {
            var raioPosY = _posicao.Y - (Constantes.Pin.ALTURA * 0.6f);
            var caminho = new Caminho(PegarAcoes(raioPosY));

            _engine.DesenharCaminho(caminho, Cor);
            _engine.DesenharTexto("1", new Vector2(_posicao.X, raioPosY * 1.01f), 35f, CorTexto);
        }

        private IEnumerable<Acao> PegarAcoes(float raioPosY)
        {
            var metadeLargura = Constantes.Pin.LARGURA / 2;

            yield return new Mover(_posicao);
            yield return new Arco(new Vector2(_posicao.X - metadeLargura, raioPosY), Constantes.Pin.RAIO);
            yield return new Arco(new Vector2(_posicao.X + metadeLargura, raioPosY), metadeLargura);
            yield return new Arco(_posicao, Constantes.Pin.RAIO);
            yield return new Fechar();
        }
    }

    public struct Caminho
    {
        public IEnumerable<Acao> Acoes { get; set; }

        public Caminho(IEnumerable<Acao> acoes)
        {
            Acoes = acoes;
        }
    }

    public abstract class Acao
    {
    }

    public class Fechar : Acao
    {
    }

    public class Mover : Acao
    {
        public Vector2 Posicao { get; set; }

        public Mover(Vector2 posicao)
        {
            Posicao = posicao;
        }
    }

    public class Arco : Acao
    {
        public int Raio { get; set; }
        public Vector2 Posicao { get; set; }

        public Arco(Vector2 posicao, int raio)
        {
            Posicao = posicao;
            Raio = raio;
        }
    }
}
