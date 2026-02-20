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
        string connectionString = @"server=localhost\SQLEXPRESS;database=data;Trusted_Connection=True;TrustServerCertificate=True;";
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
            public string? perchaseDate { get; set; }
        }
        public class NonComputer
        {
            public required string deviceName { get; set; }
            public required string Make { get; set; }
            public required string Model { get; set; }
            public required string serialNumber { get; set; }
            public string? perchaseDate { get; set; }
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
                        perchaseDate = worksheet.Cells[i, 5].Text != string.Empty ? Convert.ToDateTime(worksheet.Cells[i, 5].Text).ToShortDateString() : null
                    };

                    records.Add(record);
                }
            }
            Inventory.ItemsSource = records;
        }

        // make optional parameters so callers can pass fewer args
        public void ComputerDatabase(string connectionString, bool isConnected, DateTime? PerchaseDateFrom, DateTime? PerchaseDateTo, string DeviceName = "", string SerialNumber = "", string Make = "", string Model = "", bool submitSearchButton = false)
        {
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
                                        perchaseDate = reader["Purchase Date"] != DBNull.Value ? Convert.ToDateTime(reader["Purchase Date"]).ToShortDateString() : null
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
                MessageBox.Show("Initinal data retrieved", "SQL Data Retrieval");
                Inventory.ItemsSource = records;
            }
            else
            {
                using (SqlConnection connect = new SqlConnection(connectionString))
                {
                    if (isConnected)
                    {
                        var quaryConditions = new List<string>();
                        using (SqlCommand quaryCommand = new SqlCommand())
                        {
                            if (!string.IsNullOrWhiteSpace(DeviceName))
                            {
                                quaryConditions.Add($"[device name] like @DeviceName");
                                quaryCommand.Parameters.AddWithValue("@DeviceName", $"%{DeviceName}%");
                            }
                            if (!string.IsNullOrWhiteSpace(Make))
                            {
                                quaryConditions.Add($"make like @Make");
                                quaryCommand.Parameters.AddWithValue("@Make", $"%{Make}%");
                            }
                            if (!string.IsNullOrWhiteSpace(Model))
                            {
                                quaryConditions.Add($"model like @Model");
                                quaryCommand.Parameters.AddWithValue("@Model", $"%{Model}%");
                            }
                            if (!string.IsNullOrWhiteSpace(SerialNumber))
                            {
                                quaryConditions.Add($"[serial number] like @SerialNumber");
                                quaryCommand.Parameters.AddWithValue("@SerialNumber", $"%{SerialNumber}%");
                            }
                            if (PerchaseDateFrom.HasValue)
                            {
                                quaryConditions.Add($"[purchase date] like @PerchaseDateFrom");
                                quaryCommand.Parameters.AddWithValue("@PerchaseDateFrom", $"%{PerchaseDateFrom.Value}%");
                            }
                            if (PerchaseDateTo.HasValue)
                            {
                                quaryConditions.Add($"[purchase date] like @PerchaseDateTo");
                                quaryCommand.Parameters.AddWithValue("@PerchaseDateTo", $"%{PerchaseDateTo.Value}%");
                            }
                            string baseQuary = quaryConditions.Count > 0 ? "WHERE " + string.Join(" AND ", quaryConditions) : string.Empty;
                            quaryCommand.CommandText = $"SELECT [device name], make, model, [serial number], [purchase date] FROM assetTracker {baseQuary}";
                            quaryCommand.Connection = connect;
                            try
                            {
                                connect.Open();
                                using (SqlDataReader reader = quaryCommand.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        var record = new Computer
                                        {
                                            deviceName = reader.IsDBNull(reader.GetOrdinal("Device Name")) ? string.Empty : reader.GetString(reader.GetOrdinal("Device Name")),
                                            Make = reader.IsDBNull(reader.GetOrdinal("Make")) ? string.Empty : reader.GetString(reader.GetOrdinal("Make")),
                                            Model = reader.IsDBNull(reader.GetOrdinal("Model")) ? string.Empty : reader.GetString(reader.GetOrdinal("Model")),
                                            serialNumber = reader.IsDBNull(reader.GetOrdinal("Serial Number")) ? string.Empty : reader.GetString(reader.GetOrdinal("Serial Number")),
                                            perchaseDate = reader["Purchase Date"] != DBNull.Value ? Convert.ToDateTime(reader["Purchase Date"]).ToShortDateString() : null
                                        };
                                        records.Add(record);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error retrieving data: {ex.Message}");
                            }
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
            //string connectionString = @"server=localhost\SQLEXPRESS;database=data;Trusted_Connection=True;TrustServerCertificate=True;";
            string deviceName = DeviceName.Text;
            string serialNumber = SerialNumber.Text;
            string make = Make.Text;
            string model = Model.Text;
            DateTime? perchaseDateFrom = PurchaseDate.SelectedDate;
            DateTime? perchaseDateTo = PurchaseDateTo.SelectedDate;
            if (string.IsNullOrWhiteSpace(deviceName) || string.IsNullOrWhiteSpace(serialNumber) || string.IsNullOrWhiteSpace(make) || string.IsNullOrWhiteSpace(model) || !PurchaseDate.SelectedDate.HasValue || !PurchaseDateTo.SelectedDate.HasValue)
            {
                ComputerDatabase(connectionString, isConnected, perchaseDateFrom, perchaseDateTo, deviceName, serialNumber, make, model, submitSearchButton);
                //Inventory.ItemsSource = records;

                //string message = $"Searched for device: {deviceName}, Serial: {serialNumber}, Make: {make}, Model: {model}, date: {perchaseDate?.ToShortDateString()}";
                var messageOptions = new List<string>();

                if (string.IsNullOrWhiteSpace(deviceName))
                {
                    deviceName = "None";
                    messageOptions.Add($"Device Name: {deviceName}");
                }
                else
                {
                    messageOptions.Add($"Device Name: {deviceName}");
                }
                if (!perchaseDateFrom.HasValue)
                {
                    string perchaseDateStringFrom = "None";
                    messageOptions.Add($"Purchase Date From: {perchaseDateStringFrom}");
                }
                else
                {
                    messageOptions.Add($"Purchase Date From: {perchaseDateFrom.ToString}");
                }
                if (!perchaseDateTo.HasValue)
                {
                    string perchaseDateStringTo = "None";
                    messageOptions.Add($"Purchase Date From: {perchaseDateStringTo}");
                }
                else
                {
                    messageOptions.Add($"Purchase Date To: {perchaseDateTo.ToString}");
                }
                if (string.IsNullOrWhiteSpace(serialNumber))
                {
                    serialNumber = "None";
                    messageOptions.Add($"Serial Number: {serialNumber}");
                }
                else
                {
                    messageOptions.Add($"Serial Number: {serialNumber}");
                }
                if (string.IsNullOrWhiteSpace(make))
                {
                    make = "None";
                    messageOptions.Add($"Make: {make}");
                }
                else
                {
                    messageOptions.Add($"Make: {make}");
                }
                if (string.IsNullOrWhiteSpace(model))
                {
                    model = "None";
                    messageOptions.Add($"Model: {model}");
                }
                else
                {
                    messageOptions.Add($"Model: {model}");
                }
                string message = $"Searched for: {string.Join(", ", messageOptions)}";
                MessageBox.Show(message);
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
