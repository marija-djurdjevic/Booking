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

    

        public int TourId { get; set; }
        public string Name { get; set; }
        public KeyPointType Type { get; set; }
        public int OrdinalNumber { get; set; }
        public bool IsChecked { get; set; }

        public KeyPoint() { }

        public KeyPoint(int tourId, string keyName, KeyPointType keyType, int ordinalNumber, bool isChecked)
        {
            TourId = tourId;
            Name = keyName;
            Type = keyType;
            OrdinalNumber = ordinalNumber;
            IsChecked = isChecked;
        }

        public void FromCSV(string[] values)
        {
            TourId = Convert.ToInt32(values[0]);
            Name = values[1];
            Type = (KeyPointType)Enum.Parse(typeof(KeyPointType), values[2]);
            OrdinalNumber = Convert.ToInt32(values[3]);
            IsChecked = Convert.ToBoolean(values[4]);
        }

        public string[] ToCSV()
        {
            string[] csvValues = { TourId.ToString(), Name, Type.ToString(), OrdinalNumber.ToString(), IsChecked.ToString() };
            return csvValues;
        }
    }
}
