using detskiysad__.Entities;
using Microsoft.Win32;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace detskiysad__.Pages
{
    /// <summary>
    /// Interaction logic for AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Window
    {
        readonly DSctx ctx = new DSctx();
        readonly bool editMode;
        readonly Сhildren child;
        public AddEditPage(int? childId = null, bool editMode = false)
        {
            InitializeComponent();
            this.editMode = editMode;
            child = childId == null ? new Сhildren() : ctx.Сhildren.Find(childId);
            DataContext = child;
            GroupName.ItemsSource = (from g in ctx.Group select g).ToList();
            MenuText.ItemsSource = (from p in ctx.Menu select p).ToList();
        }

        private void Button_Click_Sojr(object sender, RoutedEventArgs e)
        {
            if (AreAllFieldsFilled())
            {

                if (editMode == false)
                {
                    var lastId = ctx.Сhildren.OrderByDescending(p => p.idСhildren).FirstOrDefault()?.idСhildren ?? 0;
                    var count = lastId + 1;
                    child.idСhildren = count;
                    ctx.Entry(child).State = child.idСhildren == count ? EntityState.Added : EntityState.Modified;
                }
                else
                {
                    ctx.Entry(child).State = child.idСhildren == 0 ? EntityState.Added : EntityState.Modified;
                }
                ctx.SaveChanges();
                Close();
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click_Foto(object sender, RoutedEventArgs e)
        {
            FileDialog fileDialog = new OpenFileDialog()
            {
                Title = "Выберите фото",
                Filter = "Файлы фотографий|*.png;*.jpg",
                Multiselect = false,
            };
            fileDialog.ShowDialog();
            if (fileDialog.FileName == "")
                return;
            byte[] fileAsBytes = File.ReadAllBytes(fileDialog.FileName);
            child.ChildrenPhoto = fileAsBytes;
            ProductImage.Source = new BitmapImage(new Uri(fileDialog.FileName));
        }

        private void ChildrenBirthday_DateChanged(object sender, SelectionChangedEventArgs e)
        {
            TimeSpan age = (TimeSpan)(DateTime.Now - ChildrenBirthday.SelectedDate);
            int years = (int)(age.TotalDays / 365.25);

            if (years >= 2 && years <= 3)
            {
                GroupName.SelectedIndex = 0;
            }
            else if (years >= 3 && years <= 4)
            {
                GroupName.SelectedIndex = 1;
            }
            else if (years >= 5 && years <= 7.5)
            {
                GroupName.SelectedIndex = 2;
            }
            else
            {
                GroupName.SelectedIndex = -1;
            }
        }

        private bool AreAllFieldsFilled()
        {

            if (string.IsNullOrEmpty(child.ChildrenName) ||
                string.IsNullOrEmpty(child.ChildrenSurname) ||
                string.IsNullOrEmpty(child.ChildrenPatronymic) ||
                !ChildrenBirthday.SelectedDate.HasValue ||
                string.IsNullOrEmpty(child.Birth_certificate) ||
                string.IsNullOrEmpty(child.Home_address) ||
                string.IsNullOrEmpty(child.SNILS) ||
                GroupName.SelectedItem == null ||
                MenuText.SelectedItem == null)
            {
                return false;
            }
            return true;
        }

        private void MenuText_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
