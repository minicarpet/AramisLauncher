using AramisLauncher.Common;
using AramisLauncher.Download;
using AramisLauncher.JSON;
using AramisLauncher.Logger;
using AramisLauncher.Minecraft;
using System;
using System.Deployment.Application;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
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
        public static Label pseudoLabel;
        public static Label connectionStatus;
        public static Button connectionButton;
        public static Label versionModpack;
        public static Label versionLauncher;
        public static Rectangle connectionStatusShape;
        public static Canvas canvasConnection;
        public static RichTextBox richTextBox;

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
            pseudoLabel = (Label)FindName("pseudo");
            connectionStatus = (Label)FindName("connection_status");
            connectionButton = (Button)FindName("connectButton");
            versionLauncher = (Label)FindName("version_launcher");
            versionModpack = (Label)FindName("version_modpack");
            connectionStatusShape = (Rectangle)FindName("connection_status_shape");
            canvasConnection = (Canvas)FindName("canvas_connection");
            richTextBox = (RichTextBox)FindName("actualites");

            if (ApplicationDeployment.IsNetworkDeployed)
                versionLauncher.Content += " " + ApplicationDeployment.CurrentDeployment.CurrentVersion;
            else
                versionLauncher.Content += " " + "1.0.0.0";

            using (MemoryStream memoryStream = new MemoryStream(webClient.DownloadData("https://raw.githubusercontent.com/minicarpet/AramisLauncher/master/Ressources/actualites/actualites.rtf")))
            {
                richTextBox.Selection.Load(memoryStream, DataFormats.Rtf);
            }

            CommonData.getLauncherProfile();

            if (CommonData.launcherProfileJson.authenticationDatabase != null)
            {
                /* file successfully loaded */
                ValidateToken(CommonData.launcherProfileJson.authenticationDatabase.accessToken, CommonData.launcherProfileJson.authenticationDatabase.clientToken);
            }
            else
            {
                connectionStatusShape.Fill = new SolidColorBrush(Colors.Red);
                pseudoLabel.Content = "";
                connectionStatus.Content = "Non connecté";
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
                switch (thread.ThreadState)
                {
                    case ThreadState.Background | ThreadState.Unstarted:
                        thread.Start();
                        break;
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
            else
            {
                MessageBox.Show("Merci de se connecter avant.");
            }
        }

        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            if(connectionButton.Content.ToString() == "Connexion")
            {
                AuthenticateToMinecraft(userNameBox.Text, passwordUserBox.Password);
                if (CommonData.launcherProfileJson.authenticationDatabase != null)
                {
                    ChangeConnectionState(true);
                }
                else
                {
                    MessageBox.Show("Error to connect !");
                }
            }
            else
            {
                ChangeConnectionState(false);
            }
        }

        public static void ChangeConnectionState(bool connected)
        {
            if(connected)
            {
                CommonData.saveLauncherProfile();
                connectionStatusShape.Fill = new SolidColorBrush(Colors.Green);
                userNameBox.Visibility = Visibility.Hidden;
                passwordUserBox.Visibility = Visibility.Hidden;
                connectionButton.Content = "Déconnexion";
                Canvas.SetTop(connectionButton, 66);
                canvasConnection.Height = 94;
                pseudoLabel.Content = CommonData.launcherProfileJson.authenticationDatabase.selectedProfile.name;
                connectionStatus.Content = "Connecté";
            }
            else
            {
                connectionStatusShape.Fill = new SolidColorBrush(Colors.Red);
                userNameBox.Text = "Adresse mail";
                userNameBox.Visibility = Visibility.Visible;
                passwordUserBox.Visibility = Visibility.Visible;
                passwordUserBox.Clear();
                Canvas.SetTop(connectionButton, 117);
                canvasConnection.Height = 147;
                connectionButton.Content = "Connexion";
                pseudoLabel.Content = "";
                connectionStatus.Content = "Non connecté";
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

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == System.Windows.Input.Key.Enter)
            {
                if(connectionButton.Content.ToString() == "Connexion")
                {
                    connectButton_Click(sender, new RoutedEventArgs());
                }
            }
        }

        private void usernameBox_GetFocus(object sender, RoutedEventArgs e)
        {
            if(userNameBox.Text == "Adresse mail")
            {
                userNameBox.Text = "";
            }
        }
    }
}
