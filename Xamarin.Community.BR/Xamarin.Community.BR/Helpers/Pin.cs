using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Community.BR.Abstractions;
using Xamarin.Forms;

namespace Xamarin.Community.BR.Helpers
{
    public struct Pin : IPin, IEquatable<Pin>
    {
        private readonly GeoLocalizacao _geolocalizacao;
        private readonly ImageSource _avatar;

        public Pin(GeoLocalizacao geolocalizacao, ImageSource avatar)
        {
            _geolocalizacao = geolocalizacao;
            _avatar = avatar;
        }

        #region IPin

        public ImageSource GetAvatar() => _avatar;

        public GeoLocalizacao GetGeolocalizacao() => _geolocalizacao;

        #endregion

        #region IEquatable<GeoLocalizacao>

        public bool Equals(Pin other)
        {
            return _geolocalizacao.Latitude == other._geolocalizacao.Latitude &&
                   _geolocalizacao.Longitude == other._geolocalizacao.Longitude &&
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
