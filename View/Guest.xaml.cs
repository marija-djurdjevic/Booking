using BookingApp.Dto;
using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for Guest.xaml
    /// </summary>
    public partial class Guest : Window
    {
        public static PropertyRepository PropertyRepository = new PropertyRepository();
        public PropertyDto SelectedProperty { get; set; }
        public User LoggedInUser { get; set; }
        public Guest()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            PropertyView propertyview = new PropertyView(SelectedProperty, LoggedInUser);
            propertyview.Show();
            Close();
        }
    }
}
