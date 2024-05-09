using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Aplication.UseCases
{
    public class RenovationService
    {
        private readonly IRenovationRepository _renovationRepository;
      
        public RenovationService(IRenovationRepository _renovationRepository)
        {
            //_renovationRepository = new RenovationRepository();
            this._renovationRepository = _renovationRepository;
        }
        public Renovation Save(Renovation newRenovation)
        {
            return _renovationRepository.Save(newRenovation);
        }
        public Renovation AddRenovation(Renovation renovation)
        {
            return _renovationRepository.AddRenovation(renovation);
        }

        public void UpdateRenovation(Renovation updatedRenovation)
        {
            _renovationRepository.UpdateRenovation(updatedRenovation);
        }

        public void DeleteRenovation(int renovationId)
        {
            _renovationRepository.DeleteRenovation(renovationId);
        }

        public List<Renovation> GetAllRenovations()
        {
            return _renovationRepository.GetAllRenovations();
        }

        public Renovation GetRenovationById(int renovationId)
        {
            return _renovationRepository.GetRenovationById(renovationId);
        }

        public void SaveChanges()
        {
            _renovationRepository.SaveChanges();
        }

        public int GetRenovationId(Renovation renovation)
        {
            return _renovationRepository.GetRenovationId(renovation);
        }
    }
}
