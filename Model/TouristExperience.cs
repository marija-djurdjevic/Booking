using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using BookingApp.Serializer;

namespace BookingApp.Model
{
    public class TouristExperience : ISerializable
    {
        public int Id { get; set; }
        public int TouristId { get; set; }
        public int TourId { get; set; }
        public int TourInterestingesRating { get; set; }
        public int GuideKnowledgeRating { get; set; }
        public int GuideLanguageRating { get; set; }
        public string Comment { get; set; }

        public string CommentStatus {  get; set; }
        public List<string> ImagesPaths { get; set; }

        public TouristExperience()
        {
            ImagesPaths = new List<string>();
            CommentStatus = "Valid";
        }

        public TouristExperience(int id, int touristId, int tourId, int tourInterestingesRating, int guideKnowledgeRating, int guideLanguageRating, string comment,string commentStatus ,List<string> imagesPaths)
        {
            Id = id;
            TouristId = touristId;
            TourId = tourId;
            TourInterestingesRating = tourInterestingesRating;
            GuideKnowledgeRating = guideKnowledgeRating;
            GuideLanguageRating = guideLanguageRating;
            Comment = comment;
            CommentStatus = "Valid";
            ImagesPaths = imagesPaths;
        }


        public Tourist Tourist { get; set; }

        public KeyPoint ArrivalKeyPoint { get; set; }


        public string[] ToCSV()
        {

            string imagesPathsStr = string.Join("|", ImagesPaths);
            string[] csvValues = { Id.ToString(), TouristId.ToString(), TourId.ToString(), TourInterestingesRating.ToString(), GuideLanguageRating.ToString(),GuideKnowledgeRating.ToString(),Comment,CommentStatus, imagesPathsStr };
            return csvValues;

        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TouristId = Convert.ToInt32(values[1]);
            TourId = Convert.ToInt32(values[2]);
            TourInterestingesRating = Convert.ToInt32(values[3]);
            GuideLanguageRating = Convert.ToInt32(values[4]);
            GuideKnowledgeRating = Convert.ToInt32(values[5]);
            Comment = values[6];
            CommentStatus = values[7];

            for (int i = 8; i < values.Length; i++)
            {
                ImagesPaths.Add(values[i]);
            }
        }
    }
}
