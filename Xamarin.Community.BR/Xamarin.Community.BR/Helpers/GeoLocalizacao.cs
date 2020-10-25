using System;

namespace Xamarin.Community.BR.Helpers
{
    public struct GeoLocalizacao : IEquatable<GeoLocalizacao>
    {
        public double Latitude { get; }
        public double Longitude { get; }

        public GeoLocalizacao(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        #region IEquatable<GeoLocalizacao>

        public bool Equals(GeoLocalizacao other)
        {
            return Latitude == other.Latitude &&
                   Longitude == other.Longitude;
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (obj is GeoLocalizacao geo)
                return Equals(geo);

            return false;
        }
    }
}
