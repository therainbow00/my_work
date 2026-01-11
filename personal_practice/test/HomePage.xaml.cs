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
using Microsoft.Identity.Client;

namespace test
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : UserControl
    {
        bool isConnected = false;
        bool defaultLoop = false;
        public DateTime? perchaseDate;
        public HomePage()
        {
            InitializeComponent();
            this.Initialized += ChooseOptions;
        }

        public class Computer
        {
            public required string deviceName { get; set; }
            public required string Make { get; set; }
            public required string Model { get; set; }
            public required string serialNumber { get; set; }
            public DateTime? perchaseDate { get; set; }
        }
        public class NonComputer
        {
            public required string deviceName { get; set; }
            public required string Make { get; set; }
            public required string Model { get; set; }
            public required string serialNumber { get; set; }
            public DateTime? perchaseDate { get; set; }
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
                        perchaseDate = worksheet.Cells[i, 5].Text != string.Empty ? Convert.ToDateTime(worksheet.Cells[i, 5].Text) : null
                    };

                    records.Add(record);
                }
            }
            Inventory.ItemsSource = records;
        }

        // make optional parameters so callers can pass fewer args
        public void ComputerDatabase(string connectionString, bool isConnected, DateTime? PerchaseDate, string DeviceName = "", string Make = "", string Model = "", string SerialNumber = "", bool submitSearchButton = false)
        {
            if (!submitSearchButton)
            {
                bool perchaseDateBool = false;
                bool deviceNameBool = false;
                bool makeBool = false;
                bool modelBool = false;
                bool serialNumberBool = false;

                if (!PerchaseDate.HasValue) perchaseDateBool = false; PerchaseDate = null;
                if (string.IsNullOrWhiteSpace(DeviceName)) deviceNameBool = false; DeviceName = "";
                if (string.IsNullOrWhiteSpace(Make)) makeBool = false; Make = "";
                if (string.IsNullOrWhiteSpace(Model)) modelBool = false; Model = "";
                if (string.IsNullOrWhiteSpace(SerialNumber)) serialNumberBool = false; SerialNumber = "";
            }

            var records = new List<Computer>();
            if (!submitSearchButton)
            {
                using (SqlConnection connect = new SqlConnection(connectionString))
                {
                    if (isConnected)
                    {
                        try
                        {
                            connect.Open();
                            string query = $"SELECT * FROM assetTracker";
                            SqlCommand command = new SqlCommand(query, connect);
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var record = new Computer
                                    {
                                        deviceName = reader.IsDBNull(reader.GetOrdinal("Device Name")) ? string.Empty : reader.GetString(reader.GetOrdinal("Device Name")),
                                        Make = reader.IsDBNull(reader.GetOrdinal("Make")) ? string.Empty : reader.GetString(reader.GetOrdinal("Make")),
                                        Model = reader.IsDBNull(reader.GetOrdinal("Model")) ? string.Empty : reader.GetString(reader.GetOrdinal("Model")),
                                        serialNumber = reader.IsDBNull(reader.GetOrdinal("Serial Number")) ? string.Empty : reader.GetString(reader.GetOrdinal("Serial Number")),
                                        perchaseDate = reader["Purchase Date"] != DBNull.Value ? Convert.ToDateTime(reader["Purchase Date"]) : null
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
                    else MessageBox.Show("data not retrieved", "SQL Data Retrieval");
                }
                defaultLoop = false;
                MessageBox.Show("Initinal data retrieved", "SQL Data Retrieval");
            }
            else
            {
                using (SqlConnection connect = new SqlConnection(connectionString))
                {
                    if (isConnected)
                    {
                        try
                        {
                            connect.Open();
                            string query = $"SELECT {DeviceName}, {Make}, {Model}, {SerialNumber}, {PerchaseDate} FROM assetTracker";
                            SqlCommand command = new SqlCommand(query, connect);
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var record = new Computer
                                    {
                                        deviceName = reader.IsDBNull(reader.GetOrdinal("Device Name")) ? string.Empty : reader.GetString(reader.GetOrdinal("Device Name")),
                                        Make = reader.IsDBNull(reader.GetOrdinal("Make")) ? string.Empty : reader.GetString(reader.GetOrdinal("Make")),
                                        Model = reader.IsDBNull(reader.GetOrdinal("Model")) ? string.Empty : reader.GetString(reader.GetOrdinal("Model")),
                                        serialNumber = reader.IsDBNull(reader.GetOrdinal("Serial Number")) ? string.Empty : reader.GetString(reader.GetOrdinal("Serial Number")),
                                        perchaseDate = reader["Purchase Date"] != DBNull.Value ? Convert.ToDateTime(reader["Purchase Date"]) : null
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
                    else MessageBox.Show("data not retrieved", "SQL Data Retrieval");
                }
            }
            Inventory.ItemsSource = records;
        }

        private void SubmitSearch_Click(object sender, RoutedEventArgs e)
        {
            bool submitSearchButton = true;
            string connectionString = @"server=localhost\SQLEXPRESS;database=data;Trusted_Connection=True;TrustServerCertificate=True;";
            string deviceName = DeviceName.Text;
            string serialNumber = SerialNumber.Text;
            string make = Make.Text;
            string model = Model.Text;
            DateTime? perchaseDate = PurchaseDate.SelectedDate;
            if (string.IsNullOrWhiteSpace(deviceName) || string.IsNullOrWhiteSpace(serialNumber) || string.IsNullOrWhiteSpace(make) || string.IsNullOrWhiteSpace(model) || !PurchaseDate.SelectedDate.HasValue)
            {
                ComputerDatabase(connectionString, isConnected, perchaseDate, deviceName, serialNumber, make, model, submitSearchButton);
                //Inventory.ItemsSource = records;
                MessageBox.Show($"Searching for device: {deviceName}, Serial: {serialNumber}, Make: {make}, Model: {model}, date: {perchaseDate?.ToShortDateString()}");
            }
            submitSearchButton = false;
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
                        isConnected = true;
                    }
                    else
                    {
                        StatusLight.Fill = Brushes.Orange;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Couldn't connect to DATABASE: {ex.Message}", "DATABASE CONNECTION");
                    StatusLight.Fill = Brushes.Red;
                    isConnected = false;
                }
            };
        }

        private void ChooseOptions(object sender, EventArgs e)
        {
            string connectionString = @"server=localhost\SQLEXPRESS;database=data;Trusted_Connection=True;TrustServerCertificate=True;";
            MessageBoxResult result = MessageBox.Show("Would you like to connect using SQL??", "Data Search using SQL", MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    //bool isConnected = false;
                    databaseConnect(connectionString);
                    ComputerDatabase(connectionString, isConnected, perchaseDate);
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
