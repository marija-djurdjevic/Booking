using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public enum RequestStatus { Approved, Declined, Processing };
    public class ReservationChangeRequest : ISerializable
    {
        public int Id { get; set; }
        
        public int ReservationId {  get; set; }
        public DateTime OldStartDate {  get; set; }
        public DateTime OldEndDate { get; set; }
        public DateTime NewStartDate {  get; set; }
        public DateTime NewEndDate { get; set; }
        public RequestStatus RequestStatus { get; set; }
        public int GuestId {  get; set; }
        public string PropertyName {  get; set; }
        public string Comment { get; set; }


        public ReservationChangeRequest() {
            RequestStatus = RequestStatus.Processing;
        }

        public ReservationChangeRequest(int id)
        {
            Id = id;
        }

        public ReservationChangeRequest(int id, int reservationId, DateTime oldStartDate, DateTime oldEndDate, DateTime newStartDate, DateTime newEndDate, RequestStatus status, int guestId, string propertyName, string comment)
        {
            Id = id;
            ReservationId = reservationId;
            OldStartDate = oldStartDate;
            OldEndDate = oldEndDate;
            NewStartDate = newStartDate;
            NewEndDate = newEndDate;
            RequestStatus = status;
            GuestId = guestId;
            PropertyName = propertyName;
            Comment = comment;
        }

        public ReservationChangeRequest(int reservationId, DateTime oldStartDate, DateTime oldEndDate, DateTime newStartDate, DateTime newEndDate, RequestStatus status, int guestId, string propertyName, string comment)
        {
            ReservationId = reservationId;
            OldStartDate = oldStartDate;
            OldEndDate = oldEndDate;
            NewStartDate = newStartDate;
            NewEndDate = newEndDate;
            RequestStatus = status;
            GuestId = guestId;
            PropertyName = propertyName;
            Comment = comment;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), ReservationId.ToString(), OldStartDate.ToString(), OldEndDate.ToString(), NewStartDate.ToString(), NewEndDate.ToString(), RequestStatus.ToString(), GuestId.ToString(), PropertyName, Comment};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            ReservationId = Convert.ToInt32(values[1]);
            OldStartDate = DateTime.Parse(values[2]);
            OldEndDate = DateTime.Parse(values[3]);
            NewStartDate = DateTime.Parse(values[4]);
            NewEndDate = DateTime.Parse(values[5]);
            RequestStatus = (RequestStatus)Enum.Parse(typeof(RequestStatus), values[6]);
            GuestId = Convert.ToInt32(values[7]);
            PropertyName = values[8];
            Comment = values[9];
        }
    }
}
