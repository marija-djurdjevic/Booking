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
        private readonly IGuestRepository guestRepository;
        private readonly IForumCommentRepository forumCommentRepository;

        public ForumService(IForumRepository ForumRepository, IGuestRepository GuestRepository, IForumCommentRepository forumCommentRepository)
        {
            this.forumRepository = ForumRepository;
            this.guestRepository = GuestRepository;
            this.forumCommentRepository = forumCommentRepository;
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

        public List<Guest> GetAllGuests()
        {
            return guestRepository.GetAll();
        }

        public List<Forum> GetAllForums()
        {
            return forumRepository.GetAll();
        }

        public void MakeForumGuestsPairs(List<Forum> Forums, List<Guest> Guests, ObservableCollection<KeyValuePair<Forum, Guest>> ForumGuests)
        {
            foreach (Forum forum in Forums)
            {
                foreach (Guest guest in Guests)
                {
                    if (forum.GuestId == guest.Id)
                    {
                        var forumGuest = new KeyValuePair<Forum, Guest>(forum, guest);
                        ForumGuests.Add(forumGuest);
                    }
                }
            }
        }

        public void MakeMyForumGuestsPairs(List<Forum> forums, Guest loggedInGuest, ObservableCollection<KeyValuePair<Forum, Guest>> forumGuests)
        {
            foreach (Forum forum in forums)
            {
                
                if (forum.GuestId == loggedInGuest.Id)
                {
                    var forumGuest = new KeyValuePair<Forum, Guest>(forum, loggedInGuest);
                    forumGuests.Add(forumGuest);
                }
                
            }
        }

        public ObservableCollection<KeyValuePair<ForumComment, Guest>> MakePairs(KeyValuePair<Forum, Guest> SelectedForum)
        {
            ObservableCollection<KeyValuePair<ForumComment, Guest>> HelpfulVar = new ObservableCollection<KeyValuePair<ForumComment, Guest>>();

            foreach (ForumComment fc in forumCommentRepository.GetAll())
            {
                foreach (Guest g in guestRepository.GetAll())
                {
                    if (fc.GuestId == g.Id && fc.ForumId == SelectedForum.Key.Id)
                    {
                        var forumcommentGuest = new KeyValuePair<ForumComment, Guest>(fc, g);
                        HelpfulVar.Add(forumcommentGuest);
                    }
                }
            }

            return HelpfulVar;
        }

        public void SendComment(ForumComment forumComment)
        {
            forumCommentRepository.AddForumComment(forumComment);
        }

        public void UpdateForum(Forum forum)
        {
            forumRepository.Update(forum);
        }

    }
}
