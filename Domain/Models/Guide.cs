using BookingApp.Domain.Models.Enums;
using BookingApp.Serializer;
using System;
using System.Linq;

namespace BookingApp.Domain.Models
{
    public class Guide : User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsSuperGuide { get; set; }
        public string Language { get; set; }
        public int ToursLastYear { get; set; }   //broj tura prosle godine 
        public double AverageRatingLastYear { get; set; }

        public Guide() { }

        public Guide(int id, string username, string password, UserRole role, string firstName, string lastName, string language, int toursLastYear, double averageRatingLastYear)
            : base(id, username, password, role)
        {
            FirstName = firstName;
            LastName = lastName;
            Language = language;
            ToursLastYear = toursLastYear;
            AverageRatingLastYear = averageRatingLastYear;
            UpdateSuperGuideStatus();
        }

        public void UpdateSuperGuideStatus()
        {
            IsSuperGuide = ToursLastYear >= 20 && AverageRatingLastYear > 4.0;
        }

        public override string[] ToCSV()
        {
            string[] baseCsv = base.ToCSV();
            string[] guideCsv = { FirstName, LastName, IsSuperGuide.ToString(), Language, ToursLastYear.ToString(), AverageRatingLastYear.ToString() };
            return baseCsv.Concat(guideCsv).ToArray();
        }

        public override void FromCSV(string[] values)
        {
            base.FromCSV(values.Take(4).ToArray());
            FirstName = values[4];
            LastName = values[5];
            IsSuperGuide = Convert.ToBoolean(values[6]);
            Language = values[7];
            ToursLastYear = Convert.ToInt32(values[8]);
            AverageRatingLastYear = Convert.ToDouble(values[9]);
          
        }
    }
}
