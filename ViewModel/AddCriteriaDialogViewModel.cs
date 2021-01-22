using System;
using System.Collections.Generic;
using System.Text;
using RestaurantFeedbackApp.Model;

namespace RestaurantFeedbackApp.ViewModel
{
    class AddCriteriaDialogViewModel : ViewModelBase
    {
        private CriteriaModel _newCriteria;

        public CriteriaModel NewCriteria
        {
            get => _newCriteria;
            set
            {
                _newCriteria = value;
                OnPropertyChanged();
            }
        }

        public AddCriteriaDialogViewModel()
        {
            NewCriteria = new CriteriaModel();
        }
    }
}
