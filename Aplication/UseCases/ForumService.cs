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
    public class ForumService
    {
        private GlobalLocationsService globalLocationsService;
        private readonly IForumRepository forumRepository;

        public ForumService(IForumRepository ForumRepository)
        {
            this.forumRepository = ForumRepository;
            globalLocationsService = new GlobalLocationsService(Injector.CreateInstance<IGlobalLocationsRepository>());
        }
        public void LoadCitiesCountries(ObservableCollection<string> CitiesCountries)
        {
            List<string> CitiesAndCountries = globalLocationsService.GetPropertiesLocations();
            foreach (var cityAndCountry in CitiesAndCountries)
            {
                CitiesCountries.Add(cityAndCountry);
            }
        }

        public void AddNewForum(Forum NewForum)
        {
            forumRepository.AddForum(NewForum);
        }
    }
}
