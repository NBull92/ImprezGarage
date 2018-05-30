//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Infrastructure.Dialogs
{
    using MahApps.Metro.Controls;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    public class DialogService : IDialogService
    {
        //Store any created windows to help control the amount of instances created per view type.
        List<Window> _activeWindows = new List<Window>();

        public bool Confirm(string message, string header = "Alert!")
        {
            return MessageBox.Show(message, header, MessageBoxButton.YesNo) == MessageBoxResult.Yes ? true : false;            
        }
        
        /// <summary>
        /// Create the window based off of the parameters passed through.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns>The created window</returns>
        private MetroWindow CreateWindow(UserControl view, int width, int height, ResizeMode resizeMode = ResizeMode.NoResize)
        {
            MetroWindow window = new MetroWindow
            {
                Content = view
            };
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Height = height;
            window.Width = width;
            window.ResizeMode = resizeMode;
            _activeWindows.Add(window);
            window.Closed += (sender, args) => { _activeWindows.Remove(window); };
            window.Show();
            return window;
        }

        /// <summary>
        /// This shows any passed through view in a new window.
        /// </summary>
        /// <param name="callback">it wil return the view model of the view or a crash.</param>
        /// <param name="allowMultipleInstances">Some windows may need the ability to have more than once instance, this parameter allows for that. It's false bny default though.</param>
        public void ShowWindow(Action<object, Exception> callback, UserControl view, int width, int height, ResizeMode resizeMode = ResizeMode.NoResize,
            bool allowMultipleInstances = false)
        {
            if (!allowMultipleInstances)
            {
                if (_activeWindows.Any(o => ((Window)o).Content.GetType() == view.GetType()))
                {
                    var window = _activeWindows.First(o => ((Window)o).Content.GetType() == view.GetType());
                    window.Focus();
                }
                else
                {
                    var window = CreateWindow(view, width, height, resizeMode);
                    window.Closed += (sender, args) =>
                    {
                        callback(view.DataContext, null);
                    };
                }
            }
            else
            {
                var window = CreateWindow(view, width, height, resizeMode);
                window.Closed += (sender, args) =>
                {
                    callback(view.DataContext, null);
                };
            }
        }
        
        /// <summary>
        /// Opens a message box to alert the user.
        /// </summary>
        /// <param name="message">The message shown to the user.</param>
        /// <param name="header">The header for the messagebox. By defualt it's set to alert.</param>
        public void Alert(string message, string header = "Alert!")
        {
            MessageBox.Show(message, header);
        }
    }
}   //ImprezGarage.Infrastructure.Dialogs namespace 