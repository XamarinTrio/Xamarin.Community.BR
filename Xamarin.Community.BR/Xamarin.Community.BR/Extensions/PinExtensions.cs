using System;
using Xamarin.Community.BR.Abstractions;
using Xamarin.Community.BR.Helpers;
using Xamarin.Forms;

namespace Xamarin.Community.BR.Extensions
{
    public static class PinExtensions
    {
        public static Pin ToPin(this IAmACommunityMember membro)
        {
            var avatar = PegarAvatar(membro);
            var localizacao = membro.Localizacao;

            return new Pin(localizacao.Latitude, localizacao.Longitude, avatar);
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
