using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Controls;

namespace UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a <see cref="Frame">.
    /// </summary>
    
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public class Person
        {
            public int id { get; set; }
            public string name { get; set; }
            public string department { get; set; }
        }

        public async void FindUserButton(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add(".csv");
            picker.FileTypeFilter.Add(".xlsx");

            Dictionary<string, int> headerMap = new();

            for (int col = 1; col <= columnCount; col++)
            {
                string header = worksheet.Cells[1, col].Text.Trim();
                if (!string.IsNullOrEmpty(header)) headerMap[header] = col;
            }

            for (int i = 2; i <= rowCount; i++)
            {
                var record = new Person();
                if (headerMap.TryGetValue("EmployeeID", out int idCol))
                {
                    record.id = worksheet.Cells[i, idCol].Text;
                }
                if (headerMap.TryGetValue("DisplayName", out int nameCol))
                {
                    record.name = worksheet.Cells[i, nameCol].Text;
                }
                if (headerMap.TryGetValue("Enabled", out int statusCol))
                {
                    string status = worksheet.Cells[i, statusCol].Text.ToLower();
                    record.department = bool.Parse(status);
                }

                records.Add(record);
            }

            var file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                var lines = await FileIO.ReadLinesAsync(file);

                var people = new List<Person>();

                for (int i = 1; i < lines.Count; i++)
                {
                    var parts = lines[i].Split(',');
                    if (parts.Length >= 3)
                    {
                        people.Add(new Person
                        {
                            id = int.Parse(parts[0]),
                            name = parts[1],
                            department = parts[2]
                        });
                    }
                }

                records.ItemsSource = people;
            }
        }

        private void ComputerSearchClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }
    }
}
