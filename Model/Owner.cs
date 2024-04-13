using BookingApp.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class Owner : User
    {
        public double OwnerAverage { get; set; }
        public bool IsSuperOwner { get; set; }
        public Owner() { }
        Owner(double ownerAverage, bool isSuperOwner)
        {
            OwnerAverage = ownerAverage;
            IsSuperOwner = isSuperOwner;
        }
        public Owner(string username, string password, double ownerAverage, bool isSuperOwner, UserRole role)
        {
            Id = Id;
            Username = username;
            Password = password;
            Role = role;
            OwnerAverage = ownerAverage;
            IsSuperOwner = isSuperOwner;
        }
        public override string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), OwnerAverage.ToString(), IsSuperOwner.ToString() };
            return csvValues;
        }

        public override void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);

            double ownerAverage;

            if (double.TryParse(values[1], out ownerAverage))
            {
                OwnerAverage = ownerAverage;
            }
            bool isSuperOwner;
            if(IsSuperOwner = bool.TryParse(values[2], out isSuperOwner))
            {
                IsSuperOwner = isSuperOwner;
            }
        }
    }
}
