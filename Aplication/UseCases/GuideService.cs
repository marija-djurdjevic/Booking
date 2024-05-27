using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.UseCases
{
    public class GuideService
    {
        private readonly IGuideRepository _guideRepository;
        private readonly UserRepository userRepository;
        public GuideService(IGuideRepository guideRepository)
        {
            _guideRepository = guideRepository;
            userRepository= new UserRepository();   
        }

        public IEnumerable<Guide> GetSuperGuides()
        {
            var guides = _guideRepository.GetAll();
            return guides.Where(g => g.IsSuperGuide);
        }

        public IEnumerable<Guide> GetGuidesByLanguage(string language)
        {
            var guides = _guideRepository.GetAll();
            return guides.Where(g => g.Language == language);
        }

        public IEnumerable<Guide> GetGuidesByAverageRating(double rating)
        {
            var guides = _guideRepository.GetAll();
            return guides.Where(g => g.AverageRatingLastYear >= rating);
        }

        public IEnumerable<Guide> GetGuidesByToursCount(int toursCount)
        {
            var guides = _guideRepository.GetAll();
            return guides.Where(g => g.ToursLastYear >= toursCount);
        }

        public bool IsSuperGuideById(int guideId)
        {
            var guide = _guideRepository.GetById(guideId);
            return guide != null && guide.IsSuperGuide;
        }


        public void RemoveGuide(int guideId)
        {
           
             _guideRepository.Delete(guideId);
             userRepository.Delete(guideId);
        }


        public void setStatus(int guideId,bool status)
        {
            var guide = _guideRepository.GetById(guideId);
            guide.IsSuperGuide = status;
            _guideRepository.Update(guide);
        }

    }
}
