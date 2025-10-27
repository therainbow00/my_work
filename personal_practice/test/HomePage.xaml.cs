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
using System.Data;
using System.Configuration;
using Microsoft.Data.SqlClient;
using System.Security.Principal;

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
            this.Initialized += ChooseOptions;
        }

        public class Computer
        {
            public string deviceName { get; set; }
            public string Make { get; set; }
            public string Model { get; set; }
            public string serialNumber { get; set; }
            public string perchaseDate { get; set; }
        }
        public class NonComputer
        {
            public string deviceName { get; set; }
            public string Make { get; set; }
            public string Model { get; set; }
            public string serialNumber { get; set; }
            public string perchaseDate { get; set; }
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
                        perchaseDate = DateTime.Parse(worksheet.Cells[i, 5].Text).ToShortDateString()
                    };

                    records.Add(record);
                }
            }
            Inventory.ItemsSource = records;
        }
        public void ComputerDatabase(string connectionString)
        {
            var records = new List<Computer>();
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                try
                {
                    connect.Open();
                    string query = "SELECT * FROM assetTracker";
                    SqlCommand command = new SqlCommand(query, connect);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var record = new Computer
                            {
                                deviceName = reader["deviceName"].ToString(),
                                Make = reader["Make"].ToString(),
                                Model = reader["Model"].ToString(),
                                serialNumber = reader["serialNumber"].ToString(),
                                perchaseDate = DateTime.Parse(reader["purchaseDate"].ToString()).ToShortDateString()
                            };

                            records.Add(record);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error retrieving data: {ex.Message}", "SQL Data Retrieval");
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
            DateTime? perchaseDate = PurchaseDate.SelectedDate;
            if (string.IsNullOrWhiteSpace(deviceName) || string.IsNullOrWhiteSpace(serialNumber) || string.IsNullOrWhiteSpace(make) || string.IsNullOrWhiteSpace(model) || string.IsNullOrWhiteSpace(deviceName))
            {
                MessageBox.Show($"Searching for device: {deviceName}, Serial: {serialNumber}, Make: {make}, Model: {model}, date: {perchaseDate?.ToShortDateString()}");
            }
        }

        private void databaseConnect(string connectionString)
        {
            //string connectionString = @"server=localhost\SQLEXPRESS;database=data;Trusted_Connection=True;TrustServerCertificate=True;";
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                try
                {
                    connect.Open();
                    if (connect.State == ConnectionState.Open)
                    {
                        MessageBox.Show("Connected to the database", "DATABASE CONNECTION");
                        StatusLight.Fill = Brushes.Green;
                    }
                    else
                    {
                        StatusLight.Fill = Brushes.Orange;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Couldn't connect to DATABASE: {ex.Message}", "SQL connect");
                    StatusLight.Fill = Brushes.Red;
                }
            }
        }

        private void ChooseOptions(object sender, EventArgs e)
        {
            string connectionString = @"server=localhost\SQLEXPRESS;database=data;Trusted_Connection=True;TrustServerCertificate=True;";
            MessageBoxResult result = MessageBox.Show("Would you like to connect using SQL??", "Data Search using SQL", MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    databaseConnect(connectionString);
                    ComputerDatabase(connectionString);
                    break;
                case MessageBoxResult.No:
                    StatusLight.Fill = Brushes.Orange;
                    string path = "C:\\Users\\mattm\\Desktop\\IT Asset Tracker.xlsx";
                    ExcelPackage.License.SetNonCommercialPersonal("test");
                    ComputerExcel(path);
                    break;
            }
        }
    }
}
