using BookingApp.Domain.Models;
using BookingApp.Serializer;
using BookingApp.WPF.Views.GuestView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IGuestNotificationRepository
    {
        GuestNotification AddNotification(GuestNotification notification);
        void Save(GuestNotification notification);
        List<GuestNotification> GetAll();
        void Update(GuestNotification updatedguestnotification);
        int NextId();
       
    }
}
