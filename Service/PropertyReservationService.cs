using BookingApp.GuestView;
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
    public class PropertyReservationService
    {
        public PropertyRepository PropertyRepository;
        public PropertyReservationRepository PropertyReservationRepository;
        public ReservedDateRepository ReservedDateRepository;

        public PropertyReservationService()
        {
            PropertyRepository = new PropertyRepository();
            ReservedDateRepository = new ReservedDateRepository();
            PropertyReservationRepository = new PropertyReservationRepository();
        }

        public ObservableCollection<PropertyReservation> GetGuestReservations(int guestId)
        {
           return new ObservableCollection<PropertyReservation>(PropertyReservationRepository.GetAll().FindAll(r => r.GuestId == guestId && r.Canceled == false));
        }

        public List<PropertyReservation> UpdateGuestReservations(int guestId)
        {
            return PropertyReservationRepository.GetAll().FindAll(r => r.GuestId == guestId && r.Canceled == false);
        }
        public bool CanCancelReservation(PropertyReservation SelectedReservation)
        {
            Property SelectedProperty = PropertyRepository.GetPropertyById(SelectedReservation.PropertyId);
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
            PropertyReservationRepository.Update(SelectedReservation);
            ReservedDateRepository.Delete(SelectedReservation.Id);
        }

        public Property GetPropertyByReservation(PropertyReservation SelectedReservation)
        {
            return PropertyRepository.GetPropertyById(SelectedReservation.PropertyId);
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
    }
}
