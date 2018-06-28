//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.Notifications
{
    using ImprezGarage.Infrastructure.Services;
    using ImprezGarage.Modules.Notifications.ViewModels;
    using ImprezGarage.Modules.Notifications.Views;

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

        public bool Confirm(string message, string header = "Confirm")
        {
            var confirm = new Confirm();
            var viewModel = confirm.DataContext as ConfirmViewModel;
            viewModel.Header = header;
            viewModel.Message = message;
            confirm.ShowDialog();

            return viewModel.DialogResult;
        }
    }
}   //ImprezGarage.Modules.Notifications namespace