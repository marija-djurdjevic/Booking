using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class GlobalLanguagesRepository
    {
        private const string FilePath = "../../../Resources/Data/globalLanguages.csv";

        private List<string> languages;

        public GlobalLanguagesRepository()
        {
            languages = new List<string>();
            using (StreamReader sr = new StreamReader(FilePath))
            {
                while (!sr.EndOfStream)
                {
                    string language = sr.ReadLine();

                    languages.Add(language);

                }
            }
        }

        public List<string> GetAll()
        {
            return languages;
        }

    }
}
