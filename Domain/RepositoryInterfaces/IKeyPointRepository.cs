using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IKeyPointRepository
    {
        List<KeyPoint> GetAll();
        void Save(KeyPoint keyPoint);
        int NextId();
        void Delete(int id);
        KeyPoint Update(KeyPoint updatedKeyPoint);
        KeyPoint GetById(int id);
        public List<KeyPoint> GetTourKeyPoints(int tourId);
    }
}
