//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.Notifications.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Threading;
    using ViewModels;

    public class NotificationsModel
    {
        #region Attributes
        /// <summary>
        /// Store a constant width of the notification.
        /// </summary>
        private const int NotificationWidth = 300;

        /// <summary>
        /// Store a constant height of the notification.
        /// </summary>
        private const int NotificationHeight = 121;

        /// <summary>
        /// Store the current notification windows.
        /// </summary>
        public List<ToastViewModel> Notifications;
        #endregion

        #region Methods
        /// <summary>
        /// Constructor of the model. Instantiate the toasts list and setup a timer which will help control the loading of each one.
        /// </summary>
        public NotificationsModel()
        {
            Notifications = new List<ToastViewModel>();
            var notificationsTimer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 3) };
            notificationsTimer.Tick += OnToastsTimerTick;
            notificationsTimer.Start();
        }

        /// <summary>
        /// This function will see if there are any toast notifications waiting to be displayed.
        /// It will then call for the toast to be properly positioned on the user's monitor.
        /// </summary>
        private void OnToastsTimerTick(object sender, EventArgs e)
        {
            if (Notifications.All(o => o.IsActive))
                return;

            var first = Notifications.FirstOrDefault(o => o.IsActive == false);

            if (first == null)
                return;

            PositionTheNotification(first);
        }

        /// <summary>
        /// Position the notification in the bottom right of the screen and stack it on top of the other notifications if other exist.
        /// By getting how many other active notification windows there are, euther set the notification to the bottom right or set it so that it is above the last one.
        /// </summary>
        private void PositionTheNotification(ToastViewModel toast)
        {
            var width = SystemParameters.WorkArea.Width;
            var height = SystemParameters.WorkArea.Height;

            var i = GetActiveToastsCount();
            if (i > 0)
            {
                toast.Top = Notifications.Last(o => o.IsActive).Top - NotificationHeight;
            }
            else
            {
                toast.Top = height - NotificationHeight;
            }

            toast.Left = width - NotificationWidth;
            toast.Visible = Visibility.Visible;
            toast.IsActive = true;
        }

        /// <summary>
        /// Return the active toasts count.
        /// </summary>
        private int GetActiveToastsCount()
        {
            return Notifications.Count(o => o.IsActive);
        }

        /// <summary>
        /// This function cycles through the active toasts and move them down the screen.
        /// </summary>
        public void LowerActivePopUps(int removedId)
        {
            if (GetActiveToastsCount() == 0)
                return;

            foreach (var notification in Notifications.Where(o => o.Id > removedId))
            {
                if (notification.Id == 0)
                    continue;

                notification.Top += NotificationHeight;
                notification.Id--;
            }
        }
        #endregion
    }
}   //ImprezGarage.Modules.Notifications.DataModel namespace 