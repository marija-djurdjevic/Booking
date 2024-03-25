using BookingApp.Model.Enums;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class Voucher : ISerializable
    {
        public int Id { get; set; }
        public int TouristId { get; set; }
        public string Reason { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string VoucherNumber { get; set; }

        public Voucher() { }

        public Voucher(int id, int tourist_id, string reason, DateTime expirationDate)
        {
            Id = id;
            TouristId = tourist_id;
            Reason = reason;
            ExpirationDate = expirationDate;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), TouristId.ToString(), Reason, ExpirationDate.ToString("dd.MM.yyyy HH:mm:ss") };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TouristId = Convert.ToInt32(values[1]);
            Reason = values[2];
            ExpirationDate = DateTime.ParseExact(values[3], "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
