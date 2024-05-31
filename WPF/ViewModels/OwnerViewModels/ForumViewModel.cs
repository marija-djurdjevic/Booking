using BookingApp.Aplication;
using BookingApp.Aplication.Dto;
using BookingApp.Aplication.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    public class ForumViewModel
    {
        private readonly User _loggedInUser;
        private ForumRepository _forumRepository;
        private GuestRepository _guestRepository;

        private readonly ForumService _forumService;

        public ObservableCollection<ForumDto> AllForums { get; set; }

        public ForumViewModel(User loggedInUser) 
        {
            _loggedInUser = loggedInUser;
            _forumRepository = new ForumRepository();
            _guestRepository = new GuestRepository();
            _forumService = new ForumService(Injector.CreateInstance<IForumRepository>(), Injector.CreateInstance<IGuestRepository>(), Injector.CreateInstance<IForumCommentRepository>());
            AllForums = new ObservableCollection<ForumDto>();

            LoadForums(loggedInUser);

           
        }
        /*private void LoadForums()
        {
            var forums = _forumRepository.GetAll();
            var forumDtos = forums.Select(f => new ForumDto
            {
                Username = GetGuestUsername(f.GuestId),
                Comment = f.Comment,
                Location = $"{f.Location.City}, {f.Location.Country}",
                GuestComments = f.GuestsComments,
                OwnersComments = f.OwnersComments,
                IsClosed = f.IsClosed
            }).ToList();

            foreach (var forumDto in forumDtos)
            {
                Forums.Add(forumDto);
            }
        }*/
        /* private void LoadForums()
         {
             var forumService = new ForumService(); // Zamijenite ForumService() sa odgovarajućim servisom ili repozitorijumom
             var allForums = forumService.GetAllForums(); // Metoda koja vraća sve forume

             Forums = new ObservableCollection<ForumDto>();
             foreach (var forum in allForums)
             {
                 var forumDto = new ForumDto(forum);
                 Forums.Add(forumDto);
             }
         }*/
        private void LoadForums(User loggedInUser)
        {
            var allForums = _forumService.GetAllForums();

            foreach (var forum in allForums)
            {
                var forumDto = new ForumDto(forum)
                {
                    Username = _forumService.GetUsernameForGuestId(forum.GuestId),
                    //CommentsCount = forum.GuestsComments + forum.OwnersComments,
                    City = _forumService.GetCityForForum(forum),
                    Country = _forumService.GetCountryForForum(forum)
                };

                AllForums.Add(forumDto);
            }
        }
    }
}
