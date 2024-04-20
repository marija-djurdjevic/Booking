using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface ILiveTourRepository
    {
        List<LiveTour> GetAll();
        void Save(LiveTour liveTour);
        void Delete(int id);
        void Update(LiveTour updatedliveTour);
        LiveTour GetById(int id);
        public void SaveChanges();
        public List<LiveTour> GetAllLiveTours();
        public List<LiveTour> GetFinishedTours();
    }
}
