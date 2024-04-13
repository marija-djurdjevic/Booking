using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Repository;


namespace BookingApp.Repository
{
    public class OwnerRepository
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
        /*public void UpdateOwnerPropertiesBasedOnReviews()
        {
            foreach (Owner owner in owners)
            {
                // Dobij ocjene vlasnika
                List<OwnerReview> ownerReviews = ownerReviewRepository.GetReviewsByOwnerId(owner.Id);

                // Provjeri da li vlasnik ima dovoljan broj ocjena
                if (ownerReviews.Count >= 50)
                {
                    // Izračunaj prosječnu ocjenu vlasnika
                    double averageRating = CalculateAverageRating(ownerReviews);

                    // Ažuriraj prosječnu ocjenu vlasnika
                    owner.AverageRating = averageRating;

                    // Provjeri da li je prosječna ocjena veća od 4.5
                    if (averageRating > 4.5)
                    {
                        // Postavi svojstvo IsSuperOwner na true
                        owner.IsSuperOwner = true;
                    }
                    else
                    {
                        // Ako prosječna ocjena nije veća od 4.5, postavi IsSuperOwner na false
                        owner.IsSuperOwner = false;
                    }

                    // Ažuriraj vlasnika u repozitorijumu
                    UpdateOwner(owner);
                }
            }
        }*/

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


        public void UpdateOwnerPropertiesBasedOnReviews()
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

        }
    }
}
