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

namespace BookingApp.WPF.ViewModels.GuestViewModels
{
    public class ForumCommentingViewModel
    {
        public KeyValuePair<Forum, Guest> SelectedForum { get; set; }
        public ObservableCollection<KeyValuePair<ForumComment, Guest>> SelectedForumComments { get; set; }
        public ForumComment ForumComment { get; set; }
        public Guest LoggedInGuest { get; set; }
        public ForumService forumService;

        public ForumCommentingViewModel(Guest guest, KeyValuePair<Forum, Guest> selectedForum)
        {
            LoggedInGuest = guest;
            forumService = new ForumService(Injector.CreateInstance<IForumRepository>(), Injector.CreateInstance<IGuestRepository>(), Injector.CreateInstance<IForumCommentRepository>());
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

    }
}
