//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Infrastructure.Dialogs
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    public interface IDialogService
    {
        bool Confirm(string message, string header = "Alert!");
        void Alert(string message, string header = "Alert!");
        void ShowWindow(Action<object, Exception> callback, UserControl view, int width, int height, ResizeMode resizeMode = ResizeMode.NoResize,
            bool allowMultipleInstances = false);
    }
}   //ImprezGarage.Infrastructure.Dialogs namespace 