using AramisLauncher.Common;
using AramisLauncher.Download;
using AramisLauncher.JSON;
using AramisLauncher.Logger;
using AramisLauncher.Minecraft;
using System;
using System.Deployment.Application;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using static AramisLauncher.Minecraft.Authenticator;

namespace AramisLauncher
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static ProgressBar downloadProgress;
        public static Label downloadDescriptor;
        public static PasswordBox passwordUserBox;
        public static TextBox userNameBox;
        public static Label connectionStateLabel;
        public static Button connectionButton;
        public static WebBrowser webBrowser;
        public static Label versionLabel;
        public static Ellipse ellipse;

        public static WebClient webClient = new WebClient();

        private ManifestManager manifestManager;
        private DownloadManager donwloaderManager;
        private MinecraftManager minecraftManager;

        private Thread thread = new Thread(ExecuteInBackground);
        public MainWindow()
        {
            InitializeComponent();

            downloadProgress = (ProgressBar)FindName("downloadProgression");
            downloadDescriptor = (Label)FindName("downloadDescription");
            userNameBox = (TextBox)FindName("usernameBox");
            passwordUserBox = (PasswordBox)FindName("passwordBox");
            connectionStateLabel = (Label)FindName("ConnectionStateLabel");
            connectionButton = (Button)FindName("connectButton");
            webBrowser = (WebBrowser)FindName("webContent");
            versionLabel = (Label)FindName("version");
            ellipse = (Ellipse)FindName("ConnectionCircle");

            if (ApplicationDeployment.IsNetworkDeployed)
                versionLabel.Content += " " + ApplicationDeployment.CurrentDeployment.CurrentVersion;

            webBrowser.NavigateToString(webClient.DownloadString("https://raw.githubusercontent.com/minicarpet/AramisLauncher/master/Ressources/actualites/actualite.html"));

            CommonData.getLauncherProfile();

            if (CommonData.launcherProfileJson.authenticationDatabase != null)
            {
                /* file successfully loaded */
                ellipse.Fill = new SolidColorBrush(Colors.Green);
                userNameBox.Visibility = Visibility.Hidden;
                passwordUserBox.Visibility = Visibility.Hidden;
                connectionButton.Content = "Disconnect";
                connectionStateLabel.Content = "Connected as : " + CommonData.launcherProfileJson.authenticationDatabase.selectedProfile.name;
            }
            else
            {
                ellipse.Fill = new SolidColorBrush(Colors.Red);
                connectionStateLabel.Content = "Not connected.";
            }

            LoggerManager.log("Launcher started at " + DateTime.Now);

            thread.IsBackground = true;
            downloadProgress.Minimum = 0;
            downloadProgress.Maximum = 100;

            manifestManager = new ManifestManager();
            donwloaderManager = new DownloadManager();
            minecraftManager = new MinecraftManager();
        }

        private void downloadButton_Click(object sender, RoutedEventArgs e)
        {
            if (CommonData.launcherProfileJson.authenticationDatabase != null)
            {
                try
                {
                    bool processIsRunning = MinecraftManager.minecraftGame.HasExited;
                    if (processIsRunning)
                    {
                        switch (thread.ThreadState)
                        {
                            case ThreadState.Unstarted:
                                thread.Start();
                                break;
                            case ThreadState.Stopped:
                                thread = new Thread(ExecuteInBackground);
                                thread.IsBackground = true;
                                thread.Start();
                                break;
                            default:
                                break;
                        }
                    }
                }
                catch (InvalidOperationException)
                {
                    switch(thread.ThreadState)
                    {
                        case ThreadState.Background | ThreadState.Unstarted:
                            thread.Start();
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                MessageBox.Show("Merci de se connecter avant.");
            }
        }

        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            if(connectionButton.Content.ToString() == "Connect")
            {
                CommonData.setAuthenticateProfile(AuthenticateToMinecraft(userNameBox.Text, passwordUserBox.Password));
                if (CommonData.launcherProfileJson.authenticationDatabase != null)
                {
                    MessageBox.Show("Connected !");
                    CommonData.saveLauncherProfile();
                    ellipse.Fill = new SolidColorBrush(Colors.Green);
                    userNameBox.Visibility = Visibility.Hidden;
                    passwordUserBox.Visibility = Visibility.Hidden;
                    connectionButton.Content = "Disconnect";
                    connectionStateLabel.Content = "Connected as : " + CommonData.launcherProfileJson.authenticationDatabase.selectedProfile.name;
                }
                else
                {
                    MessageBox.Show("Error to connect !");
                }
            }
            else
            {
                ellipse.Fill = new SolidColorBrush(Colors.Red);
                userNameBox.Visibility = Visibility.Visible;
                passwordUserBox.Visibility = Visibility.Visible;
                passwordUserBox.Clear();
                connectionButton.Content = "Connect";
                connectionStateLabel.Content = "Not connected";
                CommonData.launcherProfileJson.authenticationDatabase = null;
                CommonData.saveLauncherProfile();
            }
        }

        public static void ChangeDownLoadDescriptor(string newValue)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                downloadDescriptor.Content = newValue;
            });
        }

        public static void ChangeProgressBarValue(double newValue)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                downloadProgress.Value = newValue;
            });
        }

        private static void ExecuteInBackground()
        {
            /* Download the selected version */
            DownloadManager.startDownload();
            /* start the selected version */
            MinecraftManager.StartMinecraft();
        }
    }
}
