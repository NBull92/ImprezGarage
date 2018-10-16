//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.Notifications
{
    using Infrastructure.Services;
    using ViewModels;
    using Views;
    using System;

    internal class NotificationsService : INotificationsService
    {
        #region Methods
        /// <summary>
        /// Create a simple alert to inform or warn a user.
        /// </summary>
        public void Alert(string message, string header = "Alert")
        {
            var alert = new Alert();
            if (alert.DataContext is AlertViewModel viewModel)
            {
                viewModel.Header = header;
                viewModel.Message = message;
            }

            alert.ShowDialog();
        }

        /// <summary>
        /// Create an alert that will allow the user to confirm something.
        /// Then return result of the users action and called any 'action'.
        /// </summary>
        public bool Confirm(string message, string header = "Confirm", Action action = null)
        {
            var confirm = new Confirm();

            if (!(confirm.DataContext is ConfirmViewModel viewModel))
                return false;

            viewModel.Header = header;
            viewModel.Message = message;
            confirm.ShowDialog();

            // Check if the user clicked 'yes/okay' then perform the passed through function called action.
            if (viewModel.DialogResult)
            {
                action?.Invoke();
            }

            return viewModel.DialogResult;
        }

        /// <summary>
        /// Create a desktop notification to inform the user of important information.
        /// The passed through action will allow them to proceed with whatever you want them too.
        /// </summary>
        public void Toast(string message, string header = "ImprezGarage Alert", string buttonContent = null, Action action = null)
        {
            var toast = new Toast();

            if (toast.DataContext is ToastViewModel viewModel)
            {
                viewModel.Header = header;
                viewModel.Message = message;
                viewModel.ButtonContent = buttonContent;
                toast.Closed += (s, a) =>
                {
                    // Check if the user clicked 'yes/okay' then perform the passed through function called action.
                    if (viewModel.DialogResult)
                    {
                        action?.Invoke();
                    }
                };
            }

            toast.Show();
            toast.BringIntoView();
        }
        #endregion
    }
}   //ImprezGarage.Modules.Notifications namespace