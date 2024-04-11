using BookingApp.DTO;
using BookingApp.GuideView;
using BookingApp.Model;
using BookingApp.Repository;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace BookingApp.View
{
    public class TourReviewViewModel : BaseViewModel
    {
        private readonly TouristRepository touristRepository;
        private readonly TouristExperienceRepository touristExperienceRepository;
        private readonly TourReservationRepository tourReservationRepository;
        private int tourId;

        public ObservableCollection<TouristExperience> TouristExperiences { get; private set; }

        public TourReviewViewModel(int tourId)
        {
            this.tourId = tourId;
            touristRepository = new TouristRepository();
            touristExperienceRepository = new TouristExperienceRepository();
            tourReservationRepository = new TourReservationRepository();

            TouristExperiences = new ObservableCollection<TouristExperience>();

            LoadTouristExperiences();
        }

        private void LoadTouristExperiences()
        {
            var touristExperiences = touristExperienceRepository.GetTouristExperiencesForTour(tourId);

            foreach (var experience in touristExperiences)
            {
                var tourist = touristRepository.GetByUserId(experience.TouristId);
                experience.Tourist = tourist;

                var reservations = tourReservationRepository.GetByTourId(tourId);
                var reservation = reservations.FirstOrDefault(r => r.UserId == experience.TouristId);
                if (reservation != null)
                {
                    experience.ArrivalKeyPoint = reservation.JoinedKeyPoint;
                }
            }

            TouristExperiences = new ObservableCollection<TouristExperience>(touristExperiences);
            OnPropertyChanged(nameof(TouristExperiences));
        }

        public void RefreshList()
        {
            OnPropertyChanged(nameof(TouristExperiences));
        }

    }
}
