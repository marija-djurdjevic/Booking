using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.TouristViewModel
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        public Tourist Tourist { get; set; }
        public TouristRepository TouristRepository { get; set; }

        private bool showTooltips;

        public bool ShowTooltips
        {
            get { return showTooltips; }
            set 
            { 
                showTooltips = value; 
                OnPropertyChanged(nameof(ShowTooltips)); 
            }
        }

        public SettingsViewModel(User loggedInUser)
        {
            TouristRepository = new TouristRepository();
            Tourist = TouristRepository.GetByUserId(loggedInUser.Id);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
