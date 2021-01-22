using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MaterialDesignThemes.Wpf;

namespace RestaurantFeedbackApp.Views
{
    /// <summary>
    /// Interaction logic for AdminLoginPage.xaml
    /// </summary>
    public partial class AdminLoginPage : UserControl
    {
        public static ISnackbarMessageQueue MySnackbar { get; set; }
        public AdminLoginPage()
        {
            InitializeComponent();
        }

        private void Login_OnClick(object sender, RoutedEventArgs e)
        {
            if (!Username.Text.Equals("admin"))
            {
                MySnackbar.Enqueue("Wrong Username!");
                return;
            }
            if (!Password.Password.Equals("islington"))
            {
                MySnackbar.Enqueue("Wrong Password!");
                return;
            }

            GridMain.Children.Clear();
            GridMain.Children.Add(new DashboardPage());
        }
    }
}
