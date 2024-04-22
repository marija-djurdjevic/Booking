using BookingApp.Aplication;
using BookingApp.Aplication.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels.TouristViewModels
{
    public class SettingsViewModel : BindableBase
    {
        public Tourist Tourist { get; set; }
        public TouristService TouristService { get; set; }

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
            TouristService = new TouristService(Injector.CreateInstance<ITouristRepository>());
            Tourist = TouristService.GetByUserId(loggedInUser.Id);
        }
    }
}
