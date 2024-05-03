using BookingApp.Aplication;
using BookingApp.Aplication.UseCases;
using BookingApp.Command;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using BookingApp.WPF.Views.TouristView;
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

        private TouristService touristService;
        public RelayCommand CloseCommand { get; set; }
        public RelayCommand HelpCommand { get; set; }
        public RelayCommand WizardCommand { get; set; }
        public RelayCommand ShowTooltipsCommand { get; set; }

        public SettingsViewModel(User loggedInUser)
        {
            touristService = new TouristService(Injector.CreateInstance<ITouristRepository>());
            Tourist = touristService.GetByUserId(loggedInUser.Id);
            CloseCommand = new RelayCommand(CloseWindow);
            HelpCommand = new RelayCommand(Help);
            WizardCommand = new RelayCommand(WizardExecuteCommand);
            ShowTooltipsCommand = new RelayCommand(ShowTooltipsExecuteCommand);
        }

        private void WizardExecuteCommand(object obj)
        {
            new TouristWizardMainWindow(Tourist).ShowDialog();
        }

        private void ShowTooltipsExecuteCommand()
        {
            Tourist.ShowTooltips = ((App)Application.Current).globalVariables.ShowTooltips;
            touristService.Update(Tourist);
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
