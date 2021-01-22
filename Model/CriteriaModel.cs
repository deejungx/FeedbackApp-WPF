
using CsvHelper.Configuration.Attributes;

namespace RestaurantFeedbackApp.Model
{
    public class CriteriaModel: ObservableObject
    {
        private string _title;
        private string _description;
        private bool _isSelected;

        [Index(0)]
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

        [Index(1)]
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

        [Index(2)]
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (value == _isSelected) return;
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

    }
}
