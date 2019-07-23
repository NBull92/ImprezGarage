//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Views
{
    using Infrastructure.BaseClasses;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : DialogViewBase
    {
        public MainWindow()
        {
            InitializeComponent();

            if (!(DataContext is DialogViewModelBase model))
                return;

            model.CloseRequest += (sender, e) => Close();
            model.HideRequest += (sender, e) => Hide();
            model.ShowWindowRequest += (sender, e) => Activate();
        }
    }
}   //ImprezGarage.Views namespace 