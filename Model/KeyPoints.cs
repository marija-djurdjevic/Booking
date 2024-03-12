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
    public class KeyPoints : ISerializable
    {
        public int TourId { get; set; }
        public string KeyName { get; set; }
        public KeyPoint KeyType { get; set; }
        public int OrdinalNumber { get; set; }
        public bool IsChecked { get; set; }

        public KeyPoints() { }

        public KeyPoints(int tourId, string keyName, KeyPoint keyType, int ordinalNumber, bool isChecked)
        {
            TourId = tourId;
            KeyName = keyName;
            KeyType = keyType;
            OrdinalNumber = ordinalNumber;
            IsChecked = isChecked;
        }

        public void FromCSV(string[] values)
        {
            TourId = Convert.ToInt32(values[0]);
            KeyName = values[1];
            KeyType = (KeyPoint)Enum.Parse(typeof(KeyPoint), values[2]);
            OrdinalNumber = Convert.ToInt32(values[3]);
            IsChecked = Convert.ToBoolean(values[4]);
        }

        public string[] ToCSV()
        {
            string[] csvValues = { TourId.ToString(), KeyName, KeyType.ToString(), OrdinalNumber.ToString(), IsChecked.ToString() };
            return csvValues;
        }
    }
}
