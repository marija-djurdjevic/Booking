using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Aplication
{
    public class Injector
    {
        private static Dictionary<Type, object> _implementations = new Dictionary<Type, object>
        {
        { typeof(ITourRepository), new TourRepository() },
        { typeof(IPropertyRepository), new PropertyRepository() },
        { typeof(IOwnerRepository), new OwnerRepository() },
        { typeof(IPropertyReservationRepository), new PropertyReservationRepository() },
        { typeof(ITouristRepository), new TouristRepository() },
        { typeof(IGlobalLanguagesRepository), new GlobalLanguagesRepository() },
        { typeof(IGlobalLocationsRepository), new GlobalLocationsRepository() },
        { typeof(IKeyPointRepository), new KeyPointRepository() },
        { typeof(ILiveTourRepository), new LiveTourRepository() },
        { typeof(ITouristExperienceRepository), new TouristExperienceRepository() },
        { typeof(ITouristGuideNotificationRepository), new TouristGuideNotificationRepository() },
        { typeof(ITourReservationRepository), new TourReservationRepository() },
        { typeof(IUserRepository), new UserRepository() },
        { typeof(IVoucherRepository), new VoucherRepository() },
        { typeof(ITourRequestRepository), new TourRequestRepository() },
        { typeof(IOwnerReviewRepository), new OwnerReviewRepository() },
        { typeof(IReservedDateRepository), new ReservedDateRepository() },
        { typeof(IReservationChangeRequestRepository), new ReservationChangeRequestsRepository() },
        // Add more implementations here
    };

        public static T CreateInstance<T>()
        {
            Type type = typeof(T);

            if (_implementations.ContainsKey(type))
            {
                return (T)_implementations[type];
            }

            throw new ArgumentException($"No implementation found for type {type}");
        }
    }
}
