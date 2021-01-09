using System.Collections.Generic;
using System.ComponentModel;

namespace RestaurantFeedbackApp.Model
{
    // ObservableObject implements INotifyPropertyChanged
    public class FeedbackModel : ObservableObject
    {
        private string _name;
        private string _email;
        private string _phone;

        // Not sure if this is the right way to do it.
        // I Don't know how to initialize dictionary keys to criterias.
        private Dictionary<string, int> _ratings;

        public string Name
        {
            get => _name;
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (value == _email) return;
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                if (value == _phone) return;
                _phone = value;
                OnPropertyChanged(nameof(Phone));
            }
        }

        public Dictionary<string, int> Ratings
        {
            get => _ratings;
            set
            {
                if (value == _ratings) return;
                _ratings = value;
                OnPropertyChanged(nameof(Ratings));
            }
        }


    }
}
