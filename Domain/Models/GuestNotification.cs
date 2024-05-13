using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Serializer;


namespace BookingApp.Domain.Models
{
    public class GuestNotification : ISerializable
    {
        public int Id { get; set; }
        public int GuestId { get; set; }
        public string Message {  get; set; }
        public bool Read {  get; set; }
        public GuestNotification()
        {

        }
        public GuestNotification(int id, int guestId, string message, bool read)
        {
            Id = id;
            GuestId = guestId;
            Message = message;
            Read = read;
        }

        public GuestNotification(int guestId, string message, bool read)
        {
            GuestId = guestId;
            Message = message;
            Read = read;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), GuestId.ToString(), Message, Read.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            GuestId = Convert.ToInt32(values[1]);
            Message = values[2];
            Read = Convert.ToBoolean(values[3]);
        }
    }
}
