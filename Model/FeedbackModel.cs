// ReSharper disable ConvertToAutoProperty

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Schema;
using CsvHelper.Configuration.Attributes;

namespace RestaurantFeedbackApp.Model
{
    public class FeedbackModel : ObservableObject
    {

        private string _name;
        private string _email;
        private string _phone;
        private ObservableCollection<ReviewModel> _reviews = new ObservableCollection<ReviewModel>();
        private DateTime _submitDate;
        private double _avgRating;

        [Index(0)]
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        [Index(1)]
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged("Email");
            }
        }

        [Index(2)]
        public string Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                OnPropertyChanged("Phone");
            }
        }

        [Index(3)]
        public DateTime SubmitDate
        {
            get => _submitDate;
            set
            {
                _submitDate = value;
                OnPropertyChanged("SubmitDate");
            }
        }

        public ObservableCollection<ReviewModel> Reviews
        {
            get => _reviews;
            set
            {
                _reviews = value;
                OnPropertyChanged("Reviews");
            }
        }

        public double AverageRating
        {
            get => _avgRating;
            set
            {
                _avgRating = value;
                OnPropertyChanged("AverageRating");
            }
        }

    }
}
