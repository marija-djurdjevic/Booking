using BookingApp.Domain.Models;
using BookingApp.Repositories;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.RepositoryInterfaces;

namespace BookingApp.Aplication.UseCases
{
    public class TouristGuideNotificationService
    {
        private ITouristGuideNotificationRepository notificationRepository; 

        public TouristGuideNotificationService(ITouristGuideNotificationRepository touristGuideNotificationRepository)
        {
            notificationRepository = touristGuideNotificationRepository;
        }

        public void MarkAllUserMessagesAsRead(int userId)
        {
            var userNotifications = GetByUserId(userId);
            foreach (var notification in userNotifications)
            {
                notification.Seen = true;
                notificationRepository.Update(notification);
            }
        }

        public List<TouristGuideNotification> GetByUserId(int Id)
        {
            var touristGuideNotifications = notificationRepository.GetAll();
            return touristGuideNotifications.FindAll(t => t.TouristId == Id).OrderByDescending(x => x.CreationTime).ToList();
        }

        public int GetUnreadNotificationCount(int id)
        {
            return (int)GetByUserId(id).FindAll(t=>!t.Seen).Count();
        }

        internal void Save(TouristGuideNotification touristGuideNotification)
        {
            notificationRepository.Save(touristGuideNotification);
        }
    }
}
