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
    using System.Linq;
    using ImprezGarage.Modules.Notifications.DataModel;

    internal class NotificationsService : INotificationsService
    {
        #region Attributes
        /// <summary>
        /// Store the current instance of the data model.
        /// </summary>
        private NotificationsModel _notificationsModel;
        #endregion

        #region Methods
        /// <summary>
        /// Construct the service and instantiate the mode.
        /// </summary>
        public NotificationsService()
        {
            _notificationsModel = new NotificationsModel();
        }

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
            // Calculate the Id for this notification. It will either be zero or it will be one more than the Id of the last toast.
            var i = 0;

            if (_notificationsModel.Notifications.Any())
            {
                i = _notificationsModel.Notifications.Last().Id + 1;
            }

            var toast = new Toast();

            if (toast.DataContext is ToastViewModel viewModel)
            {
                viewModel.Id = i;
                viewModel.Header = header;
                viewModel.Message = message;
                viewModel.ButtonContent = buttonContent;
                viewModel.Action = action;
                toast.Closed += (s, a) =>
                {
                    _notificationsModel.LowerActivePopUps(viewModel.Id);
                };

                _notificationsModel.Notifications.Add(viewModel);
            }

            //toast.Show();
            //toast.BringIntoView();
        }
        #endregion
    }
}   //ImprezGarage.Modules.Notifications namespace