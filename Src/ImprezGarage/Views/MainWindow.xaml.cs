//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2020
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Views
{
    using Infrastructure.BaseClasses;
    using System.Windows;
    using System.Windows.Controls;
    using ViewModels;
    using System.Windows.Controls.Primitives;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : DialogViewBase
    {
        public MainWindow()
        {
            InitializeComponent();

            if (!(DataContext is MainWindowViewModel model))
                return;

            model.CloseRequest += (sender, e) => Close();
            model.HideRequest += (sender, e) => Hide();
            model.ShowWindowRequest += (sender, e) => Activate();
        }

        private void BtnAccountOnClick(object sender, RoutedEventArgs e)
        {
            var cm = ((Button)sender).ContextMenu;
            if (cm == null)
                return;

            cm.PlacementTarget = (Button) sender;
            cm.Placement = PlacementMode.Bottom;
            cm.IsOpen = true;
        }
        
        private void BtnAccountOnContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            e.Handled = true;
        }
    }
}   //ImprezGarage.Views namespace 