using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Repositories
{
    public class GuideRepository : IGuideRepository
    {
        private const string FilePath = "../../../Resources/Data/guides.csv";
        private readonly Serializer<Guide> _serializer;
        private List<Guide> guides;

        public GuideRepository()
        {
            _serializer = new Serializer<Guide>();

            if (!System.IO.File.Exists(FilePath))
            {
                using (System.IO.File.Create(FilePath)) { }
            }

            guides = _serializer.FromCSV(FilePath);
        }

        public void Add(Guide guide)
        {
            guides = GetAll().ToList();
            guide.Id = NextId();
            guides.Add(guide);
            _serializer.ToCSV(FilePath, guides);
        }

        public void Update(Guide updatedGuide)
        {
            guides = GetAll().ToList();
            Guide existingGuide = guides.FirstOrDefault(t => t.Id == updatedGuide.Id);
            if (existingGuide != null)
            {
                int index = guides.IndexOf(existingGuide);
                guides[index] = updatedGuide;
                _serializer.ToCSV(FilePath, guides);
            }
        }

        public void Delete(int guideId)
        {
            guides = GetAll().ToList();
            Guide guide = guides.FirstOrDefault(g => g.Id == guideId);
            if (guide != null)
            {
                guides.Remove(guide);
                _serializer.ToCSV(FilePath, guides);
            }
        }

        public Guide GetById(int guideId)
        {
            guides = _serializer.FromCSV(FilePath);
            return guides.FirstOrDefault(g => g.Id == guideId);
        }

        public IEnumerable<Guide> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

       

        public void Save(Guide guide)
        {
            guides = GetAll().ToList();
            guide.Id = NextId();
            guides.Add(guide);
            _serializer.ToCSV(FilePath,guides);
        }

        private int NextId()
        {
            guides = _serializer.FromCSV(FilePath);
            if (guides.Count < 1)
            {
                return 1;
            }
            return guides.Max(g => g.Id) + 1;
        }
    }
}
