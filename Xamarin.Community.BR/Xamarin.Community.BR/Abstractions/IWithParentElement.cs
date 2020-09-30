using Xamarin.Forms;

namespace Xamarin.Community.BR.Abstractions
{
    public interface IWithParentElement
    {
        BindableObject Parent { get; set; }
    }
}
