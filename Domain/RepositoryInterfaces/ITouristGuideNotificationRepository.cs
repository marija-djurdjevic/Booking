using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface ITouristGuideNotificationRepository
    {
        List<TouristGuideNotification> GetAll();
        void Save(TouristGuideNotification touristGuideNotification);
        int NextId();
        void Delete(int id);
        void Update(TouristGuideNotification updatedNotification);
        TouristGuideNotification GetById(int id);
    }
}
