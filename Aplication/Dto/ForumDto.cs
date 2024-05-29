using BookingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookingApp.Aplication.Dto
{
    public class ForumDto : INotifyPropertyChanged
    {
        private int guestId;
        private LocationDto locationDto = new LocationDto();
        private string comment;
        private string city;
        private string country;
        private int guestsComments;
        private int ownersComments;
        private bool isClosed;


        public int GuestsComments
        {

            get { return guestsComments; }
            set
            {
                if (value != guestsComments)
                {
                    guestsComments = value;
                    OnPropertyChanged();
                }
            }
        }

        public int OwnersComments
        {

            get { return ownersComments; }
            set
            {
                if (value != ownersComments)
                {
                    ownersComments = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsClosed
        {

            get { return isClosed; }
            set
            {
                if (value != isClosed)
                {
                    isClosed = value;
                    OnPropertyChanged();
                }
            }
        }

        public int GuestId
        {

            get { return guestId; }
            set
            {
                if (value != guestId)
                {
                    guestId = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Comment
        {

            get { return comment; }
            set
            {
                if (value != comment)
                {
                    comment = value;
                    OnPropertyChanged();
                }
            }
        }

        public LocationDto LocationDto
        {
            get { return locationDto; }
            set
            {
                if (value != locationDto)
                {
                    locationDto = value;
                    city = locationDto.City;
                    country = locationDto.Country;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(City));
                    OnPropertyChanged(nameof(Country));
                }
            }
        }

        public string City
        {
            get { return city; }
            set
            {
                if (value != city)
                {
                    city = value;
                    locationDto.City = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Country
        {
            get { return country; }
            set
            {
                if (value != country)
                {
                    country = value;
                    locationDto.Country = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Id { get; set; }

        public ForumDto()
        {
        }

        public ForumDto(int guestId, LocationDto locationDto, string comment, int guestsComments, bool isClosed, int ownersComments)
        {
            this.guestId = guestId;
            this.comment = comment;
            this.locationDto = locationDto;
            city = locationDto.City;
            country = locationDto.Country;
            this.GuestsComments = guestsComments;
            this.isClosed = isClosed;
            OwnersComments = ownersComments;
        }

        public ForumDto(Forum forum)
        {
            guestId = forum.GuestId;
            comment = forum.Comment;
            locationDto = new LocationDto(forum.Location);
            city = forum.Location.City;
            country = forum.Location.Country;
            guestsComments = forum.GuestsComments;
            isClosed = forum.IsClosed;
            ownersComments = forum.OwnersComments;

        }

        public Forum ToForum()
        {
            Location location = LocationDto != null ? locationDto.ToLocation() : new Location();
            return new Forum(guestId, location, comment, guestsComments, isClosed, ownersComments);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
