using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repositories
{
    public class GuestNotificationsRepository : IGuestNotificationRepository
    {
        private const string FilePath = "../../../Resources/Data/guestsnotifications.csv";

        private readonly Serializer<GuestNotification> _serializer;

        private List<GuestNotification> guestsnotifications;

        public GuestNotificationsRepository()
        {
            _serializer = new Serializer<GuestNotification>();

            if (!System.IO.File.Exists(FilePath))
            {
                using (System.IO.File.Create(FilePath)) { }
            }

            guestsnotifications = _serializer.FromCSV(FilePath);
        }

        public GuestNotification AddNotification(GuestNotification notification)
        {
            int nextId = NextId();
            notification.Id = nextId;
            guestsnotifications.Add(notification);
            _serializer.ToCSV(FilePath, guestsnotifications);
            return notification;
        }

        public void Save(GuestNotification notification)
        {
            guestsnotifications = GetAll();
            guestsnotifications.Add(notification);
            _serializer.ToCSV(FilePath, guestsnotifications);
        }

        public List<GuestNotification> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public void Update(GuestNotification updatedguestnotification)
        {
            GuestNotification existingGuestNotification = guestsnotifications.FirstOrDefault(t => t.Id == updatedguestnotification.Id);
            if (existingGuestNotification != null)
            {
                int index = guestsnotifications.IndexOf(existingGuestNotification);
                guestsnotifications[index] = updatedguestnotification;
                _serializer.ToCSV(FilePath, guestsnotifications);
            }
        }

        public int NextId()
        {
            if (guestsnotifications.Count < 1)
            {
                return 1;
            }
            return guestsnotifications.Max(t => t.Id) + 1;
        }
    }
}
