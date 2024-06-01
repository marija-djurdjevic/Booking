using BookingApp.Aplication.UseCases;
using BookingApp.Aplication;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using BookingApp.WPF.Views.GuestView;

namespace BookingApp.WPF.ViewModels.GuestViewModels
{
    public class ForumViewModel
    {
        public Guest LoggedInGuest { get; set; }
        public string SelectedLocation { get; set; }
        public string Comment { get; set; }
        public ObservableCollection<string> CitiesCountries { get; set; } = new ObservableCollection<string>();

        public ForumService forumService;

        public ForumViewModel(Guest guest)
        {
            LoggedInGuest = guest;
            forumService = new ForumService(Injector.CreateInstance<IForumRepository>(), Injector.CreateInstance<IGuestRepository>(), Injector.CreateInstance<IForumCommentRepository>());
            CitiesCountries = new ObservableCollection<string>();
            forumService.LoadCitiesCountries(CitiesCountries);
        }

        public ForumList PostForum()
        {
            Forum NewForum = new Forum();
            Location Location = new Location();
            NewForum.GuestId = LoggedInGuest.Id;
            NewForum.Comment = Comment;
            string[] locationParts = SelectedLocation.Split(',');
            string city = locationParts[0].Trim();
            string country = locationParts[1].Trim();
            Location.Country = country;
            Location.City = city;
            NewForum.Location = Location;
            forumService.AddNewForum(NewForum);
            return new ForumList(LoggedInGuest);
        }
    }
}
