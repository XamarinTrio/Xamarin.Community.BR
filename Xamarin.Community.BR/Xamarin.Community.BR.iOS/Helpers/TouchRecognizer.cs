using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;

using CoreGraphics;
using Foundation;
using UIKit;

using CrossTouchEffect = Xamarin.Community.BR.Helpers.TouchEffect;
using Xamarin.Community.BR.Helpers;

namespace Xamarin.Community.BR.iOS.Helpers
{
    public sealed class TouchRecognizer : UIGestureRecognizer
    {
        private readonly static Dictionary<UIView, TouchRecognizer> _viewDictionary =
            new Dictionary<UIView, TouchRecognizer>();

        private readonly static Dictionary<long, TouchRecognizer> _idToTouchDictionary =
            new Dictionary<long, TouchRecognizer>();

        private readonly CrossTouchEffect _touchEffect;
        private readonly Element _element;
        private readonly UIView _view;

        private bool capture;

        public TouchRecognizer(Element element, UIView view, CrossTouchEffect touchEffect)
        {
            _element = element;
            _view = view;
            _touchEffect = touchEffect;

            _viewDictionary.Add(view, this);
        }

        public void Detach()
        {
            _viewDictionary.Remove(_view);
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            foreach (UITouch touch in touches.Cast<UITouch>())
            {
                var id = touch.Handle.ToInt64();
                FireEvent(this, id, TouchActionType.Pressed, touch, true);

                if (!_idToTouchDictionary.ContainsKey(id))
                {
                    _idToTouchDictionary.Add(id, this);
                }
            }

            // Save the setting of the Capture property
            capture = _touchEffect.Capture;
        }

        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            base.TouchesMoved(touches, evt);

            foreach (UITouch touch in touches.Cast<UITouch>())
            {
                long id = touch.Handle.ToInt64();

                if (capture)
                {
                    FireEvent(this, id, TouchActionType.Moved, touch, true);
                }
                else
                {
                    CheckForBoundaryHop(touch);

                    if (_idToTouchDictionary[id] != null)
                    {
                        FireEvent(_idToTouchDictionary[id], id, TouchActionType.Moved, touch, true);
                    }
                }
            }
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);

            foreach (UITouch touch in touches.Cast<UITouch>())
            {
                long id = touch.Handle.ToInt64();

                if (capture)
                {
                    FireEvent(this, id, TouchActionType.Released, touch, false);
                }
                else
                {
                    CheckForBoundaryHop(touch);

                    if (_idToTouchDictionary[id] != null)
                    {
                        FireEvent(_idToTouchDictionary[id], id, TouchActionType.Released, touch, false);
                    }
                }
                _idToTouchDictionary.Remove(id);
            }
        }

        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled(touches, evt);

            foreach (UITouch touch in touches.Cast<UITouch>())
            {
                long id = touch.Handle.ToInt64();

                if (capture)
                {
                    FireEvent(this, id, TouchActionType.Cancelled, touch, false);
                }
                else if (_idToTouchDictionary[id] != null)
                {
                    FireEvent(_idToTouchDictionary[id], id, TouchActionType.Cancelled, touch, false);
                }

                _idToTouchDictionary.Remove(id);
            }
        }

        private void CheckForBoundaryHop(UITouch touch)
        {
            var id = touch.Handle.ToInt64();

            // TODO: Might require converting to a List for multiple hits
            TouchRecognizer recognizerHit = null;

            foreach (UIView view in _viewDictionary.Keys)
            {
                CGPoint location = touch.LocationInView(view);

                if (new CGRect(new CGPoint(), view.Frame.Size).Contains(location))
                {
                    recognizerHit = _viewDictionary[view];
                }
            }

            if (recognizerHit != _idToTouchDictionary[id])
            {
                if (_idToTouchDictionary[id] != null)
                {
                    FireEvent(_idToTouchDictionary[id], id, TouchActionType.Exited, touch, true);
                }
                if (recognizerHit != null)
                {
                    FireEvent(recognizerHit, id, TouchActionType.Entered, touch, true);
                }

                _idToTouchDictionary[id] = recognizerHit;
            }
        }

        private void FireEvent(TouchRecognizer recognizer, long id, TouchActionType actionType, UITouch touch, bool isInContact)
        {
            // Convert touch location to Xamarin.Forms Point value
            CGPoint cgPoint = touch.LocationInView(recognizer.View);
            Point xfPoint = new Point(cgPoint.X, cgPoint.Y);

            // Get the method to call for firing events
            Action<Element, TouchActionEventArgs> onTouchAction = recognizer._touchEffect.OnTouchAction;

            // Call that method
            onTouchAction(recognizer._element,
                new TouchActionEventArgs(id, actionType, xfPoint, isInContact));
        }
    }
}
