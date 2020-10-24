using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Community.BR.Helpers;

namespace Xamarin.Community.BR.ViewModels
{
    public abstract class BaseViewModel : ObservableObject
    {
        private bool estaOcupado;

        public bool EstaOcupado
        {
            get => estaOcupado;
            set => SetProperty(ref estaOcupado, value);
        }

        /// <summary>
        /// Executa uma tarefa demorada definindo 'EstaOcupado = true' no inicio e 'EstaOcupado = false' ao terminar a tarefa
        /// </summary>
        /// <param name="tarefa">Tarefa a ser executada</param>
        /// <param name="token">Token de cancelamento</param>
        /// <returns></returns>
        protected virtual async Task ExecutarTarefa(Func<Task> tarefa, CancellationToken token)
        {
            if (EstaOcupado || token.IsCancellationRequested)
                return;

            EstaOcupado = true;

            try
            {
                var taskCompletationSource = new TaskCompletionSource<bool>();
                using (var registration = token.Register(() => taskCompletationSource.SetCanceled()))
                {
                    await Task.WhenAny(tarefa(), taskCompletationSource.Task).ConfigureAwait(false);
                }
            }
            finally
            {
                EstaOcupado = false;
            }
        }
    }
}
