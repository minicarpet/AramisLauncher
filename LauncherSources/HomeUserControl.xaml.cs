using AramisLauncher.Common;
using AramisLauncher.Package;
using MahApps.Metro.IconPacks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AramisLauncher
{
    /// <summary>
    /// Logique d'interaction pour HomeUserControl.xaml
    /// </summary>
    public partial class HomeUserControl : UserControl
    {
        private PackageConfiguration packageConfiguration;
        public HomeUserControl()
        {
            InitializeComponent();

            WebClient webClient = new WebClient();

            /* Get news */
            using (MemoryStream memoryStream = new MemoryStream(webClient.DownloadData(CommonData.actualityURL)))
            {
                Actualities.Selection.Load(memoryStream, DataFormats.Rtf);
            }

            /* GetPackage info */
            packageConfiguration = PackageConfiguration.FromJson(webClient.DownloadString(CommonData.packageInfoJsonURL));
            foreach(Package.Package package in packageConfiguration.Packages)
            {
                /* Get the url of package */
                string currentUrl = CommonData.packageInfoBaseURL + package.PackageName + "/" + package.PackageName + ".png";

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

        }
    }
}
