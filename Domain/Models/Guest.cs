using BookingApp.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Models
{
    public class Guest : User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsSuperGuest {  get; set; }
        public int Points { get; set; }
        public DateTime SuperGuestStartDate {  get; set; }

        public Guest() { }

        public Guest(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
        public Guest(string username, string password, string firstName, string lastName, bool isSuperGuest, int points, DateTime startDateSuperGuest)
        {
            Id = Id;
            Username = username;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            IsSuperGuest = isSuperGuest;
            Points = points;
            SuperGuestStartDate = startDateSuperGuest;
        }
        public override string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), FirstName, LastName, IsSuperGuest.ToString(), Points.ToString(), SuperGuestStartDate.ToString() };
            return csvValues;
        }

        public override void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            FirstName = values[1];
            LastName = values[2];
            IsSuperGuest = Convert.ToBoolean(values[3]);
            Points = Convert.ToInt32(values[4]);    
            SuperGuestStartDate = Convert.ToDateTime(values[5]);
        }
    }
}
