using BookingApp.Domain.Models.Enums;
using BookingApp.View.GuideView;
using BookingApp.WPF.Views.GuideView;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Models
{
    public class ComplexTourRequest : BookingApp.Serializer.ISerializable
    {
        public int Id { get; set; }
        public int TouristId { get; set; }
        public TourRequestStatus Status { get; set; }
        public List<TourRequest> TourRequests { get; set; }
        public ComplexTourRequest()
        {
            TourRequests = new List<TourRequest>();
            Status = TourRequestStatus.Pending;
        }

        public ComplexTourRequest(int id, int touristId, TourRequestStatus status)
        {
            Id = id;
            TouristId = touristId;
            Status = status;
            TourRequests = new List<TourRequest>();
        }

        public ComplexTourRequest(int id, int touristId, TourRequestStatus status, List<TourRequest> tourRequests)
        {
            Id = id;
            TouristId = touristId;
            Status = TourRequestStatus.Pending;
            TourRequests = tourRequests;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), TouristId.ToString(), Status.ToString()};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TouristId = Convert.ToInt32(values[1]);
            Status = (TourRequestStatus)Enum.Parse(typeof(TourRequestStatus), values[2]);
        }
    }
}
