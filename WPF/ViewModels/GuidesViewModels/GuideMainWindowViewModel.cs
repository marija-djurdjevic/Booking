using BookingApp.Command;
using BookingApp.Domain.Models;
using BookingApp.View;
using BookingApp.View.GuideView;
using BookingApp.WPF.ViewModels.GuidesViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModels.GuidesViewModels
{
    public class GuideMainWindowViewModel:BaseViewModel
    {
        private RelayCommand goBackCommand; 
        private RelayCommand goHomeCommand;
        public User LoggedInUser { get; set; }
        public GuideMainWindowViewModel(User loggedInUser)
        {
            LoggedInUser = loggedInUser;
            goBackCommand = new RelayCommand(ExecuteGoBack);
            goHomeCommand = new RelayCommand(ExecuteGoHome);
        }


        public RelayCommand GoBackCommand
        {
            get { return goBackCommand; }
            set
            {
                if (goBackCommand != value)
                {
                    goBackCommand = value;
                    OnPropertyChanged();
                }
            }
        }


        public RelayCommand GoHomeCommand
        {
            get { return goHomeCommand; }
            set
            {
                if (goHomeCommand != value)
                {
                    goHomeCommand = value;
                    OnPropertyChanged();
                }
            }
        }



        private void ExecuteGoBack()
        {
            if (GuideMainWindow.MainFrame.CanGoBack)
            {
                GuideMainWindow.MainFrame.GoBack();
            }
        }


        private void ExecuteGoHome()
        {
            var guideMainPage = new GuideMainPage(LoggedInUser);
            GuideMainWindow.MainFrame.Navigate(guideMainPage);

        }




    }


    


}
