using System.ComponentModel;

namespace RestaurantFeedbackApp.Model
{
    public class CriteriaModel: ObservableObject
    {
        private string _title;
        private string _description;

        public string Title
        {
            get => _title;
            set
            {
                if (value == _title) return;
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (value == _description) return;
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

    }
}
