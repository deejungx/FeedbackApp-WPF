using System;
using System.Collections.Generic;
using CsvHelper;
using CsvHelper.Configuration;
using MaterialDesignThemes.Wpf;
using RestaurantFeedbackApp.Model;
using RestaurantFeedbackApp.ViewModel.Commands;
using RestaurantFeedbackApp.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Data;
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Win32;

namespace RestaurantFeedbackApp.ViewModel
{
    public class DashboardViewModel : ViewModelBase
    {
        private CriteriaModel _criteriaModel;
        public static ObservableCollection<FeedbackModel> FeedbackCollection { get; set; }
        public static ObservableCollection<CriteriaModel> CriteriaCollection { get; set; }
        public static ISnackbarMessageQueue MySnackbar { get; set; }
        public RelayCommand<ObservableCollection<CriteriaModel>> DeleteCriteriaCommand { get; }
        public RelayCommand<ObservableCollection<CriteriaModel>> EditCriteriaCommand { get; }
        public RelayCommand<ObservableCollection<CriteriaModel>> AddCriteriaCommand { get; }
        public RelayCommand<ObservableCollection<CriteriaModel>> ImportCriteriaCommand { get; }
        public RelayCommand<ObservableCollection<CriteriaModel>> SaveCriteriaCommand { get; }
        public RelayCommand<ObservableCollection<FeedbackModel>> ExportFeedbackCommand { get; }
        public RelayCommand<ObservableCollection<FeedbackModel>> ImportFeedbackCommand { get; }
        public RelayCommand<ObservableCollection<FeedbackModel>> FilterTextCommand { get; }
        public RelayCommand<ObservableCollection<FeedbackModel>> ResetFilterCommand { get; }

        // Export and Import variables
        private const string DataSeparator = ";";
        private const string CriteriaFileName = "Data\\criteriaData";
        private const string FileFilter = "Json files (*.json)|*.json";
        private readonly SaveFileDialog _saveFileDialog;
        private readonly OpenFileDialog _openFileDialog;

        public Dictionary<string, List<int>> ChartData { get; set; }
        public SeriesCollection FeedbackSeriesCollection { get; set; }
        public List<string> Labels { get; set; }
        public Func<double, string> Formatter { get; set; }
        public int FeedbackCount { get; set; }
        private ICollectionView _dataGridCollection;
        private string _filterName;

        public CriteriaModel CriteriaModel
        {
            get => _criteriaModel;
            set
            {
                _criteriaModel = value;
                OnPropertyChanged();
            }
        }

        public ICollectionView DataGridCollection
        {
            get => _dataGridCollection;
            set
            {
                _dataGridCollection = value;
                OnPropertyChanged();
            }
        }

        public string FilterName
        {
            get => _filterName;
            set
            {
                _filterName = value;
                OnPropertyChanged();
            }
        }


        public DashboardViewModel()
        {
            CriteriaModel = new CriteriaModel();
            DeleteCriteriaCommand = new RelayCommand<ObservableCollection<CriteriaModel>>(DeleteCriteria, DeleteCriteriaCanUse);
            EditCriteriaCommand = new RelayCommand<ObservableCollection<CriteriaModel>>(EditCriteria, EditCriteriaCanUse);
            AddCriteriaCommand = new RelayCommand<ObservableCollection<CriteriaModel>>(AddCriteria, AddCriteriaCanUse);
            ImportCriteriaCommand = new RelayCommand<ObservableCollection<CriteriaModel>>(ImportCriteria, ImportCriteriaCanUse);
            SaveCriteriaCommand = new RelayCommand<ObservableCollection<CriteriaModel>>(SaveCriteria, SaveCriteriaCanUse);
            ExportFeedbackCommand = new RelayCommand<ObservableCollection<FeedbackModel>>(ExportFeedback, ExportFeedbackCanUse);
            ImportFeedbackCommand = new RelayCommand<ObservableCollection<FeedbackModel>>(ImportFeedback, ImportFeedbackCanUse);
            FilterTextCommand = new RelayCommand<ObservableCollection<FeedbackModel>>(FilterByText, FilterCanUse);
            ResetFilterCommand = new RelayCommand<ObservableCollection<FeedbackModel>>(ResetFilterText, ResetFilterCanUse);
            _saveFileDialog = new SaveFileDialog();
            _openFileDialog = new OpenFileDialog();
            ChartData = new Dictionary<string, List<int>>();
            Labels = new List<string>();
            FeedbackSeriesCollection = new SeriesCollection();
            InitializeChart();
            FeedbackCount = FeedbackCollection.Count;
            DataGridCollection = CollectionViewSource.GetDefaultView(FeedbackCollection);
            DataGridCollection.Filter = new Predicate<object>(Filter);
        }

        private void ResetFilterText(object parameter)
        {
            FilterName = string.Empty;
            _dataGridCollection?.Refresh();
        }

        private bool ResetFilterCanUse(object parameter)
        {
            return true;
        }

        private void FilterByText(object parameter)
        {
            _dataGridCollection?.Refresh();
        }

        private bool FilterCanUse(object parameter)
        {
            return !string.IsNullOrEmpty(_filterName);
        }

        public bool Filter(object parameter)
        {
            if (!(parameter is FeedbackModel data)) return false;
            if (string.IsNullOrEmpty(FilterName)) return true;
            if (string.IsNullOrEmpty(data.Name))
            {
                return false;
            }
            return data.Name.Contains(FilterName);

        }

