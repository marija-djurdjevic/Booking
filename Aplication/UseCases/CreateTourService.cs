using BookingApp.Aplication.Dto;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Aplication.UseCases
{
    public class CreateTourService
    {
        private readonly ITourRepository tourRepository;
        private readonly KeyPointService keyPointService;
        private IKeyPointRepository keyPointRepository;
        private ILiveTourRepository liveTourRepository;

        public CreateTourService(ITourRepository tourRepository)
        {
            this.tourRepository = tourRepository;
            keyPointRepository = Injector.CreateInstance<IKeyPointRepository>();
            liveTourRepository = Injector.CreateInstance<ILiveTourRepository>();
            keyPointService = new KeyPointService(keyPointRepository,liveTourRepository);
        }

        private int counter = 0;
        public bool CreateTour(TourDto tourDto, ObservableCollection<string> keyPointNames, DateTime tourDate)
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
