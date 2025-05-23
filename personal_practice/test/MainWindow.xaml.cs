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
using System.IO;
using OfficeOpenXml;
using System.Security.Cryptography.X509Certificates;

namespace test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            string path = "C:\\Users\\mattm\\Desktop\\2024 October Audit Result.xlsx";
            ExcelPackage.License.SetNonCommercialPersonal("test");
            LoadExcelLeftSide(path);
            LoadExcelRightSide(path);
        }
        public class ExcelLeftRecord
        {
            public string? Id { get; set; }
            public string? Name { get; set; }
            public bool accountStatus { get; set; }
            public string? action { get; set; }
        }
        public class ExcelRightRecord
        {
            public string? Id { get; set; }
            public string? Name { get; set; }
            public bool accountStatus { get; set; }
            public string? action { get; set; }
        }
        public class ExcelBottomRecord
        {
            public string? Id { get; set; }
            public string? Name { get; set; }
            public bool accountStatus { get; set; }
            public string? action { get; set; }
        }

        public void LoadExcelLeftSide(string path)
        {
            var records = new List<ExcelLeftRecord>();

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
                    var record = new ExcelLeftRecord();
                    if (headerMap.TryGetValue("EmployeeID", out int idCol))
                    {
                        record.Id = worksheet.Cells[i, idCol].Text;
                    }
                    if (headerMap.TryGetValue("DisplayName", out int nameCol))
                    {
                        record.Name = worksheet.Cells[i, nameCol].Text;
                    }
                    if (headerMap.TryGetValue("Enabled", out int statusCol))
                    {
                        string status = worksheet.Cells[i, statusCol].Text.ToLower();
                        record.accountStatus = bool.Parse(status);
                    }
                    if (headerMap.TryGetValue("action", out int actionCol))
                    {
                        record.action = worksheet.Cells[i, actionCol].Text;
                    }

                    records.Add(record);
                }
            }
            ExcelDataLeftGrid.ItemsSource = records;
        }
        public void LoadExcelRightSide(string path)
        {
            var records = new List<ExcelLeftRecord>();

            using (var package = new ExcelPackage(new FileInfo(path)))
            {
                var worksheet = package.Workbook.Worksheets[3];
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
                    var record = new ExcelLeftRecord();
                    if (headerMap.TryGetValue("EmployeeId", out int idCol))
                    {
                        record.Id = worksheet.Cells[i, idCol].Text;
                    }
                    if (headerMap.TryGetValue("DisplayName", out int nameCol))
                    {
                        record.Name = worksheet.Cells[i, nameCol].Text;
                    }
                    if (headerMap.TryGetValue("AccountEnabled", out int statusCol))
                    {
                        string status = worksheet.Cells[i, statusCol].Text.ToLower();
                        record.accountStatus = bool.Parse(status);
                    }
                    if (headerMap.TryGetValue("action", out int actionCol))
                    {
                        record.action = worksheet.Cells[i, actionCol].Text;
                    }

                    records.Add(record);
                }
            }
            ExcelDataRightGrid.ItemsSource = records;
        }

        public void LoadExcelBottomSide(string path)
        {
            var records = new List<ExcelBottomRecord>();

            using (var package = new ExcelPackage(new FileInfo(path)))
            {
                var worksheet = package.Workbook.Worksheets[4];
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
                    var record = new ExcelBottomRecord();
                    if (headerMap.TryGetValue("EmployeeId", out int idCol))
                    {
                        record.Id = worksheet.Cells[i, idCol].Text;
                    }
                    if (headerMap.TryGetValue("DisplayName", out int nameCol))
                    {
                        record.Name = worksheet.Cells[i, nameCol].Text;
                    }
                    if (headerMap.TryGetValue("AccountEnabled", out int statusCol))
                    {
                        string status = worksheet.Cells[i, statusCol].Text.ToLower();
                        record.accountStatus = bool.Parse(status);
                    }
                    if (headerMap.TryGetValue("action", out int actionCol))
                    {
                        record.action = worksheet.Cells[i, actionCol].Text;
                    }

                    records.Add(record);
                }
            }
            ExcelDataBottomGrid.ItemsSource = records;
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