using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BookingApp.Controls
{
    public partial class StarRatingControl : UserControl
    {
        public static readonly DependencyProperty RatingProperty =
            DependencyProperty.Register("Rating", typeof(int), typeof(StarRatingControl), new PropertyMetadata(0, OnRatingChanged));

        public int Rating
        {
            get { return (int)GetValue(RatingProperty); }
            set { SetValue(RatingProperty, value); }
        }

        public ObservableCollection<int> Stars { get; set; } = new ObservableCollection<int> { 1, 2, 3, 4, 5 };

        public StarRatingControl()
        {
            InitializeComponent();
            StarItemsControl.ItemsSource = Stars;
            UpdateStars();
        }

        private static void OnRatingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is StarRatingControl control)
            {
                control.UpdateStars();
            }
        }

        private void StarButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is int star)
            {
                Rating = star;
            }
        }

        private void UpdateStars()
        {
            foreach (var item in StarItemsControl.Items)
            {
                if (StarItemsControl.ItemContainerGenerator.ContainerFromItem(item) is ContentPresenter contentPresenter &&
                    contentPresenter.ContentTemplate.FindName("PART_StarButton", contentPresenter) is Button button)
                {
                    button.Foreground = (int)item <= Rating ? Brushes.Gold : Brushes.Gray;
                }
            }
        }
    }
}
