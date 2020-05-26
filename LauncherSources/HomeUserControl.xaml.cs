using AramisLauncher.Common;
using AramisLauncher.Download;
using AramisLauncher.Forge;
using AramisLauncher.JSON;
using AramisLauncher.Minecraft;
using AramisLauncher.Package;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AramisLauncher
{
    /// <summary>
    /// Logique d'interaction pour HomeUserControl.xaml
    /// </summary>
    public partial class HomeUserControl : UserControl
    {
        private WebClient webClient = new WebClient();

        private Thread thread = new Thread(new ParameterizedThreadStart(ExecuteInBackground));
        private static PackageConfiguration packageConfiguration;
        private static List<PackageStyle> packageStyles = new List<PackageStyle>();

        public static Button downloadButtonStatic;
        public static ProgressBar downloadProgress;
        public static TextBlock downloadDescriptorStatic;
        public static ListView listViewStatic;
        public HomeUserControl()
        {
            InitializeComponent();

            downloadProgress = (ProgressBar)FindName("downloadProgression");
            downloadDescriptorStatic = (TextBlock)FindName("downloadDescriptor");
            downloadButtonStatic = (Button)FindName("downloadButton");
            listViewStatic = (ListView)FindName("listView");

            /* GetPackage info */
            packageConfiguration = PackageConfiguration.FromJson(webClient.DownloadString(CommonData.packageInfoJsonURL));
            foreach(Package.Package package in packageConfiguration.Packages)
            {
                /* Get the url of package */
                string currentUrl = CommonData.packageInfoBaseURL + package.PackageName.ToLower() + "/" + package.PackageName.ToLower() + ".png";
                string currentStyleUrl = CommonData.packageInfoBaseURL + package.PackageName.ToLower() + "/" + "style.json";

                packageStyles.Add(PackageStyle.FromJson(webClient.DownloadString(currentStyleUrl)));

                /* Get image stream of package */
                BitmapImage bi3 = new BitmapImage();
                bi3.BeginInit();
                bi3.UriSource = new Uri(currentUrl, UriKind.Absolute);
                bi3.EndInit();

                /* Create image widget based on image streamed */
                Image image = new Image();
                image.Source = bi3;
                image.Height = 50;
                image.Width = 50;

                /* Add it to the list */
                ListViewItem listViewItem = new ListViewItem();
                listViewItem.Style = (Style)FindResource("ListViewContainerStyle");
                listViewItem.Content = image;
                listViewItem.MouseEnter += ListViewItem_MouseEnter;
                listViewItem.MouseLeave += ListViewItem_MouseLeave;
                listView.Items.Add(listViewItem);
            }
            listView.SelectedItem = listView.Items.GetItemAt(0);
        }

        private void ListViewItem_MouseLeave(object sender, MouseEventArgs e)
        {
            packageInformationPopup.IsOpen = false;
        }

        private void ListViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            packageInformationPopup.PlacementTarget = (sender as UIElement);
            packageInformationPopup.Placement = PlacementMode.Right;
            packageInformationPopup.IsOpen = true;
            packageInformation.PopupText.Text = packageConfiguration.Packages[listView.Items.IndexOf(sender)].PackageName + "\nVersion : " + packageConfiguration.Packages[listView.Items.IndexOf(sender)].Version;
        }

        private void downloadButton_Click(object sender, RoutedEventArgs e)
        {
            switch (thread.ThreadState)
            {
                case ThreadState.Unstarted:
                    (sender as Button).Content = "Stopper le téléchargement";
                    thread.Start(listView.SelectedIndex);
                    break;
                case ThreadState.Stopped:
                case ThreadState.Aborted:
                    thread = new Thread(ExecuteInBackground);
                    thread.IsBackground = true;
                    (sender as Button).Content = "Stopper le téléchargement";
                    thread.Start(listView.SelectedIndex);
                    break;
                case ThreadState.Running:
                case ThreadState.WaitSleepJoin:
                case ThreadState.Background:
                    (sender as Button).Content = packageStyles[listView.SelectedIndex].DownloadButtonText;
                    downloadDescriptor.Text = "Stoppé";
                    downloadProgression.Value = 0;
                    thread.Abort();
                    break;
                default:
                    break;
            }
        }

        private static void ExecuteInBackground(object index)
        {
            CommonData.packageName = packageConfiguration.Packages[(int)index].PackageName.ToLower();
            CommonData.packageVersion = packageConfiguration.Packages[(int)index].Version;
            CommonData.packageServerAddress = packageConfiguration.Packages[(int)index].ServerAddress;
            CommonData.updatePackageName();
            ManifestManager.GetAllManifests();
            if (DownloadManager.startDownload())
            {
                ForgeInstaller.installForge();
                /* start the selected version */
                MinecraftManager.StartMinecraft();
                ChangeDownloadButtonContent(null);
                ChangeDownloadButtonVisibility(Visibility.Hidden);
            }
            else
            {
                MessageBox.Show("Erreur lors du téléchargement...", "Erreur", MessageBoxButton.OK);
                ChangeDownloadButtonContent(null);
            }
            ChangeDownloadButtonContent(null);
            ChangeDownloadButtonVisibility(Visibility.Hidden);
        }
        
        public static void ChangeDownloadButtonContent(string newContent)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if(newContent == null)
                {
                    downloadButtonStatic.Content = packageStyles[listViewStatic.SelectedIndex].DownloadButtonText;
                }
                else
                {
                    downloadButtonStatic.Content = newContent;
                }
            });
        }

        public static void ChangeDownloadButtonVisibility(Visibility visibility)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                downloadButtonStatic.Visibility = visibility;
            });
        }

        public static void ChangeDownLoadDescriptor(string newValue)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                downloadDescriptorStatic.Text = newValue;
            });
        }

        public static void ChangeProgressBarValue(double newValue)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                downloadProgress.Value = newValue;
            });
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = listView.Items.IndexOf(e.AddedItems[0]);
            string selectedPackageName = packageConfiguration.Packages[index].PackageName.ToLower();
            /* Get background color */
            backgroundBrush.ImageSource = new BitmapImage(new Uri(CommonData.packageInfoBaseURL + selectedPackageName + "/background.png"));
            byte a = packageStyles[index].DownloadButtonColor[0];
            byte r = packageStyles[index].DownloadButtonColor[1];
            byte g = packageStyles[index].DownloadButtonColor[2];
            byte b = packageStyles[index].DownloadButtonColor[3];
            SolidColorBrush solidColorBrush = new SolidColorBrush(Color.FromArgb(a, r, g, b));
            downloadProgress.Foreground = solidColorBrush;
            downloadButton.Background = solidColorBrush;
            downloadButton.Content = packageStyles[index].DownloadButtonText;
            /* Get news */
            using (MemoryStream memoryStream = new MemoryStream(webClient.DownloadData(CommonData.packageInfoBaseURL + selectedPackageName + "/actualites.rtf")))
            {
                Actualities.Selection.Load(memoryStream, DataFormats.Rtf);
            }
        }
    }
}
