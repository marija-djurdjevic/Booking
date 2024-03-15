using BookingApp.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class Guest : User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Guest() { }

        public Guest(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
        public Guest(string username, string password, string firstName, string lastName, UserRole role)
        {
            Id = Id;
            Username = username;
            Password = password;
            Role = role;
            FirstName = firstName;
            LastName = lastName;
        }
        public override string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), FirstName, LastName};
            return csvValues;
        }

        public override void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            FirstName = values[1];
            LastName = values[2];
        }
    }
}
