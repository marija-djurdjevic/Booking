using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.UseCases
{
    public class GuideService
    {
        private readonly IGuideRepository _guideRepository;

        public GuideService(IGuideRepository guideRepository)
        {
            _guideRepository = guideRepository;
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

    }
}
