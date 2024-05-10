using BookingApp.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Models
{
    public class Tourist : User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public bool ShowTooltips { set; get; }
        public bool ShowWizard { set; get; }
        public int VisitedToursInYear { get; set; }
        public int YearForVisitedTours { get; set; }

        public Tourist() { }

        public Tourist(string firstName, string lastName, int age)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            ShowTooltips = true;
            ShowWizard = true;
            VisitedToursInYear = 0;
            YearForVisitedTours = -1;
        }
        public Tourist(string username, string password, string firstName, string lastName, int age, UserRole role, bool showTooltips)
        {
            Id = Id;
            Username = username;
            Password = password;
            Role = role;
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            ShowTooltips = showTooltips;
            ShowWizard = true;
            VisitedToursInYear = 0;
            YearForVisitedTours = -1;
        }
        public override string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), FirstName, LastName, Age.ToString(),ShowTooltips.ToString(),ShowWizard.ToString(),VisitedToursInYear.ToString(),YearForVisitedTours.ToString() };
            return csvValues;
        }

        public override void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            FirstName = values[1];
            LastName = values[2];
            Age = Convert.ToInt32(values[3]);
            ShowTooltips = Convert.ToBoolean(values[4]);
            ShowWizard = Convert.ToBoolean(values[5]);
            VisitedToursInYear = Convert.ToInt32(values[6]);
            YearForVisitedTours = Convert.ToInt32(values[7]);
        }
    }
}
