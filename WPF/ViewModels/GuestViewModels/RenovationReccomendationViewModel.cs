using BookingApp.Aplication;
using BookingApp.Aplication.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BookingApp.WPF.ViewModels.GuestViewModels
{
    public class RenovationReccomendationViewModel
    {
        public RenovationReccomendation RenovationReccomendation { get; set; }
        public RenovationReccomendationService renovationReccomendationService;

        public RenovationReccomendationViewModel()
        {
            RenovationReccomendation = new RenovationReccomendation();
            renovationReccomendationService = new RenovationReccomendationService(Injector.CreateInstance<IRenovationReccomendationRepository>(), Injector.CreateInstance<IOwnerReviewRepository>());
        }

        public void SaveReccomendation()
        {
            RenovationReccomendation.OwnerReviewId = renovationReccomendationService.GetOwnerReviewId();
            renovationReccomendationService.SaveRenovationReccomendation(RenovationReccomendation);
        }

        public void SetUrgencyLevel(object sender)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton != null)
            {
                if (int.TryParse(radioButton.Content.ToString(), out int value))
                {
                    RenovationReccomendation.UrgencyLevel = value;
                }
                else
                {
                    RenovationReccomendation.UrgencyLevel = 0;
                }
            }
        }
    }
}
