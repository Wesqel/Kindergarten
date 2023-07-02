using detskiysad__.Entities;
using System.Linq;
using System.Windows;

namespace detskiysad__.Pages
{
    /// <summary>
    /// Interaction logic for AddExtra.xaml
    /// </summary>
    public partial class AddExtra : Window
    {
        readonly DSctx ctx = new DSctx();
        public AddExtra(int childId)
        {
            InitializeComponent();
            this.DataContext = ctx.Сhildren.Find(childId);
            ManufacturerFilter.ItemsSource = (from p in ctx.Extra_class select p).ToList();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Сhildren selectedChild = (Сhildren)this.DataContext;
            Extra_class selectedExtraClass = (Extra_class)ManufacturerFilter.SelectedItem;

            
            bool isAlreadyAdded = selectedChild.Extra_class.Any(ec => ec.Name == selectedExtraClass.Name);

            if (isAlreadyAdded)
            {
                selectedChild.Extra_class.Remove(selectedExtraClass);
            }
            else
            {
                selectedChild.Extra_class.Add(selectedExtraClass);
            }

            ctx.SaveChanges();
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
