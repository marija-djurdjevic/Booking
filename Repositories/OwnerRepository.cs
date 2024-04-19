using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;

namespace BookingApp.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private const string FilePath = "../../../Resources/Data/owners.csv";

        private readonly Serializer<Owner> _serializer;

        private List<Owner> owners;
        private OwnerReviewRepository ownerReviewRepository;

        public OwnerRepository()
        {
            ownerReviewRepository = new OwnerReviewRepository();
            _serializer = new Serializer<Owner>();

            if (!System.IO.File.Exists(FilePath))
            {
                using (System.IO.File.Create(FilePath)) { }
            }

            owners = _serializer.FromCSV(FilePath);
        }

        public void Save(Owner owner)
        {
            owners = GetAll();
            owners.Add(owner);
            _serializer.ToCSV(FilePath, owners);
        }

        public List<Owner> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public Owner GetByUserId(int Id)
        {
            owners = _serializer.FromCSV(FilePath);
            return owners.Find(t => t.Id == Id);
        }

        public void UpdateOwner(Owner updatedOwner)
        {
            Owner existingOwner = owners.FirstOrDefault(o => o.Id == updatedOwner.Id);
            if (existingOwner != null)
            {
                int index = owners.IndexOf(existingOwner);
                owners[index] = updatedOwner;
                Save();
            }
        }
        public void Save()
        {
            _serializer.ToCSV(FilePath, owners);
        }


        /*public void UpdateOwnerPropertiesBasedOnReviews()
        {
            List<Owner> allOwners = GetAll();
            foreach (Owner owner in allOwners)
            {
               double averageRating = ownerReviewRepository.CalculateAverageRating(owner.Id);
               owner.OwnerAverage = averageRating;

               List<OwnerReview> ownerReviews = ownerReviewRepository.GetReviewsByOwnerId(owner.Id);
                if (ownerReviews.Count >= 50)
                {
                   if (averageRating > 4.5)
                    {
                        owner.IsSuperOwner = true;
                    }
                    else
                    {
                       owner.IsSuperOwner = false;
                    }
                }
                else
                {
                    owner.IsSuperOwner = false;
                }
                UpdateOwner(owner);
            }

        }*/
    }
}
