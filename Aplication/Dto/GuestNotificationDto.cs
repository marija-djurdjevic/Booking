using BookingApp.Domain.Models;
using BookingApp.WPF.Views.GuestView;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Printing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Aplication.Dto
{
    public class GuestNotificationDto : INotifyPropertyChanged
    {
        private int Id;
        private int guestId;
        private string message;
        private bool read;

        public GuestNotificationDto() { }

        public GuestNotificationDto(int guestId, string messange, bool read)
        {
            this.guestId = guestId;
            this.message = messange;
            this.read = read;
        }

        public GuestNotificationDto(GuestNotification notification)
        {
            guestId = notification.GuestId;
            message = notification.Message;
            read = notification.Read;
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

        public string Message
        {

            get { return message; }
            set
            {
                if (value != message)
                {
                    message = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool Read
        {
            get { return read; }
            set
            {
                if (value != read)
                {
                    read = value;
                    OnPropertyChanged();
                }
            }
        }
    
        public GuestNotification ToGuestNotification()
        {
            return new GuestNotification(GuestId, Message, Read);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }

}
