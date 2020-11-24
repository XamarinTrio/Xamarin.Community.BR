using System;
using System.Numerics;
using Xamarin.Community.BR.Helpers;

namespace Xamarin.Community.BR.Extensions
{
    public static class GeoLocalizacaoExtensions
    {
        private const float EARTH_RADIUS = 6.371f;

        public static Vector2 ToScreenXY(this GeoLocalizacao localizacao,
                                         PontoReferencia pontoInicial, PontoReferencia pontoFinal,
                                         Vector2 pontoInicialGlobal, Vector2 pontoFinalGlobal)
        {
            //Calculate global X and Y for projection point
            var pos = ToGlobalXY(localizacao, pontoInicial, pontoFinal);

            //Calculate the percentage of Global X position in relation to total global width
            var perX = ((pos.X - pontoInicialGlobal.X)
                / (pontoFinalGlobal.X - pontoInicialGlobal.X));

            //Calculate the percentage of Global Y position in relation to total global height
            var perY = ((pos.Y - pontoInicialGlobal.Y)
                / (pontoFinalGlobal.Y - pontoInicialGlobal.Y));

            var x = pontoInicial.Referencia.X +
                (pontoFinal.Referencia.X - pontoInicial.Referencia.X) * perX;

            var y = pontoInicial.Referencia.Y +
                (pontoFinal.Referencia.Y - pontoInicial.Referencia.Y) * perY;

            return new Vector2(x, y);
        }

        public static Vector2 ToGlobalXY(this GeoLocalizacao geoLocalizacao,
                                         PontoReferencia pontoInicial, PontoReferencia pontoFinal)
        {
            //Calculates x based on cos of average of the latitudes
            var x = EARTH_RADIUS * geoLocalizacao.Longitude
                * Math.Cos((pontoInicial.Localizacao.Latitude + pontoFinal.Localizacao.Latitude) / 2);

            //Calculates y based on latitude
            var y = EARTH_RADIUS * geoLocalizacao.Latitude.ToSingle();

            return new Vector2(x.ToSingle(), y);
        }

    }
}
