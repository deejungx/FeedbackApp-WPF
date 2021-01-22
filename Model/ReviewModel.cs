
namespace RestaurantFeedbackApp.Model
{
    public class ReviewModel: ObservableObject
    {
        private string _criteria;
        private int _rating;

        public string Criteria
        {
            get => _criteria;
            set
            {
                _criteria = value;
                OnPropertyChanged("Criteria");
            }
        }

        public int Rating
        {
            get => _rating;
            set
            {
                _rating = value;
                OnPropertyChanged("Rating");
            }
        }
    }
}
