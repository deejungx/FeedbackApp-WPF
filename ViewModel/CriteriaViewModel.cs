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
    public class CriteriaViewModel : ObservableCollection<CriteriaModel>
    {
        // These criteria objects were created for testing only.
        // The Final implementation must import the criterias from a CSV file.
        // If the file is not found, the object is initialized empty (resulting in 
        // an empty feedback form.)
        public CriteriaViewModel()
        {
            Add(new CriteriaModel()
            {
                Title = "Taste satisfaction",
                Description = "How satisfied are you with the taste?"
            });

            Add(new CriteriaModel()
            {
                Title = "Service",
                Description = "How satisfied are you with the service?"
            });
        }


        //          TO DO
        // ------------------------

        //      1. Import criterias implementation
        //      2. Export criterias implementation
        //      3. Add, update and delete criteria implementation
        //      4. Save criterias in CSV



    }
}
