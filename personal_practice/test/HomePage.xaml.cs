using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace test
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : UserControl
    {
        public HomePage()
        {
            InitializeComponent();
            string path = "C:\\Users\\mattm\\Desktop\\IT Asset Tracker.xlsx";
            ExcelPackage.License.SetNonCommercialPersonal("test");
            ComputerExcel(path);
        }

        public class Computer
        {
            public string deviceName { get; set; }
            public string Make { get; set; }
            public string Model { get; set; }
            public string serialNumber { get; set; }
            public string purchaseDate { get; set; }
        }
        public class NonComputer
        {
            public string deviceName { get; set; }
            public string Make { get; set; }
            public string Model { get; set; }
            public string serialNumber { get; set; }
            public string purchaseDate { get; set; }
        }

        public void ComputerExcel(string path)
        {
            var records = new List<Computer>();

            using (var package = new ExcelPackage(new FileInfo(path)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                int columnCount = worksheet.Dimension.End.Column;
                int rowCount = worksheet.Dimension.End.Row;

                for (int i = 2; i <= rowCount; i++)
                {
                    var record = new Computer
                    {
                        deviceName = worksheet.Cells[i, 1].Text,
                        Make = worksheet.Cells[i, 2].Text,
                        Model = worksheet.Cells[i, 3].Text,
                        serialNumber = worksheet.Cells[i, 4].Text, 
                        purchaseDate = DateTime.Parse(worksheet.Cells[i, 5].Text).ToShortDateString()
                    };

                    records.Add(record);
                }
            }
            Inventory.ItemsSource = records;
        }

        private void SubmitSearch_Click(object sender, RoutedEventArgs e)
        {
            string deviceName = DeviceName.Text;
            string serialNumber = SerialNumber.Text;
            string make = Make.Text;
            string model = Model.Text;
            DateTime? purchaseDate = PurchaseDate.SelectedDate;
            if (string.IsNullOrWhiteSpace(deviceName) || string.IsNullOrWhiteSpace(serialNumber) || string.IsNullOrWhiteSpace(make) || string.IsNullOrWhiteSpace(model) || string.IsNullOrWhiteSpace(deviceName))
            {
                MessageBox.Show($"Searching for device: {deviceName}, Serial: {serialNumber}, Make: {make}, Model: {model}, date: {purchaseDate?.ToShortDateString()}");
            }
        }
    }
}
