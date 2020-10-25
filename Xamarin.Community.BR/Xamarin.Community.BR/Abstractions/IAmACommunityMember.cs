using Xamarin.Community.BR.Helpers;

namespace Xamarin.Community.BR.Abstractions
{
    public interface IAmACommunityMember
    {
        string Nome { get; }
        string SobreNome { get; }
        string GravatarHash { get; }
        GeoLocalizacao Localizacao { get; }
    }
}
