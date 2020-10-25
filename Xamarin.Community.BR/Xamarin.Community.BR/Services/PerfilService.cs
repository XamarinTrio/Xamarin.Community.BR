using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xamarin.Community.BR.Abstractions;

namespace Xamarin.Community.BR.Services
{
    public sealed class PerfilService : IPerfilService
    {
        private readonly Assembly _assembly;
        private readonly Type _tipoInterface;

        public PerfilService()
        {
            _assembly = Assembly.GetExecutingAssembly();
            _tipoInterface = typeof(IAmACommunityMember);
        }

        public IEnumerable<IAmACommunityMember> PegarTodos()
        {
            var tiposColecao = _assembly.GetTypes()
                                        .Where(p => _tipoInterface.IsAssignableFrom(p) && !p.IsInterface);

            foreach (var tipo in tiposColecao)
            {
                yield return (IAmACommunityMember)Activator.CreateInstance(tipo);
            }
        }
    }
}
