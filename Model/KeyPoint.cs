using BookingApp.Model.Enums;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Xml.Linq;

namespace BookingApp.Model
{
    public class KeyPoint : ISerializable
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public string Name { get; set; }
        public KeyPointType Type { get; set; }
        public int OrdinalNumber { get; set; }
        public bool IsChecked { get; set; }

        public KeyPoint() { }

        public KeyPoint(int id,int tourId, string keyName, KeyPointType keyType, int ordinalNumber, bool isChecked)
        {
            Id = id;
            TourId = tourId;
            Name = keyName;
            Type = keyType;
            OrdinalNumber = ordinalNumber;
            IsChecked = isChecked;
        }

        public KeyPoint( int tourId, string keyName, KeyPointType keyType, int ordinalNumber, bool isChecked)
        {
            
            TourId = tourId;
            Name = keyName;
            Type = keyType;
            OrdinalNumber = ordinalNumber;
            IsChecked = isChecked;
        }




        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TourId = Convert.ToInt32(values[1]);
            Name = values[2];
            Type = (KeyPointType)Enum.Parse(typeof(KeyPointType), values[3]);
            OrdinalNumber = Convert.ToInt32(values[4]);
            IsChecked = Convert.ToBoolean(values[5]);
        }

        public string[] ToCSV()
        {
            string[] csvValues = {Id.ToString(), TourId.ToString(), Name, Type.ToString(), OrdinalNumber.ToString(), IsChecked.ToString() };
            return csvValues;
        }
    }
}
