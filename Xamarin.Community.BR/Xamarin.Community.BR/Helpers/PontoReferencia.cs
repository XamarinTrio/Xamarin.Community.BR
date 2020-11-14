using System.Numerics;

namespace Xamarin.Community.BR.Helpers
{
    public struct PontoReferencia
    {
        public Vector2 Referencia { get; set; }

        public GeoLocalizacao Localizacao { get; set; }

        public PontoReferencia(Vector2 referencia, GeoLocalizacao localizacao)
        {
            Referencia = referencia;
            Localizacao = localizacao;
        }
    }
}
