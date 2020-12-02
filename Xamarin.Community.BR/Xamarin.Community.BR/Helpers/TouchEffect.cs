using Xamarin.Forms;

namespace Xamarin.Community.BR.Helpers
{
    public sealed class TouchEffect : RoutingEffect
    {
        public delegate void TouchActionEventHandler(object sender, TouchActionEventArgs args);

        public event TouchActionEventHandler TouchAction;

        public TouchEffect() : base("Xamarin.Community.BR.TouchEffect")
        {
        }

        public bool Capture { set; get; }

        public void OnTouchAction(Element element, TouchActionEventArgs args)
        {
            TouchAction?.Invoke(element, args);
        }
    }
}