        private void ImportFeedback(object parameter)
        {
            _openFileDialog.Filter = FileFilter;
            _openFileDialog.FilterIndex = 2;
            _openFileDialog.RestoreDirectory = true;
            if (_openFileDialog.ShowDialog() != true) return;
            //Read the contents of the file into a stream
            var fileStream = _openFileDialog.OpenFile();

            using var reader = new StreamReader(fileStream);
            var fileContent = reader.ReadToEnd();

            var importedFeedbackCollection = JsonSerializer.Deserialize<ObservableCollection<FeedbackModel>>(fileContent);

            foreach (var feedbackItem in importedFeedbackCollection)
            {
                FeedbackCollection.Add(feedbackItem);
            }
            
            MySnackbar.Enqueue("Feedback file imported!");
        }

        private static bool ImportFeedbackCanUse(object parameter)
        {
            return true;
        }

        private void ExportFeedback(object parameter)
        {
            var serializedFeedbackCollection = JsonSerializer.Serialize(FeedbackCollection);
            _saveFileDialog.FileName = "feedback" + DateTime.Now.ToString("yy-MMM-dd");
            _saveFileDialog.DefaultExt = ".json";
            _saveFileDialog.FilterIndex = 2;
            _saveFileDialog.Filter = FileFilter;
            var result = _saveFileDialog.ShowDialog();
            if (result != true) return;
            // Save document
            var filename = _saveFileDialog.FileName;
            if (string.IsNullOrEmpty(filename)) return;
            File.WriteAllText(filename, serializedFeedbackCollection);
            MySnackbar.Enqueue("Feedback saved!");
        }

        private static bool ExportFeedbackCanUse(object parameter)
        {
            return true;
        }

        private static void SaveCriteria(object parameter)
        {
            SaveCriteriaData(CriteriaFileName);
            MySnackbar.Enqueue("Criteria saved on Storage!");

        }

        private void ImportCriteria(object parameter)
        {
            CriteriaCollection.Clear();
            LoadCriteriaCsv(CriteriaFileName);
            MySnackbar.Enqueue("Criteria imported from Save File!");
        }

        private static bool SaveCriteriaCanUse(object parameter)
        {
            return true;
        }

        private static bool ImportCriteriaCanUse(object parameter)
        {
            return true;
        }

        private static bool AddCriteriaCanUse(object parameter)
        {
            // Maximum criteria count set to 8
            return !CriteriaCollection.Count.Equals(8);
        }

        private static async void AddCriteria(object parameter)
        {
            var newCriteriaViewModel = new AddCriteriaDialogViewModel();
            var view = new AddCriteriaDialog()
            {
                DataContext = newCriteriaViewModel
            };
            await DialogHost.Show(view, "RootDialog", AddCriteriaClosingEventHandler);
            var newCriteria = newCriteriaViewModel.NewCriteria;
            if (string.IsNullOrEmpty(newCriteria.Title))
            {
                return;
            }
            CriteriaCollection.Add(newCriteria);
        }

        private bool EditCriteriaCanUse(object parameter)
        {
            return CriteriaSelectedCount(CriteriaCollection).Equals(1);
        }

        private static async void EditCriteria(object parameter)
        {
            var selectedCriteria = CriteriaCollection.First(criteria => criteria.IsSelected);
            var view = new CriteriaDialog()
            {
                DataContext = new CriteriaDialogViewModel { EditableCriteria = selectedCriteria }
            };
            await DialogHost.Show(view, "RootDialog", ClosingEventHandler);
        }

        private static void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            MySnackbar.Enqueue("Criteria is Updated!");
        }

        private static void AddCriteriaClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            MySnackbar.Enqueue("New Criteria Added!");
        }

        private bool DeleteCriteriaCanUse(object parameter)
        {
            return !CriteriaSelectedCount(CriteriaCollection).Equals(0);
        }

        private static void DeleteCriteria(object parameter)
        {
            foreach (var criteria in CriteriaCollection.ToList())
            {
                if (criteria.IsSelected)
                {
                    CriteriaCollection.Remove(criteria);
                }
            }
            MySnackbar.Enqueue("Deleted selected criteria!");
        }

        public int CriteriaSelectedCount(ObservableCollection<CriteriaModel> criteriaCollection)
        {
            var selectedCount = criteriaCollection.Count(criteria => criteria.IsSelected);
            return selectedCount;
        }


        public static void SaveCriteriaData(string fileName)
        {
            fileName = Path.ChangeExtension(fileName, ".csv");
            var pathName = Path.GetFullPath(fileName);
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = DataSeparator
            };
            using var writer = new StreamWriter(pathName);
            using var csv = new CsvWriter(writer, config);
            csv.WriteHeader<CriteriaModel>();
            csv.NextRecord();
            foreach (var criteria in CriteriaCollection)
            {
                csv.WriteRecord(criteria);
                csv.NextRecord();
            }
        }

        public void LoadCriteriaCsv(string fileName)
        {
            fileName = Path.ChangeExtension(fileName, ".csv");
            var pathName = Path.GetFullPath(fileName);
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
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

        public void InitializeChart()
        {
            // Arrange data in dictionary of criteria keys
            foreach (var feedbackItem in FeedbackCollection)
            {
                foreach (var review in feedbackItem.Reviews)
                {
                    if (!ChartData.ContainsKey(review.Criteria))
                    {
                        ChartData[review.Criteria] = new List<int>();
                    }
                    ChartData[review.Criteria].Add(review.Rating);
                }
            }

            // Create two ordered list of criteria and average rating
            var criteriaList = new List<string>(ChartData.Keys);
            var avgRating = new List<double>();
            foreach (var criteriaItem in criteriaList)
            {
                var average = ChartData[criteriaItem].Count > 0 ? ChartData[criteriaItem].Average() : 0.0;
                avgRating.Add(average);
            }

            FeedbackSeriesCollection.Add(new ColumnSeries
            {
                Title = "Average Rating",
                Values = new ChartValues<double> (avgRating)
            });
            Labels = criteriaList;
            Formatter = value => value.ToString("N");
        }

    }
}
