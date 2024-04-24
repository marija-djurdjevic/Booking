using BookingApp.Aplication;
using BookingApp.Aplication.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

        public RelayCommand CloseCommand { get; set; }
        public RelayCommand HelpCommand { get; set; }

        public SettingsViewModel(User loggedInUser)
        {
            TouristService = new TouristService(Injector.CreateInstance<ITouristRepository>());
            Tourist = TouristService.GetByUserId(loggedInUser.Id);
            CloseCommand = new RelayCommand(CloseWindow);
            HelpCommand = new RelayCommand(Help);
        }

        private void Help()
        {

        }
        private void CloseWindow()
        {
            // Slanje poruke za zatvaranje prozora koristeći MVVM Light Messaging
            Messenger.Default.Send(new NotificationMessage("CloseSettingsWindowMessage"));
        }
    }
}
