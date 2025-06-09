using OfficeOpenXml;
using System;
using System.Collections.Generic;
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
            string path = "C:\\Users\\mattm\\Desktop\\2024 October Audit Result.xlsx";
            ExcelPackage.License.SetNonCommercialPersonal("test");
            ComputerExcel(path);
        }

        public class Computer
        {
            public string deviceName { get; set; }
            public string Make { get; set; }
            public bool Model { get; set; }
            public string serialNumber { get; set; }
            public string perchaseDate { get; set; }
        }
        public class NonComputer
        {
            public string deviceName { get; set; }
            public string Make { get; set; }
            public string Model { get; set; }
            public string serialNumber { get; set; }
            public DateTime perchaseDate { get; set; }
        }

        public void ComputerExcel(string path)
        {
            var records = new List<Computer>();

            using (var package = new ExcelPackage(new FileInfo(path)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                int columnCount = worksheet.Dimension.End.Column;
                int rowCount = worksheet.Dimension.End.Row;

                Dictionary<string, int> headerMap = new();

                for (int col = 1; col <= columnCount; col++)
                {
                    string header = worksheet.Cells[1, col].Text.Trim();
                    if (!string.IsNullOrEmpty(header)) headerMap[header] = col;
                }

                for (int i = 2; i <= rowCount; i++)
                {
                    var record = new Computer();
                    if (headerMap.TryGetValue("EmployeeID", out int idCol))
                    {
                        record.deviceName = worksheet.Cells[i, idCol].Text;
                    }
                    if (headerMap.TryGetValue("DisplayName", out int nameCol))
                    {
                        record.Make = worksheet.Cells[i, nameCol].Text;
                    }
                    if (headerMap.TryGetValue("Enabled", out int statusCol))
                    {
                        string status = worksheet.Cells[i, statusCol].Text.ToLower();
                        record.Model = bool.Parse(status);
                    }
                    if (headerMap.TryGetValue("action", out int actionCol))
                    {
                        record.serialNumber = worksheet.Cells[i, actionCol].Text;
                    }
                    if (headerMap.TryGetValue("Department", out int departmentCol))
                    {
                        record.perchaseDate = worksheet.Cells[i, departmentCol].Text;
                    }

                    records.Add(record);
                }
            }
            Inventory.ItemsSource = records;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
