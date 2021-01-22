using RestaurantFeedbackApp.Model;
using RestaurantFeedbackApp.ViewModel.Commands;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using MaterialDesignThemes.Wpf;

namespace RestaurantFeedbackApp.ViewModel
{
    internal class FeedbackViewModel : ViewModelBase
    {

        private FeedbackModel _feedback;
        public static ObservableCollection<FeedbackModel> FeedbackCollection { get; set; }
        public static ObservableCollection<CriteriaModel> CriteriaCollection { get; set; }
        public RelayCommand<FeedbackModel> SubmitFeedbackCommand { get; }
        public RelayCommand<FeedbackModel> ResetFormCommand { get; }
        public static ISnackbarMessageQueue MySnackbar { get; set; }

        public FeedbackModel Feedback
        {
            get => _feedback;
            set 
            { 
                _feedback = value;
                OnPropertyChanged();
            }
        }

        public FeedbackViewModel()
        {
            Feedback = new FeedbackModel();
            
            foreach (var criteria in CriteriaCollection)
            {
                 Feedback.Reviews.Add(new ReviewModel() { Criteria = criteria.Title, Rating = 0 });
            }

            SubmitFeedbackCommand = new RelayCommand<FeedbackModel>(SubmitFeedback, SubmitFeedbackCanUse);
            ResetFormCommand = new RelayCommand<FeedbackModel>(ResetFeedbackForm, ResetCanUse);
        }

        // Save new feedback instance
        public void SubmitFeedback(object parameter)
        {
            // Validate Email Field
            if (!string.IsNullOrEmpty(Feedback.Email))
            {
                if (!IsValidEmail(Feedback.Email))
                {
                    MySnackbar.Enqueue("Invalid Email!");
                    return;
                }
            }

            // Validate Phone Field
            if (!string.IsNullOrEmpty(Feedback.Phone))
            {
                if (!IsDigitsOnly(Feedback.Phone))
                {
                    MySnackbar.Enqueue("Invalid Phone!");
                    return;
                }
            }

            Feedback.AverageRating = GetAverageRating(Feedback);
            Feedback.SubmitDate = DateTime.Now;
            FeedbackCollection.Add(Feedback);

            MySnackbar.Enqueue("Thank You for Feedback!");
            ResetForm();
        } 

        // Add validation for submit
        public bool SubmitFeedbackCanUse(object parameter)
        {
            foreach (var review in Feedback.Reviews)
            {
                if (review.Rating == 0)
                {
                    return false;
                }
            }
            return true;
        }

        // Reset form
        public void ResetFeedbackForm(object parameter)
        {
            ResetForm();
            MySnackbar.Enqueue("Form is Reset.");
        }

        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private static bool IsDigitsOnly(string str)
        {
            return str.All(c => c >= '0' && c <= '9');
        }

        public void ResetForm()
        {
            Feedback = new FeedbackModel {Name = string.Empty, Email = string.Empty, Phone = string.Empty};

            // Reset ratings
            foreach (var criteria in CriteriaCollection)
            {
                Feedback.Reviews.Add(new ReviewModel() { Criteria = criteria.Title, Rating = 0 });
            }
        }

        public bool ResetCanUse(object parameter)
        {
            return true;
        }

        public double GetAverageRating(FeedbackModel f)
        {
            var total = f.Reviews.Sum(review => review.Rating);
            var avg = total / f.Reviews.Count;
            return avg;
        }


    }
}