using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Service
{
    public class CreateTourService
    {
        private readonly TourRepository tourRepository;
        private readonly KeyPointService keyPointService;
        

        public CreateTourService()
        {
            tourRepository = new TourRepository();
            keyPointService = new KeyPointService();
        }

        private int counter=0;
        public bool CreateTour(TourDto tourDto, ObservableCollection<string> keyPointNames,DateTime tourDate)
        {

            Tour tour = new Tour
            {
                Name = tourDto.Name,
                Description = tourDto.Description,
                Language = tourDto.Language,
                MaxTouristsNumber = tourDto.MaxTouristNumber,
                Duration = tourDto.Duration,
                StartDateTime = tourDate,
                Location = new Location
                {
                    City = tourDto.LocationDto.City,
                    Country = tourDto.LocationDto.Country
                },
                ImagesPaths = tourDto.ImagesPaths.ToList()
            };

            
            tourRepository.Save(tour);
            keyPointService.SetKeyPoints(tour.Id, keyPointNames);

            return true;
        }
    }
}
