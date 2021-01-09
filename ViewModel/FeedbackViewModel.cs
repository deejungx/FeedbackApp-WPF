using System.Windows.Input;
using System.ComponentModel;
using System.Collections.Generic;
using RestaurantFeedbackApp.Model;
using RestaurantFeedbackApp.ViewModel.Commands;
using System;
using System.Windows;
using System.Collections.ObjectModel;

namespace RestaurantFeedbackApp.ViewModel
{
    public class FeedbackViewModel : ObservableCollection<FeedbackModel>
    {

        public RelayCommand<FeedbackModel> SubmitFeedbackCommand { get; private set; }
        public RelayCommand<FeedbackModel> ResetFormCommand { get; private set; }

        public FeedbackViewModel()
        {
            this.SubmitFeedbackCommand = new RelayCommand<FeedbackModel>(SubmitFeedback, SubmitFeedbackCanUse);
            this.ResetFormCommand = new RelayCommand<FeedbackModel>(ResetFeedbackForm, ResetCanUse);
        }

        // Save new feedback instance
        public void SubmitFeedback(FeedbackModel feedback)
        {
            FeedbackModel fb = new FeedbackModel
            {
                Name = feedback.Name,
                Email = feedback.Email,
                Phone = feedback.Phone

                // Having difficulty binding feedback ratings !!!
                // Ratings = feedback.Ratings (Dictionary<string, int> -> criteria name, rating )
            };
            Add(fb);

            // After success, show message using snackbar.
        } 

        // Add validation for submit
        public bool SubmitFeedbackCanUse(FeedbackModel feedback)
        {
            return true;
        }

        // Reset form
        public void ResetFeedbackForm(FeedbackModel feedback)
        {
            feedback.Name = string.Empty;
            feedback.Email = string.Empty;
            feedback.Phone = string.Empty;

            // Add reset for ratings
        }

        public bool ResetCanUse(FeedbackModel feedback)
        {
            return true;
        }



        //          TO DO
        // ------------------------

        //      1. Import feedbacks implementation
        //      2. Export feedbacks implementation
        //      3. Filtering feedbacks implementation 
        //         (filter by: date, average rating)
        //      4. Sorting feedback implementation
        //         ( sort by: date, name)
        //      5. Chart implementation
        //         https://lvcharts.net/


    }
}