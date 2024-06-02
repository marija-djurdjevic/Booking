using BookingApp.Aplication.UseCases;
using BookingApp.Aplication;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Repositories;
using BookingApp.WPF.Views.GuestView;
using System.ComponentModel;
using System.Windows.Input;
using BookingApp.Command;
using System.Windows.Controls;
using System.Windows;

namespace BookingApp.WPF.ViewModels.GuestViewModels
{
    public class ForumListViewModel : INotifyPropertyChanged
    {
        public ICommand CloseForumCommand { get; }
        public Guest LoggedInGuest { get; set; }
        public List<Forum> Forums { get; set; }
        public List<Guest> Guests { get; set; }
        public ObservableCollection<KeyValuePair<Forum, Guest>> ForumGuests { get; set; }

        private KeyValuePair<Forum, Guest> selectedForum;
        public KeyValuePair<Forum, Guest> SelectedForum
        {
            get => selectedForum;
            set
            {
                if (selectedForum.Key != value.Key || selectedForum.Value != value.Value)
                {
                    selectedForum = value;
                    OnPropertyChanged(nameof(SelectedForum));
                }
            }
        }

        public ForumService forumService;

        public ForumListViewModel(Guest guest)
        {
            ForumGuests = new ObservableCollection<KeyValuePair<Forum, Guest>>();
            CloseForumCommand = new SimpleRelayCommand(CloseForum, CanCloseForum);
            SelectedForum = new KeyValuePair<Forum, Guest>();
            forumService = new ForumService(Injector.CreateInstance<IForumRepository>(), Injector.CreateInstance<IGuestRepository>(), Injector.CreateInstance<IForumCommentRepository>());
            Forums = forumService.GetAllForums();
            Guests = forumService.GetAllGuests();
            LoggedInGuest = guest;
            MakeForumGuestsPairs();
        }

        private bool CanCloseForum(object parameter)
        {
            if (parameter is KeyValuePair<Forum, Guest> forumGuest)
            {
                return forumGuest.Key.GuestId == LoggedInGuest.Id;
            }
            return false;
        }

        private void CloseForum(object parameter)
        {
            if (parameter is KeyValuePair<Forum, Guest> forumGuest)
            {
                if (forumGuest.Key.IsClosed == false)
                {
                    forumGuest.Key.IsClosed = true;
                    forumService.UpdateForum(forumGuest.Key);
                    MessageBox.Show("Succesfully closed forum!");
                }
                else MessageBox.Show("This forum is already closed!");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public OpenNewForum OpenForum()
        {
            return new OpenNewForum(LoggedInGuest);
        }

        public void MakeForumGuestsPairs()
        {
            forumService.MakeForumGuestsPairs(Forums, Guests, ForumGuests);
        }

        public ForumCommenting ForumCommentingView()
        {
            return new ForumCommenting(LoggedInGuest, SelectedForum);
        }

        public void GetAllForums()
        {
            ForumGuests.Clear();
            forumService.MakeForumGuestsPairs(Forums, Guests, ForumGuests);
        }

        public void GetMyForums()
        {
            ForumGuests.Clear();
            forumService.MakeMyForumGuestsPairs(Forums, LoggedInGuest, ForumGuests);
        }
    }
}
