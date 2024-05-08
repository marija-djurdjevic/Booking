using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModels.OwnerViewModels
{
    public class RenovationsViewModel
    {
        public User LoggedInUser { get; set; }
        public RenovationsViewModel(User LoggedInUser) {
            this.LoggedInUser = LoggedInUser; 
        }
    }
}
