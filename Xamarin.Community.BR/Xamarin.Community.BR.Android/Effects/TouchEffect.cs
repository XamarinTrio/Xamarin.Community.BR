using System;
using System.Collections.Generic;
using System.Linq;
using Android.Views;
using Xamarin.Community.BR.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ATouchEffect = Xamarin.Community.BR.Droid.Effects.TouchEffect;
using CrossTouchEffect = Xamarin.Community.BR.Helpers.TouchEffect;
using AView = Android.Views.View;

[assembly: ResolutionGroupName("Xamarin.Community.BR")]
[assembly: ExportEffect(typeof(ATouchEffect), "TouchEffect")]
namespace Xamarin.Community.BR.Droid.Effects
{
    public class TouchEffect : PlatformEffect
    {
        private static readonly Dictionary<AView, ATouchEffect> _viewDictionary =
            new Dictionary<AView, ATouchEffect>();

        private static readonly Dictionary<int, ATouchEffect> _idToEffectDictionary =
            new Dictionary<int, ATouchEffect>();

        private readonly int[] _twoIntArray = new int[2];

        private AView view;
        private Element formsElement;
        private CrossTouchEffect libTouchEffect;

        private bool capture;
        private Func<double, double> fromPixels;

        protected override void OnAttached()
        {
            // Get the Android View corresponding to the Element that the effect is attached to
            view = Control == null ? Container : Control;

            // Get access to the TouchEffect class in the .NET Standard library
            var touchEffect =
                (CrossTouchEffect)Element.Effects.
                    FirstOrDefault(e => e is CrossTouchEffect);

            if (touchEffect != null && view != null)
            {
                _ = _viewDictionary.TryAdd(view, this);

                formsElement = Element;

                libTouchEffect = touchEffect;

                // Save fromPixels function
                fromPixels = view.Context.FromPixels;

                // Set event handler on View
                view.Touch += OnTouch;
            }
        }

        protected override void OnDetached()
        {
            if (!_viewDictionary.ContainsKey(view))
                return;

            _viewDictionary.Remove(view);
            view.Touch -= OnTouch;
        }

        private void OnTouch(object sender, AView.TouchEventArgs args)
        {
            var senderView = sender as AView;
            var motionEvent = args.Event;

            var pointerIndex = motionEvent.ActionIndex;

            var id = motionEvent.GetPointerId(pointerIndex);

            senderView.GetLocationOnScreen(_twoIntArray);
            Point screenPointerCoords = new Point(_twoIntArray[0] + motionEvent.GetX(pointerIndex),
                                                  _twoIntArray[1] + motionEvent.GetY(pointerIndex));


            // Use ActionMasked here rather than Action to reduce the number of possibilities
            switch (args.Event.ActionMasked)
            {
                case MotionEventActions.Down:
                case MotionEventActions.PointerDown:
                    FireEvent(this, id, TouchActionType.Pressed, screenPointerCoords, true);

                    _ = _idToEffectDictionary.TryAdd(id, this);

                    capture = libTouchEffect.Capture;
                    break;

                case MotionEventActions.Move:
                    // Multiple Move events are bundled, so handle them in a loop
                    for (pointerIndex = 0; pointerIndex < motionEvent.PointerCount; pointerIndex++)
                    {
                        id = motionEvent.GetPointerId(pointerIndex);

                        if (capture)
                        {
                            senderView.GetLocationOnScreen(_twoIntArray);

                            screenPointerCoords = new Point(_twoIntArray[0] + motionEvent.GetX(pointerIndex),
                                                            _twoIntArray[1] + motionEvent.GetY(pointerIndex));

                            FireEvent(this, id, TouchActionType.Moved, screenPointerCoords, true);
                        }
                        else
                        {
                            CheckForBoundaryHop(id, screenPointerCoords);

                            if (_idToEffectDictionary[id] != null)
                            {
                                FireEvent(_idToEffectDictionary[id], id, TouchActionType.Moved, screenPointerCoords, true);
                            }
                        }
                    }
                    break;

                case MotionEventActions.Up:
                case MotionEventActions.Pointer1Up:
                    if (capture)
                    {
                        FireEvent(this, id, TouchActionType.Released, screenPointerCoords, false);
                    }
                    else
                    {
                        CheckForBoundaryHop(id, screenPointerCoords);

                        if (_idToEffectDictionary[id] != null)
                        {
                            FireEvent(_idToEffectDictionary[id], id, TouchActionType.Released, screenPointerCoords, false);
                        }
                    }
                    _idToEffectDictionary.Remove(id);
                    break;

                case MotionEventActions.Cancel:
                    if (capture)
                    {
                        FireEvent(this, id, TouchActionType.Cancelled, screenPointerCoords, false);
                    }
                    else
                    {
                        if (_idToEffectDictionary[id] != null)
                        {
                            FireEvent(_idToEffectDictionary[id], id, TouchActionType.Cancelled, screenPointerCoords, false);
                        }
                    }
                    _idToEffectDictionary.Remove(id);
                    break;
            }
        }

        private void CheckForBoundaryHop(int id, Point pointerLocation)
        {
            TouchEffect touchEffectHit = null;

            foreach (AView view in _viewDictionary.Keys)
            {
                // Get the view rectangle
                try
                {
                    view.GetLocationOnScreen(_twoIntArray);
                }
                catch // System.ObjectDisposedException: Cannot access a disposed object.
                {
                    continue;
                }
                Rectangle viewRect = new Rectangle(_twoIntArray[0], _twoIntArray[1], view.Width, view.Height);

                if (viewRect.Contains(pointerLocation))
                {
                    touchEffectHit = _viewDictionary[view];
                }
            }

            if (touchEffectHit != _idToEffectDictionary[id])
            {
                if (_idToEffectDictionary[id] != null)
                {
                    FireEvent(_idToEffectDictionary[id], id, TouchActionType.Exited, pointerLocation, true);
                }
                if (touchEffectHit != null)
                {
                    FireEvent(touchEffectHit, id, TouchActionType.Entered, pointerLocation, true);
                }
                _idToEffectDictionary[id] = touchEffectHit;
            }
        }

        private void FireEvent(TouchEffect touchEffect, int id, TouchActionType actionType, Point pointerLocation, bool isInContact)
        {
            // Get the method to call for firing events
            Action<Element, TouchActionEventArgs> onTouchAction = touchEffect.libTouchEffect.OnTouchAction;

            // Get the location of the pointer within the view
            touchEffect.view.GetLocationOnScreen(_twoIntArray);
            double x = pointerLocation.X - _twoIntArray[0];
            double y = pointerLocation.Y - _twoIntArray[1];
            Point point = new Point(fromPixels(x), fromPixels(y));

            // Call the method
            onTouchAction(touchEffect.formsElement,
                new TouchActionEventArgs(id, actionType, point, isInContact));
        }
    }
}
