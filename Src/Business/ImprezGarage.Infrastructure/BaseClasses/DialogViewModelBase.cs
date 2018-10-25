﻿//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Infrastructure.BaseClasses
{
    using Prism.Mvvm;
    using System;

    public class DialogViewModelBase : BindableBase, IDisposable
    {
        #region Attributes
        /// <summary>
        /// An event handler request for closing the associated view.
        /// </summary>
        public event EventHandler CloseRequest;

        /// <summary>
        /// An event handler request for dragging and moving the associated view.
        /// </summary>
        public event EventHandler DragMoveRequest;
        #endregion

        #region Properties
        /// <summary>
        /// Set to true or false dependant on whether the Okay button or Cancel button are clicked.
        /// </summary>
        public bool DialogResult { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Request the Close event for the window.
        /// This will close the window.
        /// </summary>
        public void Close()
        {
            DialogResult = false;
            Dispose();
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Call the drag move event, which will allow the user to move the associated view around.
        /// </summary>
        public void Move()
        {
            DragMoveRequest?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Base dispose function for children of this class to override.
        /// </summary>
        public virtual void Dispose()
        {

        }
        #endregion
    }
}  //ImprezGarage.Infrastructure.BaseClasses namespace 