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

namespace BookingApp.WPF.ViewModels.GuestViewModels
{
    public class ForumListViewModel
    {
        public Guest LoggedInGuest { get; set; }
        public List<Forum> Forums { get; set; }
        public List<Guest> Guests { get; set; }
        public ObservableCollection<KeyValuePair<Forum, Guest>> ForumGuests { get; set; }
        public KeyValuePair<Forum, Guest> SelectedForum {  get; set; }

        public ForumService forumService;

        public ForumListViewModel(Guest guest)
        {
            ForumGuests = new ObservableCollection<KeyValuePair<Forum, Guest>>();
            SelectedForum = new KeyValuePair<Forum, Guest>();
            forumService = new ForumService(Injector.CreateInstance<IForumRepository>(), Injector.CreateInstance<IGuestRepository>(), Injector.CreateInstance<IForumCommentRepository>());
            Forums = forumService.GetAllForums();
            Guests = forumService.GetAllGuests();
            LoggedInGuest = guest;
            MakeForumGuestsPairs();
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

    }
}
