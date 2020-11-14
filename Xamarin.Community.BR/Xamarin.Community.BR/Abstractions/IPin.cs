using Xamarin.Community.BR.Helpers;
using Xamarin.Forms;

namespace Xamarin.Community.BR.Abstractions
{
    public interface IPin
    {
        GeoLocalizacao GetGeolocalizacao();

        ImageSource GetAvatar();
    }
}
