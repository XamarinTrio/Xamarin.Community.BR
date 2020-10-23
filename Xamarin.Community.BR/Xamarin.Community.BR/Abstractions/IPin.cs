using Xamarin.Forms;

namespace Xamarin.Community.BR.Abstractions
{
    public interface IPin
    {
        double GetLatitude();
        double GetLongitude();

        ImageSource GetAvatar();
    }
}
