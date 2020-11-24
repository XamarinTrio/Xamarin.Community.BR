using System;
using System.Numerics;
using Xamarin.Community.BR.Abstractions;
using Xamarin.Community.BR.Abstractions.Renderers;
using Xamarin.Community.BR.Helpers;
using Xamarin.Community.BR.Renderers.Mapa;
using Xamarin.Forms;

namespace Xamarin.Community.BR.Extensions
{
    public static class PinExtensions
    {
        public static Pin ToPin(this IAmACommunityMember membro)
        {
            var avatar = PegarAvatar(membro);
            var localizacao = membro.Localizacao;

            return new Pin(localizacao, avatar);
        }

        public static IRenderer ToRenderer(this IPin pin, 
                                           PontoReferencia pontoInicial,
                                           PontoReferencia pontoFinal,
                                           Vector2 pontoInicialGlobal,
                                           Vector2 pontoFinalGlobal,
                                           IEngine2D engine)

        {
            var geoPos = pin.GetGeolocalizacao();
            var pos = geoPos.ToScreenXY(pontoInicial,
                                        pontoFinal,
                                        pontoInicialGlobal,
                                        pontoFinalGlobal);

            return  new PinRenderer(pos, engine)
            {
                Cor = Color.FromHex("#313639")
            };
        }

        private static ImageSource PegarAvatar(IAmACommunityMember membro)
        {
            if (string.IsNullOrWhiteSpace(membro.GravatarHash))
                return null;

            var avatarUri = new Uri($"{Constantes.Gravatar.URL_BASE}{membro.GravatarHash}.jpg?s={Constantes.Gravatar.TAMANHO_PADRAO}&d=mm");
            var avatar = ImageSource.FromUri(avatarUri);
            return avatar;
        }
    }
}
