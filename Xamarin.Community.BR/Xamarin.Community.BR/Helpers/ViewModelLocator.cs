using System;
using System.Globalization;
using System.Reflection;
using TinyIoC;
using Xamarin.Forms;

namespace Xamarin.Community.BR.Helpers
{
    public static class ViewModelLocator
    {
        public static void LocalizaViewModel(this BindableObject bindable, TinyIoCContainer container)
        {
            var view = bindable as Element;
            if (view == null)
                return;

            if (view.BindingContext != null)
                return;

            var viewType = view.GetType();
            var viewName = viewType.FullName.Replace(".Views.", ".ViewModels.");
            var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
            var viewModelName = string.Format(
                CultureInfo.InvariantCulture, "{0}ViewModel, {1}", viewName, viewAssemblyName);

            var viewModelType = Type.GetType(viewModelName);
            if (viewModelType == null)
            {
                return;
            }

            var viewModel = container.Resolve(viewModelType);
            view.BindingContext = viewModel;
        }
    }
}
