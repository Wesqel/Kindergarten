using detskiysad__.Entities;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace detskiysad__.Pages
{
    /// <summary>
    /// Interaction logic for Auth.xaml
    /// </summary>
    public partial class Auth : Page
    {
        readonly IQueryable<User> _users = new Entities.DSctx().User;
        string character = "";
        public Auth()
        {
            InitializeComponent();
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (CaptchaField.Text != character)
            {
                ShowError("Неверно введена капча");
                LoginButton.IsEnabled = false;
                DispatcherTimer timer = new DispatcherTimer()
                {
                    Interval = TimeSpan.FromSeconds(10),
                };
                timer.Tick += (s, ea) =>
                {
                    LoginButton.IsEnabled = true;
                    timer.Stop();
                };
                timer.Start();
                return;
            }
            User user = (from u in _users where u.UserLogin == LoginField.Text && u.UserPassword == PasswordField.Password select u).FirstOrDefault();
            if (user != null)
            {
                ((MainWindow)Window.GetWindow(this)).LoggedInAs.Text = $"Вы вошли как {user.UserSurname} {user.UserName} {user.UserPatronymic}";
                ((MainWindow)Window.GetWindow(this)).MainFrame.Content = new Main(user.UserRole);
                ((MainWindow)Window.GetWindow(this)).LoggedInAs.Visibility = Visibility.Visible;
                ((MainWindow)Window.GetWindow(this)).LogoutButton.Visibility = Visibility.Visible;
                ErrorMessage.Visibility = Visibility.Hidden;
            }
            else if (user == null)
            {
                ShowError("Неверный логин или пароль.");
                return;
            }

        }
        private void ShowError(string mesage)
        {
            ErrorMessage.Text = mesage;
            ErrorMessage.Visibility = Visibility.Visible;
            GenerateCaptcha();
        }

        private void GenerateCaptcha()
        {
            character = "";
            CaptchaCanvas.Children.Clear();
            CaptchaContainer.Visibility = Visibility.Visible;
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            for (int i = 0; i < 4; i++)
            {
                character += characters[random.Next(characters.Length)].ToString();

                CaptchaCanvas.Children.Add(new TextBlock()
                {
                    Text = character[i].ToString(),
                    Margin = new Thickness(60 * i, CaptchaCanvas.ActualHeight / 4 + random.Next((int)-CaptchaCanvas.ActualHeight / 4, (int)CaptchaCanvas.ActualHeight / 4), 0, 0),
                    FontSize = 32,
                });
            }
            CaptchaCanvas.Children.Add(new Line()
            {
                Stroke = (Brush)FindResource("Accent"),
                StrokeThickness = 6,
                X1 = 0,
                X2 = CaptchaCanvas.ActualWidth,
                Y1 = CaptchaCanvas.ActualHeight / 2 + random.Next((int)(-CaptchaCanvas.ActualHeight / 3), (int)(CaptchaCanvas.ActualHeight / 3)),
                Y2 = CaptchaCanvas.ActualHeight / 2 + random.Next((int)(-CaptchaCanvas.ActualHeight / 3), (int)(CaptchaCanvas.ActualHeight / 3)),
            });
        }

    }
}

