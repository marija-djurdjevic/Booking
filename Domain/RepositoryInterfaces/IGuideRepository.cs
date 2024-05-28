using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IGuideRepository
    {
       
        void Add(Guide guide);
        void Update(Guide guide);
        void Delete(int guideId);
        Guide GetById(int guideId);
        IEnumerable<Guide> GetAll();

       

       
    }
}
