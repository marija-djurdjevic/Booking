using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class TouristGuideNotificationRepository
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

        public void Save(TouristGuideNotification touristGuideNotification)
        {
            touristGuideNotifications = GetAll();
            touristGuideNotifications.Add(touristGuideNotification);
            _serializer.ToCSV(FilePath, touristGuideNotifications);
        }

        public List<TouristGuideNotification> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public List<TouristGuideNotification> GetByUserId(int Id)
        {
            touristGuideNotifications = _serializer.FromCSV(FilePath);
            return touristGuideNotifications.FindAll(t => t.TouristId == Id);
        }
    }
}
