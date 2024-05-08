using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repositories
{
    public class RenovationReccomendationRepository : IRenovationReccomendationRepository
    {
        private const string FilePath = "../../../Resources/Data/renovationReccomendations.csv";

        private readonly Serializer<RenovationReccomendation> _serializer;

        private List<RenovationReccomendation> renovationReccomendations;

        public RenovationReccomendationRepository()
        {
            _serializer = new Serializer<RenovationReccomendation>();

            if (!System.IO.File.Exists(FilePath))
            {
                using (System.IO.File.Create(FilePath)) { }
            }

            renovationReccomendations = _serializer.FromCSV(FilePath);
        }

        public void AddRenovationReccomendation(RenovationReccomendation _renovationReccomendation)
        {
            int nextId = NextId();
            _renovationReccomendation.Id = nextId;
            renovationReccomendations.Add(_renovationReccomendation);
            _serializer.ToCSV(FilePath, renovationReccomendations);
        }

        public List<RenovationReccomendation> GetAll()
        {
            return renovationReccomendations;
        }

        public List<RenovationReccomendation> GetRenovationRequestDataById(int id)
        {
            return renovationReccomendations.FindAll(t => t.Id == id);
        }


        public int NextId()
        {
            if (renovationReccomendations.Count < 1)
            {
                return 1;
            }
            return renovationReccomendations.Max(t => t.Id) + 1;
        }

    }
}
