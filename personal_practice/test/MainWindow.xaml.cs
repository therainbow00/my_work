using System.Collections.ObjectModel;
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
using IronXL;

namespace test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class ExcelRecord
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public bool accountStatus { get; set; }
        }

        public List<ExcelRecord> LoadExcel(string filePath)
        {
            var records = new List<ExcelRecord>();
            WorkBook workbook = WorkBook.Load(filePath);
            WorkSheet sheet = workbook.WorkSheets.First();

            for (int i = 1; i < sheet.RowCount; i++)
            {
                records.Add(new ExcelRecord
                {
                    Id = int.TryParse(sheet[$"A{i + 1}"].ToString(), out int Id) ? Id : 0,
                    Name = sheet[$"B{i + 1}"].ToString(),
                    accountStatus = bool.TryParse(sheet[$"C{i + 1}"].ToString(), out bool accountStatus) ? accountStatus : false
                });
            }
            return records;
        }
        public MainWindow()
        {
            InitializeComponent();
            ExcelRecord record = new();
            record.Id = 0;
            record.Name = "Empty";
            record.accountStatus = true;
            //string path = "";
            //var data = LoadExcel(path);
            //excelListView.ItemsSource = data;

        }

        /*private void Button_Click(object sender, RoutedEventArgs e)
        {
            test.Text = "hello";
            test.Background = Brushes.Yellow;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            test.Text = "here";
            test.Background = Brushes.LightBlue;
        }*/
    }
}