//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.ViewModels
{
    using Infrastructure.Services;
    using Infrastructure;
    using Infrastructure.BaseClasses;
    using Microsoft.Practices.ServiceLocation;
    using Prism.Commands;
    using Prism.Events;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using System.Windows.Media.Imaging;

    public class MainWindowViewModel : DialogViewModelBase
    {
        #region Attibute
        /// <summary>
        /// Store the injected event aggregator.
        /// </summary>
        private readonly IEventAggregator _eventAggregator;
        #endregion

        #region Properties
        /// <summary>
        /// Store the title of the main window.
        /// </summary>
        private string _title = "Imprez Garage";
        public string Title
        {
            get => _title; 
            set => SetProperty(ref _title, value); 
        }

        /// <summary>
        /// Store whether the settings view is currently open or not.
        /// </summary>
        private bool _isSettingsOpen;
        public bool IsSettingsOpen
        {
            get => _isSettingsOpen;
            set => SetProperty(ref _isSettingsOpen, value);
        }

        public string Icon { get; } = "pack://application:,,,/ImprezGarage;component/Resources/icon_v2.png";

        private string _signInOut;
        public string SignInOut
        {
            get => _signInOut;
            set => SetProperty(ref _signInOut, value);
        }

        private bool _isBusy = true;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }
        
        #region Commands
        /// <summary>
        /// Command for refreshing the current data.
        /// </summary>
        public DelegateCommand RefreshCommand { get; set; }

        /// <summary>
        /// Command for showing and closing the settings view.
        /// </summary>
        public DelegateCommand Settings { get; set; }
        public DelegateCommand MinimizeToTray { get; set; }
        public DelegateCommand SignOut { get; set; }
        #endregion
        #endregion

        #region Methods
        /// <summary>
        /// Construct this view model and inject the event aggregator.
        /// </summary>
        public MainWindowViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            RefreshCommand = new DelegateCommand(RefreshExecute);
            Settings = new DelegateCommand(OnSettingsClicked);
            MinimizeToTray = new DelegateCommand(Hide);
            SignOut = new DelegateCommand(OnSignOut);

            _eventAggregator.GetEvent<Events.UserAccountChange>().Subscribe(OnUserAccountChange);
            SignInOut = "Sign In";
            CreateSystemTrayIcon();
        }
        
        private void OnUserAccountChange(Tuple<bool, string> loginData)
        {
            IsBusy = !loginData.Item1;
            if (IsBusy)
            {
                SignInOut = "Sign In";
                ServiceLocator.Current.GetInstance<IAuthenticationService>().SignIn();
            }
            else
            {
                SignInOut ="Sign Out";
            }
        }

        private void OnSignOut()
        {
            _eventAggregator.GetEvent<Events.UserAccountChange>().Publish(new Tuple<bool, string>(false, string.Empty));
        }

        private void CreateSystemTrayIcon()
        {
            var icon = ConvertFromBitmapFrame("pack://application:,,,/ImprezGarage;component/Resources/icon_v2.png");

            var notifyIcon = new NotifyIcon
            {
                Icon = icon,
                Visible = true,
                ContextMenu = new ContextMenu(),
                Text = Title,
            };
            notifyIcon.MouseDoubleClick += OnNotifyIcon_MouseDoubleClick;

            var itemOpen = new MenuItem("Open",
                (o, e) =>
                {
                    ShowWindow();
                });

            var itemQuit = new MenuItem("Exit",
                (o, e) =>
                {
                    Close();
                });

            notifyIcon.ContextMenu.MenuItems.Add(itemOpen);
            notifyIcon.ContextMenu.MenuItems.Add("-");
            notifyIcon.ContextMenu.MenuItems.Add(itemQuit);
        }

        /// <summary>
        /// An called when the system tray icon is double clicked.
        /// This reactivates the window and forces to the front.
        /// </summary>
        private void OnNotifyIcon_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            ShowWindow();
        }

        /// <summary>
        /// Takes in a filename of an image and converts it to an icon.
        /// </summary>
        private static Icon ConvertFromBitmapFrame(string fileName)
        {
            var uri = new Uri(fileName, UriKind.RelativeOrAbsolute);
            var source = BitmapFrame.Create(uri);
            var ms = new MemoryStream();
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(source);
            encoder.Save(ms);
            ms.Seek(0, SeekOrigin.Begin);
            var bmp = new Bitmap(ms);
            return System.Drawing.Icon.FromHandle(bmp.GetHicon());
        }
        
        /// <summary>
        /// When the settings command is clicked, set IsSettingsOpen to it's opposite.
        /// </summary>
        private void OnSettingsClicked()
        {
            IsSettingsOpen = !IsSettingsOpen;
        }

        /// <summary>
        /// When the user clicks the refresh, publish the refresh data event and that will call any subscribers.
        /// </summary>
        private void RefreshExecute()
        {
            _eventAggregator.GetEvent<Events.RefreshDataEvent>().Publish();
        }
        #endregion
    }
}   //ImprezGarage.ViewModels namespace 