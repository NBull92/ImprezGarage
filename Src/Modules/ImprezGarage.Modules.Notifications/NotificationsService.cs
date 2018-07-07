//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.Notifications
{
    using ImprezGarage.Infrastructure.Services;
    using ImprezGarage.Modules.Notifications.ViewModels;
    using ImprezGarage.Modules.Notifications.Views;
    using System;

    internal class NotificationsService : INotificationsService
    {
        public void Alert(string message, string header = "Alert")
        {
            var alert = new Alert();
            var viewModel = alert.DataContext as AlertViewModel;
            viewModel.Header = header;
            viewModel.Message = message;
            alert.ShowDialog();
        }

        public bool Confirm(string message, string header = "Confirm", Action action = null)
        {
            var confirm = new Confirm();
            var viewModel = confirm.DataContext as ConfirmViewModel;
            viewModel.Header = header;
            viewModel.Message = message;
            confirm.ShowDialog();

            // Check if the user clicked 'yes/okay' then perform the passed through function called action.
            if(viewModel.DialogResult && action != null)
            {
                action();
            }

            return viewModel.DialogResult;
        }

        public void Toast(string message, string header = "ImprezGarage Alert", string buttonContent = null, Action action = null)
        {
            var toast = new Toast();
            var viewModel = toast.DataContext as ToastViewModel;
            viewModel.Header = header;
            viewModel.Message = message;
            viewModel.ButtonContent = buttonContent;
            toast.Closed += (s, a) =>
            {
                // Check if the user clicked 'yes/okay' then perform the passed through function called action.
                if (viewModel.DialogResult && action != null)
                {
                    action();
                }
            };
            toast.Show();
            toast.BringIntoView();
        }
    }
}   //ImprezGarage.Modules.Notifications namespace