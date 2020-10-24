using Xamarin.Community.BR.Abstractions;
using Xamarin.Community.BR.Helpers;

namespace Xamarin.Community.BR.Programadores
{
    public sealed class FelipeBaltazar : IAmACommunityMember
    {
        public string Nome => "Felipe";

        public string SobreNome => "Baltazar";

        public string GravatarHash => "c4deac62305f590fbda80209628afd0e";

        public GeoLocalizacao Localizacao => new GeoLocalizacao(-29.940163, -51.088751);
    }
}
