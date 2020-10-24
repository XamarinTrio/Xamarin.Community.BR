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
            var avatarUri = new Uri($"{Constantes.Gravatar.URL_BASE}{membro.GravatarHash}");
            var avatar = ImageSource.FromUri(avatarUri);
            var localizacao = membro.Localizacao;
            
            return new Pin(localizacao.Latitude, localizacao.Longitude, avatar);
        }
    }
}
