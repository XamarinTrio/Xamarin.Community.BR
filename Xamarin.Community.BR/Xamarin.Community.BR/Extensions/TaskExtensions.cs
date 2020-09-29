using System;
using System.Threading.Tasks;

namespace Xamarin.Community.BR.Extensions
{
    internal static class TaskExtensions
    {
        private static Action<Exception> aoDispararExcecaoPadrao;

        internal static void DefinirAoDispararExcecaoPadrao(Action<Exception> acao)
        {
            aoDispararExcecaoPadrao = acao;
        }

        internal static async void TentarExecutar(this Task task, bool continuarNoContextoCapturado = false, Action<Exception> aoDispararExcecao = null)
        {
            try
            {
                await task.ConfigureAwait(continuarNoContextoCapturado);
            }
            catch (Exception ex) when (aoDispararExcecao != null || aoDispararExcecaoPadrao != null)
            {
                (aoDispararExcecao ?? aoDispararExcecaoPadrao).Invoke(ex);
            }
        }
    }
}
