using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class DateRangeRepository
    {
        private const string FilePath = "../../../Resources/Data/dateranges.csv";

        private readonly Serializer<DateRange> _serializer;

        private List<DateRange> dateRanges;

        public DateRangeRepository()
        {
            _serializer = new Serializer<DateRange>();

            if (!System.IO.File.Exists(FilePath))
            {
                using (System.IO.File.Create(FilePath)) { }
            }

            dateRanges = _serializer.FromCSV(FilePath);
        }

        public void AddDateRange(DateRange _dateRange)
        {
            dateRanges.Add(_dateRange);
            _serializer.ToCSV(FilePath, dateRanges);
        }

        public List<DateRange> GetAllDateRanges()
        {
            return dateRanges;
        }

        public List<DateRange> GetDateRangesByPropertyId(int propertyId)
        {
            return dateRanges.FindAll(t => t.PropertyId == propertyId);
        }


        private void SaveChanges()
        {
            _serializer.ToCSV(FilePath, dateRanges);
        }
    }
}
