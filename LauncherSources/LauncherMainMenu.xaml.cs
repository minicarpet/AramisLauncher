using AramisLauncher.Common;
using AramisLauncher.Updater;
using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.IO;
using System.Linq;
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
using System.Windows.Shapes;

namespace AramisLauncher
{
    /// <summary>
    /// Logique d'interaction pour LauncherMainMenu.xaml
    /// </summary>
    public partial class LauncherMainMenu : Window
    {
        static private HomeUserControl homeUserControl;
        static private ProfileUserControl profileUserControl = new ProfileUserControl();
        static private SettingsUserMenu settingsUserMenu = new SettingsUserMenu();

        private UpdaterManager updaterManager;

        public LauncherMainMenu()
        {
            InitializeComponent();
            homeUserControl = homeUserControlDefault;

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                launcherVersionText.Text += " " + ApplicationDeployment.CurrentDeployment.CurrentVersion;
                if (ApplicationDeployment.CurrentDeployment.IsFirstRun)
                {
                    /* Show release notes */
                    MessageBox.Show("NEW VERSION", "What's new", MessageBoxButton.OK);

                    if (Directory.Exists(CommonData.aramisFolder))
                    {
                        Directory.Delete(CommonData.aramisFolder);
                    }
                    if (Directory.Exists(CommonData.appDataFolder + ".freebuild"))
                    {
                        Directory.Delete(CommonData.appDataFolder + ".freebuild");
                    }
                    if (Directory.Exists(CommonData.appDataFolder + ".aramiscraft"))
                    {
                        Directory.Delete(CommonData.appDataFolder + ".aramiscraft");
                    }
                    if (Directory.Exists(CommonData.appDataFolder + ".eventis"))
                    {
                        Directory.Delete(CommonData.appDataFolder + ".eventis");
                    }
                    if (Directory.Exists(CommonData.appDataFolder + ".pixelmon"))
                    {
                        Directory.Delete(CommonData.appDataFolder + ".pixelmon");
                    }
                }
                updaterManager = new UpdaterManager(UpdateButton, updaterInformation);
            }
            else
            {
                launcherVersionText.Text += " " + "1.0.0.000";
            }
        }

        private void LeaveButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            Content.Children.Clear();
            Content.Children.Add(homeUserControl);
        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            Content.Children.Clear();
            Content.Children.Add(profileUserControl);
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            Content.Children.Clear();
            Content.Children.Add(settingsUserMenu);
        }

        private void UpdateButton_MouseEnter(object sender, MouseEventArgs e)
        {
            updaterInformationPopup.IsOpen = true;
        }

        private void UpdateButton_MouseLeave(object sender, MouseEventArgs e)
        {
            updaterInformationPopup.IsOpen = false;
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            updaterManager.BeginUpdate();
        }
    }
}
