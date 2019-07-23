//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.ViewModels
{
    using Infrastructure;
    using Infrastructure.BaseClasses;
    using Prism.Commands;
    using Prism.Events;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Windows.Media.Imaging;


    //using System.Windows.Forms;

    public class MainWindowViewModel : DialogViewModelBase
    {
        #region Attibute
        /// <summary>
        /// Store the injected event aggregator.
        /// </summary>
        private readonly IEventAggregator _eventAggregator;

        /// <summary>
        /// Store the title of the main window.
        /// </summary>
        private string _title = "Imprez Garage";

        /// <summary>
        /// Store whether the settings view is currently open or not.
        /// </summary>
        private bool _isSettingsOpen;
        #endregion

        #region Properties
        /// <summary>
        /// Store the title of the main window.
        /// </summary>
        public string Title
        {
            get => _title; 
            set => SetProperty(ref _title, value); 
        }

        /// <summary>
        /// Store whether the settings view is currently open or not.
        /// </summary>
        public bool IsSettingsOpen
        {
            get => _isSettingsOpen;
            set => SetProperty(ref _isSettingsOpen, value);
        }

        public string Icon { get; } = "pack://application:,,,/ImprezGarage;component/icon.png";

        #region Command
        /// <summary>
        /// Command for refreshing the current data.
        /// </summary>
        public DelegateCommand RefreshCommand { get; set; }

        /// <summary>
        /// Command for showing and closing the settings view.
        /// </summary>
        public DelegateCommand Settings { get; set; }

        public DelegateCommand MinimizeToTray { get; set; }
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

            CreateSystemTrayIcon();
        }

        private void CreateSystemTrayIcon()
        {
            var icon = ConvertFromBitmapFrame("pack://application:,,,/ImprezGarage;component/icon.png");

            var notifyIcon = new System.Windows.Forms.NotifyIcon
            {
                Icon = icon,
                Visible = true,
                ContextMenu = new System.Windows.Forms.ContextMenu(),
                Text = Title,
            };
            notifyIcon.MouseDoubleClick += OnNotifyIcon_MouseDoubleClick;

            var itemOpen = new System.Windows.Forms.MenuItem("Open",
                (o, e) =>
                {
                    ShowWindow();
                });

            var itemQuit = new System.Windows.Forms.MenuItem("Exit",
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