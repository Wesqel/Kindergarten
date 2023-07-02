using detskiysad__.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace detskiysad__.Pages
{
    /// <summary>
    /// Interaction logic for PrintPage.xaml
    /// </summary>
    public partial class PrintPage : Window
    {
        readonly DSctx ctx = new DSctx();
        public PrintPage(List<ChildForPrint> children)
        {
            InitializeComponent();
            ChildGrid.ItemsSource = children;
        }
        private void AddDataToReportsTable()
        {
                foreach (ChildForPrint child in ChildGrid.ItemsSource)
                {
                var report = new Reports
                {
                        FIO_Children = child.FullName,
                        AttendanceTime = TimeSpan.FromHours(child.Attendance),
                        DateReport = (DateTime)child.Date
                    };

                   ctx.Reports.Add(report);
                }

                ctx.SaveChanges();
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.PrintDialog Printdlg = new System.Windows.Controls.PrintDialog();
            if ((bool)Printdlg.ShowDialog().GetValueOrDefault())
            {
                Size pageSize = new Size(Printdlg.PrintableAreaWidth, Printdlg.PrintableAreaHeight);
                this.Measure(pageSize);
                this.Arrange(new Rect(5, 5, pageSize.Width, pageSize.Height));
                Printdlg.PrintVisual(this, Title);
            }
            AddDataToReportsTable();
        }
    }
}
