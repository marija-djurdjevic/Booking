using BookingApp.DTO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BookingApp.Dto;

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for AddProperty.xaml
    /// </summary>
    public partial class AddProperty : Page
    {
        private PropertyDto _propertyDto;

        PropertyRepository propertyRepository;
        public AddProperty()
        {
            InitializeComponent();
            _propertyDto = new PropertyDto();
            DataContext = _propertyDto;
            propertyRepository = new PropertyRepository();
        }
        private void AddProperty_Click(object sender, RoutedEventArgs e)
        {
            PropertyDto newPropertyDto = new PropertyDto(_propertyDto.Name, _propertyDto.LocationDto, _propertyDto.Type, _propertyDto.MaxGuests, _propertyDto.MinReservationDays, _propertyDto.CancellationDeadline, _propertyDto.ImagesPaths);
            propertyRepository.AddProperty(newPropertyDto.ToProperty());
            int id = propertyRepository.NextId() - 1;
            
            MessageBox.Show("Property created successfully!");
            //this.Close();


        }
        private void AddImagePathButton_Click(object sender, RoutedEventArgs e)
        {

            string newImagePath = Microsoft.VisualBasic.Interaction.InputBox("Enter a new image path:", "Add Image Path", "");

            if (!string.IsNullOrEmpty(newImagePath))
            {

                _propertyDto.ImagesPaths.Add(newImagePath);
            }
        }

       
    }
}
