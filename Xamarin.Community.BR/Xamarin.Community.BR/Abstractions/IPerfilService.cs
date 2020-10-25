using System.Collections.Generic;

namespace Xamarin.Community.BR.Abstractions
{
    public interface IPerfilService
    {
        IEnumerable<IAmACommunityMember> PegarTodos();
    }
}
