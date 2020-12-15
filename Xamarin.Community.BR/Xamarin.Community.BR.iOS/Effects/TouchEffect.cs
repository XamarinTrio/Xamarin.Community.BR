using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

using UIKit;

using ITouchEffect = Xamarin.Community.BR.iOS.Effects.TouchEffect;
using CrossTouchEffect = Xamarin.Community.BR.Helpers.TouchEffect;
using Xamarin.Community.BR.iOS.Helpers;

[assembly: ResolutionGroupName("Xamarin.Community.BR")]
[assembly: ExportEffect(typeof(ITouchEffect), "TouchEffect")]
namespace Xamarin.Community.BR.iOS.Effects
{
    public sealed class  TouchEffect : PlatformEffect
    {
        private UIView view;
        private TouchRecognizer touchRecognizer;

        protected override void OnAttached()
        {
            view = Control == null ? Container : Control;

            // Uncomment this line if the UIView does not have touch enabled by default
            //view.UserInteractionEnabled = true;

            var effect = (CrossTouchEffect)Element.Effects.FirstOrDefault(e => e is CrossTouchEffect);

            if (effect != null && view != null)
            {
                touchRecognizer = new TouchRecognizer(Element, view, effect);
                view.AddGestureRecognizer(touchRecognizer);
            }
        }

        protected override void OnDetached()
        {
            if (touchRecognizer is null)
                return;

            touchRecognizer.Detach();
            view.RemoveGestureRecognizer(touchRecognizer);
        }
    }
}
