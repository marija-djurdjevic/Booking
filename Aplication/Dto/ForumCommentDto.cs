using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Aplication.Dto
{
    public class ForumCommentDto : INotifyPropertyChanged
    {
        private int guestId;
        private int forumId;
        private string comment;
        private int authorId;
        private bool guestVisited;
        private int reportsCount;

        public int ReportsCount
        {

            get { return reportsCount; }
            set
            {
                if (value != reportsCount)
                {
                    reportsCount = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool GuestVisited
        {

            get { return guestVisited; }
            set
            {
                if (value != guestVisited)
                {
                    guestVisited = value;
                    OnPropertyChanged();
                }
            }
        }
        public int GuestId
        {

            get { return guestId; }
            set
            {
                if (value != guestId)
                {
                    guestId = value;
                    OnPropertyChanged();
                }
            }
        }

        public int AuthorId
        {

            get { return authorId; }
            set
            {
                if (value != authorId)
                {
                    authorId = value;
                    OnPropertyChanged();
                }
            }
        }

        public int ForumId
        {

            get { return forumId; }
            set
            {
                if (value != forumId)
                {
                    forumId = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Comment
        {

            get { return comment; }
            set
            {
                if (value != comment)
                {
                    comment = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Id { get; set; }

        public ForumCommentDto()
        {
        }

        public ForumCommentDto(int guestId, int forumId, string comment, int authorId, bool guestVisited)
        {
            this.guestId = guestId;
            this.guestId = forumId;
            this.comment = comment;
            this.authorId = authorId;
            this.guestVisited = guestVisited;
        }

        public ForumCommentDto(ForumComment forumcomment)
        {
            guestId = forumcomment.GuestId;
            forumId = forumcomment.ForumId;
            comment = forumcomment.Comment;
            authorId = forumcomment.AuthorId;
            guestVisited = forumcomment.GuestVisited;
            reportsCount = forumcomment.ReportsCount;
        }

        public ForumComment ToForumComment()
        {
            return new ForumComment(guestId, forumId, comment, authorId, guestVisited);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
