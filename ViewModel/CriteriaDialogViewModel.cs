using System;
using System.Collections.Generic;
using System.Text;
using RestaurantFeedbackApp.Model;

namespace RestaurantFeedbackApp.ViewModel
{
    public class CriteriaDialogViewModel : ViewModelBase
    {
        private CriteriaModel _editableCriteria;

        public CriteriaModel EditableCriteria
        {
            get => _editableCriteria;
            set
            {
                _editableCriteria = value;
                OnPropertyChanged();
            }
        }

        public CriteriaDialogViewModel()
        {
            EditableCriteria = new CriteriaModel();
        }
    }
}
