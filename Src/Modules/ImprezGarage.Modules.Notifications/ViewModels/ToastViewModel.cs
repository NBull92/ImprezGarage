//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.Notifications.ViewModels
{
    using System;
    using System.Windows;
    using ImprezGarage.Infrastructure.BaseClasses;
    using Prism.Commands;

    public class ToastViewModel : DialogViewModelBase
    {
        #region Attributes
        /// <summary>
        /// The message that will appear on in the content of the toast.
        /// </summary>
        private string _message;

        /// <summary>
        /// The header of the toast view.
        /// </summary>
        private string _header;

        /// <summary>
        /// The text that will appear on the proceed button if required.
        /// </summary>
        private string _buttonContent;

        /// <summary>
        /// Store the left position of the view.
        /// </summary>
        private double _left;

        /// <summary>
        /// Store the top position of the view.
        /// </summary>
        private double _top;

        /// <summary>
        /// The visibility of notification window.
        /// </summary>
        private Visibility _visible;
        #endregion

        #region Properties
        /// <summary>
        /// Store the id of this toast.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Store whether this toast has been activiated or not.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// The message that will appear on in the content of the toast.
        /// </summary>
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        /// <summary>
        /// The header of the toast view.
        /// </summary>
        public string Header
        {
            get => _header;
            set => SetProperty(ref _header, value);
        }

        /// <summary>
        /// The text that will appear on the proceed button if required.
        /// </summary>
        public string ButtonContent
        {
            get => _buttonContent;
            set => SetProperty(ref _buttonContent, value);
        }

        /// <summary>
        /// Store the left position of the view.
        /// </summary>
        public double Left
        {
            get => _left;
            set => SetProperty(ref _left, value);
        }

        /// <summary>
        /// Store the top position of the view.
        /// </summary>
        public double Top
        {
            get => _top;
            set => SetProperty(ref _top, value);
        }

        /// <summary>
        /// The visibility of notification window.
        /// </summary>
        public Visibility Visible
        {
            get => _visible;
            set => SetProperty(ref _visible, value);
        }

        /// <summary>
        /// The action to be preformed when the proceed command is called.
        /// </summary>
        public Action Action { get; set; }

        #region Command
        /// <summary>
        /// A command for the only button on the toast.
        /// There to proceed with an associated Action of this view model.
        /// </summary>
        public DelegateCommand Proceed { get; set; }
        #endregion
        #endregion

        #region Methods
        /// <summary>
        /// Construct the toast view model and instantiate the proceed command to it's execute function.
        /// </summary>
        public ToastViewModel()
        {
            Proceed = new DelegateCommand(OnProceed);
        }

        #region Command Handlers
        /// <summary>
        /// Call the Action of this toast and then close the view.
        /// </summary>
        private void OnProceed()
        {
            Action?.Invoke();
            Close();
        }
        #endregion
        #endregion
    }
}   //ImprezGarage.Modules.Notifications.ViewModels namespace 