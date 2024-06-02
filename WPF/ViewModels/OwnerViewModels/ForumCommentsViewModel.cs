using BookingApp.Aplication;
using BookingApp.Aplication.Dto;
using BookingApp.Aplication.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    class ForumCommentsViewModel : INotifyPropertyChanged
    {
       
        private readonly ForumService _forumService;
        
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private User _loggedInUser;
        public User LoggedInUser
        {
            get { return _loggedInUser; }
            set
            {
                _loggedInUser = value;
                OnPropertyChanged(nameof(LoggedInUser));
            }
        }

        private ForumDto _selectedForum;
        public ForumDto SelectedForum
        {
            get { return _selectedForum; }
            set
            {
                _selectedForum = value;
                OnPropertyChanged(nameof(SelectedForum));
            }
        }

        private ObservableCollection<ForumComment> _forumComments;
        public ObservableCollection<ForumComment> ForumComments
        {
            get { return _forumComments; }
            set
            {
                _forumComments = value;
                OnPropertyChanged(nameof(ForumComments));
            }
        }

        private string _newComment;
        public string NewComment
        {
            get { return _newComment; }
            set
            {
                _newComment = value;
                OnPropertyChanged(nameof(NewComment));
            }
        }

        private int reportsCount;
        public  int ReportsCount
        {
            get { return reportsCount; }
            set
            {
                reportsCount = value;
                OnPropertyChanged(nameof(ReportsCount));
            }
        }

        public ForumCommentsViewModel(User loggedInUser, ForumDto selectedForum)
        {
            LoggedInUser = loggedInUser;
            SelectedForum = selectedForum;
            _forumService = new ForumService(Injector.CreateInstance<IForumRepository>(), Injector.CreateInstance<IGuestRepository>(), Injector.CreateInstance<IForumCommentRepository>());
            LoadForumComments();
        }
        private void LoadForumComments()
        {
           ForumComments = new ObservableCollection<ForumComment>(_forumService.GetCommentsForForum(SelectedForum.Id));
        }
        public void AddComment()
        {
            if (!string.IsNullOrWhiteSpace(NewComment))
            {
                var newForumComment = new ForumComment
                {
                    UserId = 0,
                    ForumId = SelectedForum.Id,
                    Comment = NewComment,
                    AuthorId = SelectedForum.GuestId,
                    GuestVisited = true
                };

                SelectedForum.Comments++;
                SelectedForum.OwnersComments++;
                _forumService.UpdateForum(SelectedForum.ToForum());
               
                _forumService.SendComment(newForumComment);
                ForumComments.Add(newForumComment);

                NewComment = string.Empty;
            }
        }

        public void ReportComment(ForumComment comment)
        {
            if (!comment.GuestVisited)
            {
                comment.ReportsCount++;
                _forumService.UpdateComment(comment);
                MessageBox.Show("Comment reported!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            else
            {
                MessageBox.Show("Guest has visited the location and cannot be reported.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
