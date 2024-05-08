using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Klasa se koristi za testiranje(validacuju) unsesnih podataka
 */

namespace BookingApp.WPF.Validations
{
    public class Verifications
    {
        public int TryParseInt(string s)                ///Parsiranje int broja
        {
            int i;
            if (int.TryParse(s, out i))
            {
                return i;
            }
            else
            {
                do
                {
                    Console.WriteLine("Failed to parse int type, enter int: ");
                    s = Console.ReadLine();
                } while (!int.TryParse(s, out i));
                return i;
            }
        }

        public string checkString(string str)               ///Provjera da li je string prazan
        {
            while (str == string.Empty)
            {
                Console.WriteLine("Empty string is not valid, try again:");
                str = Console.ReadLine();
            }
            return str;
        }
    }
}
