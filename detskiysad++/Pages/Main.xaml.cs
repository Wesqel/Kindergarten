using detskiysad__.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace detskiysad__.Pages
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Page
    {
        readonly DSctx ctx = new DSctx();
        public Main(int roleId)
        {
            InitializeComponent();
            if (roleId == 1)
            {
                AdminPanel.Visibility = Visibility.Visible;
            }
            else if (roleId == 2)
            {
                MentorPanel.Visibility = Visibility.Visible;
            }
            ItemsList.ItemsSource = ctx.Сhildren.Select(c => new { Child = c, ExtraClassName = c.Extra_class.Select(ec => ec.Name).ToList() }).ToList();
            List<string> groups = (from g in ctx.Group select g.NameGroup).ToList();
            groups.Add("Все группы");
            List<string> extra = (from g in ctx.Extra_class select g.Name).ToList();
            extra.Add("Все занятости");
            List<string> menu = (from g in ctx.Menu select g.Name).ToList();
            menu.Add("Все меню");
            List<string> age = (from g in ctx.Сhildren select g.ChildrenBirthday.ToString()).ToList();
            age.Add("Любой год");
            ManufacturerFilter.ItemsSource = groups;
            ManufacturerFilter.SelectedIndex = groups.IndexOf("Все группы");
            ManufacturerFilter2.ItemsSource = extra;
            ManufacturerFilter2.SelectedIndex = extra.IndexOf("Все занятости");
            ManufacturerFilter3.ItemsSource = menu;
            ManufacturerFilter3.SelectedIndex = menu.IndexOf("Все меню");

        }
        private void ResetButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SearchField.Text = "";
            ManufacturerFilter.SelectedIndex = ManufacturerFilter.Items.IndexOf("Все группы");
            ManufacturerFilter2.SelectedIndex = ManufacturerFilter2.Items.IndexOf("Все занятости");
            ManufacturerFilter3.SelectedIndex = ManufacturerFilter3.Items.IndexOf("Все меню");
            Update();
        }

        private void Update()
        {
            List<Сhildren> children = new DSctx().Сhildren.ToList();
            if (SearchField.Text != "")
            {
                children = children.FindAll((e) =>
                {
                    return e.ChildrenName.ToLower().Contains(SearchField.Text.ToLower()) ||
                           e.ChildrenSurname.ToLower().Contains(SearchField.Text.ToLower()) ||
                           e.ChildrenPatronymic.ToLower().Contains(SearchField.Text.ToLower()) ||
                           e.Group.NameGroup.ToLower().Contains(SearchField.Text.ToLower()) ||
                           e.Home_address.ToLower().Contains(SearchField.Text.ToLower()) ||
                           e.Birth_certificate.ToLower().Contains(SearchField.Text.ToLower());
                    //e.Extra_class.Name.ToLower().Contains(SearchField.Text.ToLower());
                });
            }
            if (ManufacturerFilter.SelectedIndex != ManufacturerFilter.Items.IndexOf("Все группы"))
            {
                children = children.FindAll(e => e.Group.NameGroup == (string)ManufacturerFilter.SelectedItem);
            }
            if (ManufacturerFilter2.SelectedIndex != ManufacturerFilter2.Items.IndexOf("Все занятости"))
            {
                children = children.FindAll(e => e.Extra_class.Any(extra => extra.Name == (string)ManufacturerFilter2.SelectedItem));
            }
            if (ManufacturerFilter3.SelectedIndex != ManufacturerFilter3.Items.IndexOf("Все меню"))
            {
                children = children.FindAll(e => e.Menu.Name == (string)ManufacturerFilter3.SelectedItem);
            }
            ShownAmountText.Text = $"Показано: {children.Count} из {ctx.Сhildren.Count()}";
            ItemsList.ItemsSource = children;
        }

        private void SearchField_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update();
        }

        private void ManufacturerFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }

        private void SortDescending_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Update();
        }

        private void SortAscending_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Update();
        }

        private void DeleteButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ItemsList.SelectedItem == null)
            {
                MessageBox.Show("Не выбран ребенок.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Сhildren toDel = ctx.Сhildren.Single(o => o.idСhildren == ((Сhildren)ItemsList.SelectedItem).idСhildren);

            MessageBoxResult toDelete = MessageBox.Show($"Удалить {toDel.FullName}?",
                "Вы уверены?", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            if (toDelete == MessageBoxResult.Yes)
            {
                try
                {
                    using (var dbctx = new DSctx())
                    {
                        var tdl = dbctx.Сhildren.Single(o => o.idСhildren == toDel.idСhildren);
                        dbctx.Сhildren.Remove(tdl);
                        dbctx.SaveChanges();
                    }
                    Update();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Невозможно удалить товар.\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void EditButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ItemsList.SelectedItem == null)
            {
                MessageBox.Show("Не выбран ребенок.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            AddEditPage addEditPage = new AddEditPage(((Сhildren)ItemsList.SelectedItem).idСhildren, true);
            addEditPage.ShowDialog();
            Update();
        }

        private void AddButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            AddEditPage addEditPage = new AddEditPage();
            addEditPage.ShowDialog();
            Update();
        }

        private void addExtraClass_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ItemsList.SelectedItem == null)
            {
                MessageBox.Show("Не выбран ребенок.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            AddExtra addExtra = new AddExtra(((Сhildren)ItemsList.SelectedItem).idСhildren);
            addExtra.ShowDialog();
            Update();
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            List<ChildForPrint> children = new List<ChildForPrint>();
            foreach (Сhildren child in ItemsList.Items)
            {
                children.Add(new ChildForPrint(child, DateFilter.SelectedDate));
            }
            PrintPage printPage = new PrintPage(children);

            printPage.ShowDialog();
        }
    }
}
