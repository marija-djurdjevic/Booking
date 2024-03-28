using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.TouristView.ComplexTourRequests
{
    /// <summary>
    /// Interaction logic for ComlexTourRequestsPage.xaml
    /// </summary>
    public partial class ComlexTourRequestsPage : Page
    {
        public ObservableCollection<string> Images { get; set; }
        public ComlexTourRequestsPage()
        {
            InitializeComponent();
            DataContext = this;
            Images = new ObservableCollection<string>();

            Images.Add(new string("/TouristView/Icons/broken-image.png"));
            Images.Add(new string("/TouristView/Icons/help1.png"));
            Images.Add(new string("/TouristView/Icons/Slika.png"));
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (ImageListBox.SelectedIndex < ImageListBox.Items.Count - 1)
            {
                ImageListBox.SelectedIndex++;
            }
            else
            {
                // Reset na prvu sliku ako smo došli do kraja liste.
                ImageListBox.SelectedIndex = 0;
            }
        }


        private void HelpButtonClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
