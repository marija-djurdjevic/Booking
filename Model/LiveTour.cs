using BookingApp.Model.Enums;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace BookingApp.Model
{
    public class LiveTour: ISerializable
    {
        public int TourId { get; set; }
        public List<KeyPoints> KeyPoints { get; set; }
        public bool IsLive {  get; set; }

        public LiveTour() { }

        public LiveTour(int tourId, List<KeyPoints> keyPoints,bool isLive)
        {
            TourId = tourId;
            KeyPoints= keyPoints;
            IsLive = isLive;
        }

        public string[] ToCSV()
        {
            List<string> csvValues = new List<string>();
            csvValues.Add(TourId.ToString());
            csvValues.Add(IsLive.ToString());
            csvValues.AddRange(KeyPoints.Select(kp => $"{kp.TourId}|{kp.KeyName}|{kp.KeyType}|{kp.OrdinalNumber}|{kp.IsChecked}"));

            return csvValues.ToArray();
        }

        public void FromCSV(string[] values)
        {
            TourId = int.Parse(values[0]);
            IsLive = bool.Parse(values[1]);

            KeyPoints = new List<KeyPoints>();
            for (int i = 2; i < values.Length; i += 5) // Povećajemo i za 5 jer svaki red ima 5 polja
            {
                KeyPoints.Add(new KeyPoints
                {
                    TourId = int.Parse(values[i]),
                    KeyName = values[i + 1],
                    KeyType = (KeyPoint)Enum.Parse(typeof(KeyPoint), values[i + 2]),
                    OrdinalNumber = int.Parse(values[i + 3]),
                    IsChecked = bool.Parse(values[i + 4])
                });
            }
        }

    }
}

///treba cuvati ovu klasu u nekom posebno csv,nju kreira guid.cs kada pokrene turu i ima m guide ima metodu koju ce kad se =cekira da promjeni vrijednost svake tacke
///isChecked i stavlja da je tura live i svaki put kad nesto promjeni cuva u livetour.csv