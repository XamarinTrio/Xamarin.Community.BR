using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Community.BR.Abstractions;
using Xamarin.Forms;

namespace Xamarin.Community.BR.Helpers
{
    public struct Pin : IPin, IEquatable<Pin>
    {
        private readonly double _latitude;
        private readonly double _longitute;
        private readonly ImageSource _avatar;

        public Pin(double latitude, double longitude, ImageSource avatar)
        {
            _latitude = latitude;
            _longitute = longitude;
            _avatar = avatar;
        }

        #region IPin

        public ImageSource GetAvatar() => _avatar;

        public double GetLatitude() => _latitude;

        public double GetLongitude() => _longitute;

        #endregion

        #region IEquatable<GeoLocalizacao>

        public bool Equals(Pin other)
        {
            return _latitude == other._latitude &&
                   _longitute == other._longitute &&
                   _avatar == other._avatar;
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (obj is Pin pin)
                return Equals(pin);

            return false;
        } 
    }
}
