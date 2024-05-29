using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.GuestView;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Aplication.UseCases
{
    public class PropertyReservationService
    {
        private readonly IPropertyRepository propertyRepository;
        private readonly IPropertyReservationRepository propertyReservationRepository;
        private readonly IReservedDateRepository reservedDateRepository;

        public PropertyReservationService(IPropertyRepository propertyRepository, IPropertyReservationRepository propertyReservationRepository, IReservedDateRepository reservedDateRepository)
        {
            this.propertyRepository = propertyRepository;
            this.propertyReservationRepository = propertyReservationRepository;
            this.reservedDateRepository = reservedDateRepository;
        }

        public ObservableCollection<PropertyReservation> GetGuestReservations(int guestId)
        {
            return new ObservableCollection<PropertyReservation>(propertyReservationRepository.GetAll().FindAll(r => r.GuestId == guestId && r.Canceled == false));
        }

        public List<PropertyReservation> UpdateGuestReservations(int guestId)
        {
            return propertyReservationRepository.GetAll().FindAll(r => r.GuestId == guestId && r.Canceled == false);
        }
        public bool CanCancelReservation(PropertyReservation SelectedReservation)
        {
            Property SelectedProperty = propertyRepository.GetPropertyById(SelectedReservation.PropertyId);
            if (DateTime.Now.AddDays(SelectedProperty.CancellationDeadline) <= SelectedReservation.StartDate)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void CancelReservation(PropertyReservation SelectedReservation)
        {
            SelectedReservation.Canceled = true;
            propertyReservationRepository.Update(SelectedReservation);
            reservedDateRepository.Delete(SelectedReservation.Id);
        }

        public Property GetPropertyByReservation(PropertyReservation SelectedReservation)
        {
            return propertyRepository.GetPropertyById(SelectedReservation.PropertyId);
        }

        public bool CanMakeReview(PropertyReservation SelectedReservation)
        {
            if (SelectedReservation.EndDate < DateTime.Now && DateTime.Now <= SelectedReservation.EndDate.AddDays(5))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<string> CheckAvailabilityForAllRequests(IEnumerable<ReservationChangeRequest> requests)
        {
            var allReservations = propertyReservationRepository.GetAll();

            List<string> availabilityStatus = new List<string>();

            foreach (var request in requests)
            {
                int reservationId = request.ReservationId;

                var reservationsForProperty = allReservations.Where(r => r.Id == reservationId).ToList();

                bool isAvailable = CheckAvailability(request.NewStartDate, request.NewEndDate, reservationsForProperty);
                availabilityStatus.Add(isAvailable ? "Free" : "Occupied");
            }

            return availabilityStatus;
        }

        private bool CheckAvailability(DateTime newStartDate, DateTime newEndDate, List<PropertyReservation> reservations)
        {
            foreach (var reservation in reservations)
            {

                if (newStartDate >= reservation.StartDate && newStartDate <= reservation.EndDate)
                {
                    return false;
                }
                if (newEndDate >= reservation.StartDate && newEndDate <= reservation.EndDate)
                {
                    return false;
                }
            }
            return true;
        }
        public void UpdateReservation(ReservationChangeRequest request)
        {
            var reservationsForProperty = propertyReservationRepository.GetReservationDataById(request.ReservationId);

            if (reservationsForProperty != null && reservationsForProperty.Any())
            {

                var updatedReservation = reservationsForProperty.First();
                updatedReservation.StartDate = request.NewStartDate;
                updatedReservation.EndDate = request.NewEndDate;

                propertyReservationRepository.UpdatePropertyReservation(updatedReservation);
            }
            else
            {
                throw new Exception("Reservation not found for ReservationId: " + request.ReservationId);
            }
        }

        public List<PropertyReservation> GetUpcomingReservations(int guestId)
        {
            DateTime today = DateTime.Today;
            return propertyReservationRepository.GetAll().FindAll(r => r.GuestId == guestId && r.Canceled == false && r.StartDate >= today);
        }

        public List<PropertyReservation> GetCompletedReservations(int guestId)
        {
            DateTime today = DateTime.Today;
            return propertyReservationRepository.GetAll().FindAll(r => r.GuestId == guestId && r.Canceled == false && r.EndDate < today);
        }
        public int GetReservationsCountForYear(string propertyName, int year)
        {

            var reservationsForProperty = propertyReservationRepository.GetAll().Where(r => r.PropertyName == propertyName);

            var reservationsForYear = reservationsForProperty.Where(r => r.StartDate.Year == year);

            return reservationsForYear.Count();
        }
        public int GetCanceledReservationsCount(string propertyName, int year)
        {
            var reservationsForProperty = propertyReservationRepository.GetAll().Where(r => r.PropertyName == propertyName);

            var reservationsForYear = reservationsForProperty.Where(r => r.StartDate.Year == year);

            var canceledReservationsForYear = reservationsForYear.Where(r => r.Canceled);

            return canceledReservationsForYear.Count();
        }
        public int GetReservationsCountForMonth(string propertyName, int year, int month)
        {
            var reservationsForProperty = propertyReservationRepository.GetAll().Where(r => r.PropertyName == propertyName);

            var reservationsForYearAndMonth = reservationsForProperty.Where(r => r.StartDate.Year == year && r.StartDate.Month == month);

            return reservationsForYearAndMonth.Count();
        }
        public int GetCanceledReservationsCountForMonth(string propertyName, int year, int month)
        {
            var reservationsForProperty = propertyReservationRepository.GetAll().Where(r => r.PropertyName == propertyName);

            var canceledReservationsForYearAndMonth = reservationsForProperty
                .Where(r => r.StartDate.Year == year && r.StartDate.Month == month && r.Canceled);

            return canceledReservationsForYearAndMonth.Count();
        }
        public int GetMostOccupiedYear(string propertyName)
        {
            Dictionary<int, double> yearlyOccupancy = new Dictionary<int, double>();

           
            var reservationsForProperty = propertyReservationRepository.GetAll().Where(r => r.PropertyName == propertyName);

            foreach (var reservation in reservationsForProperty)
            {
                int year = reservation.StartDate.Year;
                if (!yearlyOccupancy.ContainsKey(year))
                {
                    yearlyOccupancy[year] = 0;
                }

                double occupancyInYear = yearlyOccupancy[year];
                occupancyInYear += (reservation.EndDate - reservation.StartDate).TotalDays;
                yearlyOccupancy[year] = occupancyInYear;
            }

           
            int mostOccupiedYear = yearlyOccupancy.OrderByDescending(x => x.Value).FirstOrDefault().Key;

            return mostOccupiedYear;
        }

        public int GetMostOccupiedMonthInYear(string propertyName, int year)
        {
            Dictionary<int, double> monthlyOccupancy = new Dictionary<int, double>();

          
            var reservationsForPropertyInYear = propertyReservationRepository.GetAll().Where(r => r.PropertyName == propertyName && r.StartDate.Year == year);

            foreach (var reservation in reservationsForPropertyInYear)
            {
                int month = reservation.StartDate.Month;
                if (!monthlyOccupancy.ContainsKey(month))
                {
                    monthlyOccupancy[month] = 0;
                }

                double occupancyInMonth = monthlyOccupancy[month];
                occupancyInMonth += (reservation.EndDate - reservation.StartDate).TotalDays;
                monthlyOccupancy[month] = occupancyInMonth;
            }

          
            int mostOccupiedMonth = monthlyOccupancy.OrderByDescending(x => x.Value).FirstOrDefault().Key;

            return mostOccupiedMonth;
        }
        public List<string> GetMostPopularLocations(int topN)
        {
            var properties = propertyRepository.GetAllProperties();
            var reservationData = propertyReservationRepository.GetAll();

            var locationPopularity = properties
                .GroupJoin(reservationData,
                           property => property.Id,
                           reservation => reservation.PropertyId,
                           (property, reservations) => new
                           {
                               Location = property.Location,
                               ReservationCount = reservations.Count(),
                               TotalReservationDays = reservations.Sum(r => (r.EndDate - r.StartDate).Days),
                               TotalDaysAvailable = reservations.Any() ? (reservations.Max(r => r.EndDate) - reservations.Min(r => r.StartDate)).Days : 1 
                           })
                .GroupBy(x => x.Location)
                .Select(g => new
                {
                    Location = g.Key,
                    TotalReservations = g.Sum(x => x.ReservationCount),
                    AverageOccupancyRate = g.Sum(x => x.TotalReservationDays) / (double)g.Sum(x => x.TotalDaysAvailable)
                })
                .OrderByDescending(x => x.TotalReservations)
                .ThenByDescending(x => x.AverageOccupancyRate)
                .Take(topN)
                .Select(x => x.Location.City + ", " + x.Location.Country + " (Reservations: " + x.TotalReservations + ", Occupancy Rate: " + (x.AverageOccupancyRate * 100).ToString("F2") + "%)")
                .ToList();

            return locationPopularity;
        }


        /*public List<string> GetLeastPopularLocations(int bottomN)
        {
            var properties = propertyRepository.GetAllProperties();
            var reservationData = propertyReservationRepository.GetAll();

            var locationPopularity = properties
                .GroupJoin(reservationData,
                           property => property.Id,
                           reservation => reservation.PropertyId,
                           (property, reservations) => new
                           {
                               Location = property.Location,
                               ReservationCount = reservations.Count(),
                               OccupancyRate = reservations.Any() ?
                                   reservations.Sum(r => (r.EndDate - r.StartDate).Days) / (double)(reservations.Max(r => r.EndDate) - reservations.Min(r => r.StartDate)).Days
                                   : 0
                           })
                .GroupBy(x => x.Location)
                .Select(g => new
                {
                    Location = g.Key,
                    TotalReservations = g.Sum(x => x.ReservationCount),
                    AverageOccupancyRate = g.Average(x => x.OccupancyRate)
                })
                .OrderBy(x => x.TotalReservations)
                .ThenBy(x => x.AverageOccupancyRate)
                .Take(bottomN)
                .Select(x => x.Location.City + ", " + x.Location.Country)
                .ToList();

            return locationPopularity;
        }*/
        public List<string> GetLeastPopularLocations(int bottomN)
        {
            var properties = propertyRepository.GetAllProperties();
            var reservationData = propertyReservationRepository.GetAll();

            var locationPopularity = properties
                .GroupJoin(reservationData,
                           property => property.Id,
                           reservation => reservation.PropertyId,
                           (property, reservations) => new
                           {
                               Location = property.Location,
                               ReservationCount = reservations.Count(),
                               TotalReservationDays = reservations.Sum(r => (r.EndDate - r.StartDate).Days),
                               TotalDaysAvailable = reservations.Any() ? (reservations.Max(r => r.EndDate) - reservations.Min(r => r.StartDate)).Days : 1
                           })
                .GroupBy(x => x.Location)
                .Select(g => new
                {
                    Location = g.Key,
                    TotalReservations = g.Sum(x => x.ReservationCount),
                    AverageOccupancyRate = g.Sum(x => x.TotalReservationDays) / (double)g.Sum(x => x.TotalDaysAvailable)
                })
                .OrderBy(x => x.TotalReservations)
                .ThenBy(x => x.AverageOccupancyRate)
                .Take(bottomN)
                .Select(x => x.Location.City + ", " + x.Location.Country + " (Reservations: " + x.TotalReservations + ", Occupancy Rate: " + (x.AverageOccupancyRate * 100).ToString("F2") + "%)")
                .ToList();

            return locationPopularity;
        }


        public bool CheckIfGuestVisited(Guest guest, Location location)
        {
            foreach (PropertyReservation propertyReservation in propertyReservationRepository.GetAll())
            {
                foreach(Property property in propertyRepository.GetAllProperties()){
                    if(propertyReservation.PropertyId == property.Id && propertyReservation.GuestId == guest.Id && location.City == property.Location.City && location.Country == property.Location.Country)
                    {
                        return true;
                    } 
                }
            }

            return false;
        }
    }
}
