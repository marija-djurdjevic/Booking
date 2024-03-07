using BookingApp.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class KeyPoints
    {
        public KeyPoint Type { get; set; }
        public string Name { get; set; }

        public KeyPoints() { }
        public KeyPoints(KeyPoint type, string name)
        {
            Type = type;
            Name = name;
        }



    }
}