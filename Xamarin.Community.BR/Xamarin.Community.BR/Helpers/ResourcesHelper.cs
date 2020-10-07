using System.IO;
using System.Reflection;

namespace Xamarin.Community.BR.Helpers
{
    public static class ResourcesHelper
    {
        private static readonly Assembly _assembly = Assembly.GetExecutingAssembly();

        public static Stream CarregarRecurso(string nome)
        {
            return _assembly.GetManifestResourceStream($"Xamarin.Community.BR.Resources.{nome}");
        }
    }
}
