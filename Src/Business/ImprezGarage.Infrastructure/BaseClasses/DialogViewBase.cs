using MahApps.Metro.Controls;
using System.Windows.Input;

namespace ImprezGarage.Infrastructure.BaseClasses
{
    public class DialogViewBase : MetroWindow
    {
        /// <summary>
        /// When the user clicks the left button on the mouse, they can drag the window around.
        /// </summary>
        public void OnDragMove(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
