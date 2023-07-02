using System.Windows;

namespace detskiysad__
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly Pages.Auth Auth = new Pages.Auth();
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Content = Auth;
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = Auth;
            LoggedInAs.Visibility = Visibility.Hidden;
            LogoutButton.Visibility = Visibility.Hidden;
        }
    }
}
