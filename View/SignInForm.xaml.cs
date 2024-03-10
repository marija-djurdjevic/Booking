using BookingApp.Model;
using BookingApp.Model.Enums;
using BookingApp.Repository;
using BookingApp.Tourist;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using BookingApp.Tourist;

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for SignInForm.xaml
    /// </summary>
    public partial class SignInForm : Window
    {

        private readonly UserRepository _repository;

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                if (value != _username)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SignInForm()
        {
            InitializeComponent();
            DataContext = this;
            _repository = new UserRepository();
        }

        private void SignIn(object sender, RoutedEventArgs e)
        {
            User user = _repository.GetByUsername(Username);
            if (user != null)
            {
                if (user.Password == txtPassword.Password)
                {
                    switch (user.Role)
                    {
                        case UserRole.Default:
                            {
                                CommentsOverview commentsOverview = new CommentsOverview(user);
                                commentsOverview.Show();
                                Close();
                                break;
                            }
                        case UserRole.Owner:
                            {
                                MessageBox.Show("You signed in as Owner User!");
                                break;
                            }
                        case UserRole.Guest:
                            {
                                MessageBox.Show("You signed in as Guest User!");
                                break;
                            }
                        case UserRole.Guide:
                            {
                                MessageBox.Show("You signed in as Guide User!");


                                Guide guide = new Guide();
                                guide.Show();
                                Close();
                                break;
                            }
                        case UserRole.Tourist:
                            {
                                MessageBox.Show("You signed in as Tourist User!");
                                break;
                            }
                    }

                }
                else
                {
                    MessageBox.Show("Wrong password!");
                }
            }
            else
            {
                //MessageBox.Show("Wrong username!");
                TouristMainWindow touristMainWindow = new TouristMainWindow();
                touristMainWindow.Show();
                Close();
            }

        }
    }
}
