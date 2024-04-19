using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Aplication.UseCases
{
    public class GlobalLanguagesService : IGlobalLanguagesRepository
    {
        private IGlobalLanguagesRepository languagesRepository;
        public GlobalLanguagesService(IGlobalLanguagesRepository globalLanguagesRepository)
        {
            languagesRepository = globalLanguagesRepository;
        }
        public List<string> GetAll()
        {
            return languagesRepository.GetAll();
        }
    }
}
