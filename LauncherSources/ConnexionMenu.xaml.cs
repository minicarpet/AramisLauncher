using AramisLauncher.Common;
using AramisLauncher.MojangAuth;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Threading;

namespace AramisLauncher
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class ConnexionMenu : Window
    {
        public ConnexionMenu()
        {
            InitializeComponent();

            /* Check if user is connected */
            CommonData.getLauncherProfile();
            if (CommonData.launcherProfileJson.authenticationDatabase != null)
            {
                /* file successfully loaded and profile stored */
                if(true == Authenticator.ValidateToken(CommonData.launcherProfileJson.authenticationDatabase.accessToken, CommonData.launcherProfileJson.authenticationDatabase.clientToken))
                {
                    /* Already connected */
                    LauncherMainMenu launcherMainMenu = new LauncherMainMenu();
                    launcherMainMenu.Show();
                    Close();
                }
                else
                {
                    /* We couldn't validate the token again, user needs to reconnect */
                    StatusText.Visibility = Visibility.Visible;
                    Task.Delay(2000).ContinueWith(_ =>
                    {
                        Dispatcher.Invoke(() =>
                        {
                            StatusText.Visibility = Visibility.Hidden;
                        });
                    });
                }
            }
        }

        private void LeaveButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void PassWord_GotFocus(object sender, RoutedEventArgs e)
        {
            if(PassWord.Password == "Password")
                PassWord.Clear();
        }

        private void UserName_GotFocus(object sender, RoutedEventArgs e)
        {
            if(UserName.Text == "Username")
                UserName.Clear();
        }

        private void ConnexionButton_Click(object sender, RoutedEventArgs e)
        {
            /* Try to connect user to Mojang account */
            if (true == Authenticator.AuthenticateToMinecraft(UserName.Text, PassWord.Password))
            {
                StatusText.Visibility = Visibility.Collapsed;
                LauncherMainMenu launcherMainMenu = new LauncherMainMenu();
                launcherMainMenu.Show();
                Close();
            }
            else
            {
                StatusText.Visibility = Visibility.Visible;
                Task.Delay(2000).ContinueWith(_ =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        StatusText.Visibility = Visibility.Hidden;
                    });
                });
            }
        }
    }
}
