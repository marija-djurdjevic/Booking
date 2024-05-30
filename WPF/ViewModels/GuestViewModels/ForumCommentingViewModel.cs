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
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BookingApp.WPF.ViewModels.GuestViewModels
{
    public class ForumCommentingViewModel : INotifyPropertyChanged

    {
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

        
        public ObservableCollection<KeyValuePair<ForumComment, Guest>> SelectedForumComments { get; set; }
        public ForumComment ForumComment { get; set; }
        public Guest LoggedInGuest { get; set; }
        public ForumService forumService;
        public PropertyReservationService reservationService;

        public ForumCommentingViewModel(Guest guest, KeyValuePair<Forum, Guest> selectedForum)
        {
            LoggedInGuest = guest;
            forumService = new ForumService(Injector.CreateInstance<IForumRepository>(), Injector.CreateInstance<IGuestRepository>(), Injector.CreateInstance<IForumCommentRepository>());
            reservationService = new PropertyReservationService(Injector.CreateInstance<IPropertyRepository>(), Injector.CreateInstance<IPropertyReservationRepository>(), Injector.CreateInstance<IReservedDateRepository>());
            SelectedForumComments = new ObservableCollection<KeyValuePair<ForumComment, Guest>>();
            ForumComment = new ForumComment();
            SelectedForum = selectedForum;
            SelectedForumComments = MakeForumCommentGuestPairs();
        }
        public ObservableCollection<KeyValuePair<ForumComment, Guest>> MakeForumCommentGuestPairs()
        {
            return forumService.MakePairs(SelectedForum);
        }

        public void Send_Comment()
        {
            ForumComment.GuestId = LoggedInGuest.Id;
            ForumComment.ForumId = SelectedForum.Key.Id;
            ForumComment.AuthorId = SelectedForum.Key.GuestId;
            CheckGuestVisitedStatus();
            SelectedForum.Key.GuestsComments++;
            SelectedForum.Key.Comments++;
            forumService.UpdateForum(SelectedForum.Key);
            OnPropertyChanged(nameof(SelectedForum));
            forumService.SendComment(ForumComment);
            SelectedForumComments.Clear();
            foreach (var item in MakeForumCommentGuestPairs())
            {
                if (!SelectedForumComments.Contains(item))
                {
                    SelectedForumComments.Add(item);
                }
            }
        }

        public void CheckGuestVisitedStatus()
        {
            if (reservationService.CheckIfGuestVisited(LoggedInGuest, SelectedForum.Key.Location))
            {
                ForumComment.GuestVisited = true;
            }

        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
