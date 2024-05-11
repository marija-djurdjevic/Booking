using BookingApp.Domain.Models;
using BookingApp.WPF.Validations;
using System.Collections.Generic;
using System.ComponentModel;

namespace BookingApp.WPF.ViewModels.TouristViewModels
{
    public class TouristExperienceViewModel : BindableBase, IDataErrorInfo
    {
        private int id;
        private int touristId;
        private int tourId;
        private int tourInterestingesRating;
        private int guideKnowledgeRating;
        private int guideLanguageRating;
        private string comment;
        private string commentStatus;
        private List<string> imagesPaths;

        public string Error => null;

        public TouristExperienceViewModel()
        {
            ImagesPaths = new List<string>();
            CommentStatus = "Valid";
        }
        //Verifikation
        public Verifications verifications = new Verifications();
        public string this[string columnName]
        {
            get
            {
                if (columnName == "TourInterestingesRating")
                {
                    if (TourInterestingesRating < 1 || TourInterestingesRating > 5)
                        return "You must select grade";
                }
                if (columnName == "GuideKnowledgeRating")
                {
                    if (GuideKnowledgeRating < 1 || GuideKnowledgeRating > 5)
                        return "You must select grade";
                }
                if (columnName == "GuideLanguageRating")
                {
                    if (GuideLanguageRating < 1 || GuideLanguageRating > 5)
                        return "You must select grade";
                }
                return null;
            }
        }

        private readonly string[] _validatedProperties = { "TourInterestingesRating", "GuideKnowledgeRating", "GuideLanguageRating" };

        public bool IsValid
        {
            get
            {
                foreach (var property in _validatedProperties)
                {
                    if (this[property] != null)
                        return false;
                }

                return true;
            }
        }
        //End of verification

        public int Id
        {
            get => id;
            set
            {
                if (value != id)
                {
                    id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }
        public int TouristId
        {
            get => touristId;
            set
            {
                if (value != touristId)
                {
                    touristId = value;
                    OnPropertyChanged(nameof(TouristId));
                }
            }
        }
        public int TourId
        {
            get => tourId;
            set
            {
                if (value != tourId)
                {
                    tourId = value;
                    OnPropertyChanged(nameof(TourId));
                }
            }
        }
        public int TourInterestingesRating
        {
            get => tourInterestingesRating;
            set
            {
                if (value != tourInterestingesRating)
                {
                    tourInterestingesRating = value;
                    OnPropertyChanged(nameof(TourInterestingesRating));
                }
            }
        }
        public int GuideKnowledgeRating
        {
            get => guideKnowledgeRating;
            set
            {
                if (value != guideKnowledgeRating)
                {
                    guideKnowledgeRating = value;
                    OnPropertyChanged(nameof(GuideKnowledgeRating));
                }
            }
        }
        public int GuideLanguageRating
        {
            get => guideLanguageRating;
            set
            {
                if (value != guideLanguageRating)
                {
                    guideLanguageRating = value;
                    OnPropertyChanged(nameof(GuideLanguageRating));
                }
            }
        }
        public string Comment
        {
            get => comment;
            set
            {
                if (value != comment)
                {
                    comment = value;
                    OnPropertyChanged(nameof(Comment));
                }
            }
        }
        public string CommentStatus
        {
            get => commentStatus;
            set
            {
                if (value != commentStatus)
                {
                    commentStatus = value;
                    OnPropertyChanged(nameof(CommentStatus));
                }
            }
        }
        public List<string> ImagesPaths
        {
            get => imagesPaths;
            set
            {
                if (value != imagesPaths)
                {
                    imagesPaths = value;
                    OnPropertyChanged(nameof(ImagesPaths));
                }
            }
        }
        public TouristExperienceViewModel(TouristExperience touristExperience)
        {
            Id = touristExperience.Id;
            TouristId = touristExperience.TouristId;
            TourId = touristExperience.TourId;
            TourInterestingesRating = touristExperience.TourInterestingesRating;
            GuideKnowledgeRating = touristExperience.GuideKnowledgeRating;
            GuideLanguageRating = touristExperience.GuideLanguageRating;
            Comment = touristExperience.Comment;
            CommentStatus = touristExperience.CommentStatus;
            ImagesPaths = touristExperience.ImagesPaths;
        }
        public TouristExperience ToTouristExperience()
        {
            return new TouristExperience(Id, TouristId, TourId, TourInterestingesRating, GuideKnowledgeRating, GuideLanguageRating, Comment, CommentStatus, ImagesPaths);
        }

    }
}
