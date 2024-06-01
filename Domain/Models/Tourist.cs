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
        public List<DateOnly> VisitedToursDates { get; set; }

        public Tourist()
        {
            VisitedToursDates = new List<DateOnly>();
        }

        public Tourist(string firstName, string lastName, int age)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            ShowTooltips = true;
            ShowWizard = true;
            VisitedToursDates = new List<DateOnly>();
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
            VisitedToursDates = new List<DateOnly>();
        }
        public override string[] ToCSV()
        {
            List<string> dates = new List<string>();
            dates.AddRange(VisitedToursDates.Select(t => t.ToString("dd.MM.yyyy")));
            string strDates = string.Join("|", dates);
            string[] csvValues = { Id.ToString(), FirstName, LastName, Age.ToString(), ShowTooltips.ToString(), ShowWizard.ToString(), strDates };
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
            VisitedToursDates = new List<DateOnly>();
            for (int i = 6; i < values.Length; i++)
            {
                if (values[i] != "")
                    VisitedToursDates.Add(DateOnly.ParseExact(values[i], "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture));
            }
        }
    }
}
