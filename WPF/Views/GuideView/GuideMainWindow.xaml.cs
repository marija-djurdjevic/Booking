using BookingApp.View.GuideView;
using BookingApp.WPF.ViewModels.GuidesViewModels;
using System.Windows;
using System.Windows.Controls;

namespace BookingApp.View
{
    public partial class GuideMainWindow : Window
    {
        public static Frame MainFrame { get; private set; }

        public GuideMainWindow()
        {
            InitializeComponent();
            MainFrame = (Frame)this.FindName("MainFrameControl");
            MainFrame.Navigate(new GuideMainPage());
            DataContext = new GuideMainWindowViewModel();
        }
    }


}
