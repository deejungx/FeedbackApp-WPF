using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using CsvHelper;
using CsvHelper.Configuration;
using RestaurantFeedbackApp.Model;
using RestaurantFeedbackApp.Views;

namespace RestaurantFeedbackApp.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public ISnackbarMessageQueue MySnackbar { get; set; }
        private ObservableCollection<FeedbackModel> _feedbackCollection;
        private ObservableCollection<CriteriaModel> _criteriaCollection;

        private const string DataSeparator = ";";
        private const string CriteriaFileName = "Data\\criteriaData";

        public ObservableCollection<FeedbackModel> FeedbackCollection
        {
            get => _feedbackCollection;
            set
            {
                _feedbackCollection = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<CriteriaModel> CriteriaCollection
        {
            get => _criteriaCollection;
            set
            {
                _criteriaCollection = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            MySnackbar = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(3000));
            FeedbackCollection = new ObservableCollection<FeedbackModel>();
            CriteriaCollection = new ObservableCollection<CriteriaModel>();
            LoadCriteriaCsv(CriteriaFileName);
            FeedbackViewModel.MySnackbar = MySnackbar;
            AdminLoginPage.MySnackbar = MySnackbar;
            DashboardViewModel.MySnackbar = MySnackbar;
            FeedbackViewModel.FeedbackCollection = FeedbackCollection;
            FeedbackViewModel.CriteriaCollection = CriteriaCollection;
            DashboardViewModel.FeedbackCollection = FeedbackCollection;
            DashboardViewModel.CriteriaCollection = CriteriaCollection;
        }
        
        public void LoadCriteriaCsv(string fileName)
        {
            fileName = System.IO.Path.ChangeExtension(fileName, ".csv");
            var pathName = System.IO.Path.GetFullPath(fileName);
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
                Delimiter = DataSeparator
            };

            using var reader = new StreamReader(pathName);
            using var csv = new CsvReader(reader, config);
            csv.Read();
            while (csv.Read())
            {
                var criteria = new CriteriaModel
                {
                    Title = csv.GetField(0),
                    Description = csv.GetField(1)
                };
                CriteriaCollection.Add(criteria);
            }
        }

    }
}
