using System;
using System.Globalization;

namespace Xamarin.Community.BR.Extensions
{
    public static class NumberExtensions
    {
        public static float ToSingle(this double valor, CultureInfo cultura = null) =>
            Convert.ToSingle(valor, cultura ?? CultureInfo.InvariantCulture);
    }
}
