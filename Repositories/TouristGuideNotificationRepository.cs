using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.Repositories
{
    public class TouristGuideNotificationRepository : ITouristGuideNotificationRepository
    {
        private const string FilePath = "../../../Resources/Data/touristGuideNotification.csv";

        private readonly Serializer<TouristGuideNotification> _serializer;

        private List<TouristGuideNotification> touristGuideNotifications;

        public TouristGuideNotificationRepository()
        {
            _serializer = new Serializer<TouristGuideNotification>();

            if (!System.IO.File.Exists(FilePath))
            {
                using (System.IO.File.Create(FilePath)) { }
            }

            touristGuideNotifications = _serializer.FromCSV(FilePath);
        }

        public List<TouristGuideNotification> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public TouristGuideNotification GetById(int notificationId)
        {
            touristGuideNotifications = GetAll();
            return touristGuideNotifications.FirstOrDefault(t => t.Id == notificationId);
        }

        public void Save(TouristGuideNotification touristGuideNotification)
        {
            touristGuideNotifications = GetAll();
            touristGuideNotification.Id = NextId();
            touristGuideNotifications.Add(touristGuideNotification);
            _serializer.ToCSV(FilePath, touristGuideNotifications);
        }

        public void Update(TouristGuideNotification updatedNotification)
        {
            touristGuideNotifications = GetAll();
            TouristGuideNotification existingNotification = touristGuideNotifications.FirstOrDefault(n => n.Id == updatedNotification.Id);
            if (existingNotification != null)
            {
                int index = touristGuideNotifications.IndexOf(existingNotification);
                touristGuideNotifications[index] = updatedNotification;
                _serializer.ToCSV(FilePath, touristGuideNotifications);
            }
        }

        public void Delete(int notificationId)
        {
            touristGuideNotifications = GetAll();
            TouristGuideNotification existingNotification = touristGuideNotifications.FirstOrDefault(t => t.Id == notificationId);
            if (existingNotification != null)
            {
                touristGuideNotifications.Remove(existingNotification);
                _serializer.ToCSV(FilePath, touristGuideNotifications);
            }
        }

        public int NextId()
        {
            touristGuideNotifications = _serializer.FromCSV(FilePath);
            if (touristGuideNotifications.Count < 1)
            {
                return 1;
            }
            return touristGuideNotifications.Max(t => t.Id) + 1;
        }
    }
}
