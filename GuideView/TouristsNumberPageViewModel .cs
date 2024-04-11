using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using BookingApp.Model;
using BookingApp.Repository;

namespace BookingApp.GuideView
{
    internal class TouristsNumberPageViewModel : BaseViewModel
    {
        private int tourId;
        private int under18Count;
        private int between18And50Count;
        private int over50Count;
        private readonly TouristRepository touristRepository;
        private readonly TouristExperienceRepository touristExperienceRepository;
       

        public TouristsNumberPageViewModel(int tourId)
        {
            this.tourId = tourId;
            TouristExperienceRepository touristExperienceRepository = new TouristExperienceRepository();
            TouristRepository touristRepository = new TouristRepository();
            CountTouristsByAge();
        }

        public int Under18Count
        {
            get { return under18Count; }
            set
            {
                if (under18Count != value)
                {
                    under18Count = value;
                    OnPropertyChanged(nameof(Under18Count));
                }
            }
        }

        public int Between18And50Count
        {
            get { return between18And50Count; }
            set
            {
                if (between18And50Count != value)
                {
                    between18And50Count = value;
                    OnPropertyChanged(nameof(Between18And50Count));
                }
            }
        }

        public int Over50Count
        {
            get { return over50Count; }
            set
            {
                if (over50Count != value)
                {
                    over50Count = value;
                    OnPropertyChanged(nameof(Over50Count));
                }
            }
        }

        private void CountTouristsByAge()
        {
            TouristRepository touristRepository = new TouristRepository();
            var touristIds = new TouristExperienceRepository().GetTouristIdsByTourId(tourId);

            foreach (var touristId in touristIds)
            {
                int age = touristRepository.GetAgeById(touristId);
                if (age < 18)
                {
                    Under18Count++;
                }
                else if (age >= 18 && age <= 50)
                {
                    Between18And50Count++;
                }
                else
                {
                    Over50Count++;
                }
            }
        }
    }
}
