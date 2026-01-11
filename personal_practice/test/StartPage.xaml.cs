using System;
using System.Collections.Generic;
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
using System.Security.Principal;
using System.DirectoryServices.AccountManagement;
using System.Text.RegularExpressions;

namespace test
{
    /// <summary>
    /// Interaction logic for StartPage.xaml
    /// </summary>
    public partial class StartPage : UserControl
    {
        public StartPage()
        {
            InitializeComponent();

            // Example: use the currently signed-in Windows user and look up AD details
            /*try
            {
                string winName = WindowsIdentity.GetCurrent().Name; // DOMAIN\username
                string sam = winName.Contains('\\') ? winName.Split('\\')[1] : winName;

                using (var ctx = new PrincipalContext(ContextType.Domain))
                {
                    var user = UserPrincipal.FindByIdentity(ctx, IdentityType.SamAccountName, sam);
                    if (user != null)
                    {
                        var info = new AdUserInfo
                        {
                            SamAccountName = user.SamAccountName ?? sam,
                            DisplayName = user.DisplayName,
                            Email = user.EmailAddress
                        };

                        foreach (var group in user.GetAuthorizationGroups().OfType<GroupPrincipal>())
                        {
                            if (!string.IsNullOrEmpty(user.SamAccountName))
                            {
                                info.Groups.Add(group.SamAccountName);
                            }
                        }
                        // store the UserPrincipal for app-wide use
                        Application.Current.Properties["CurrentUser"] = user;

                        // show minimal info as an example
                        MessageBox.Show($"Signed in as: {user.DisplayName} ({user.SamAccountName})\nEmail: {user.EmailAddress}", "AD user", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Windows account {winName} not found in AD.", "AD lookup");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"AD lookup failed: {ex.Message}", "AD lookup error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }*/
        }

        public void ComputerSearch(object sender, RoutedEventArgs e)
        {
            /*if (Application.Current.Properties.Contains("CurrentUser"))
            {
                var user = Application.Current.Properties["CurrentUser"] as UserPrincipal;
                if (user != null)
                {
                    var groups = user.GetAuthorizationGroups().OfType<GroupPrincipal>().Select(g => g.SamAccountName);
                    if (groups.Contains("Information Technology"))
                    {
                        ((MainWindow)Application.Current.MainWindow).NavigateTo(new HomePage());
                    }
                    else
                    {
                        MessageBox.Show("You do not have permission to access the app.", "Access Denied", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }*/
            ((MainWindow)Application.Current.MainWindow).NavigateTo(new HomePage());
        }
    }
    public class AdUserInfo
    {
        public string SamAccountName { get; init; } = "";
        public string? DisplayName { get; init; }
        public string? Email { get; init; }
        public List<string> Groups { get; init; } = new();
    }
}
