using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Deployment.Application;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Linq;

namespace AramisLauncher.Updater
{
    class UpdaterManager
    {
        private Button updateButton;
        private UpdaterInformation updaterInformation;
        public UpdaterManager(Button button, UpdaterInformation newUpdaterInformation)
        {
            updateButton = button;
            updaterInformation = newUpdaterInformation;
            /* Configure callback */
            ApplicationDeployment.CurrentDeployment.CheckForUpdateCompleted += new CheckForUpdateCompletedEventHandler(ad_CheckForUpdateCompleted);
            ApplicationDeployment.CurrentDeployment.CheckForUpdateProgressChanged += new DeploymentProgressChangedEventHandler(ad_CheckForUpdateProgressChanged);
            ApplicationDeployment.CurrentDeployment.UpdateCompleted += new AsyncCompletedEventHandler(ad_UpdateCompleted);
            /* Indicate progress in the application's status bar. */
            ApplicationDeployment.CurrentDeployment.UpdateProgressChanged += new DeploymentProgressChangedEventHandler(ad_UpdateProgressChanged);

            /* When this class start ask for an update */
            ApplicationDeployment.CurrentDeployment.CheckForUpdateAsync();
        }

        public void BeginUpdate()
        {
            ApplicationDeployment.CurrentDeployment.UpdateAsync();
        }

        private void ad_CheckForUpdateProgressChanged(object sender, DeploymentProgressChangedEventArgs e)
        {
            /* In case I want to add text or progressBar status */
            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                updaterInformation.PopupText.Text = String.Format("Downloading: {0}. {1:D}K of {2:D}K downloaded.", GetProgressString(e.State), e.BytesCompleted / 1024, e.BytesTotal / 1024);
                updaterInformation.progressBar.Value = e.ProgressPercentage;
            });
        }

        private string GetProgressString(DeploymentProgressState state)
        {
            if (state == DeploymentProgressState.DownloadingApplicationFiles)
            {
                return "application files";
            }
            else if (state == DeploymentProgressState.DownloadingApplicationInformation)
            {
                return "application manifest";
            }
            else
            {
                return "deployment manifest";
            }
        }

        private void ad_CheckForUpdateCompleted(object sender, CheckForUpdateCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("ERROR: Could not retrieve new version of the application. Reason: \n" + e.Error.Message + "\nPlease report this error to the system administrator.");
                return;
            }
            else if (e.Cancelled == true)
            {
                MessageBox.Show("The update was cancelled.");
            }

            if (e.UpdateAvailable)
            {
                Dispatcher.CurrentDispatcher.Invoke(() =>
                {
                    updateButton.Visibility = Visibility.Visible;
                });
                if (e.IsUpdateRequired)
                {
                    BeginUpdate();
                }
            }
        }

        private void ad_UpdateProgressChanged(object sender, DeploymentProgressChangedEventArgs e)
        {
            String progressText = String.Format("{0:D}K out of {1:D}K downloaded - {2:D}% complete", e.BytesCompleted / 1024, e.BytesTotal / 1024, e.ProgressPercentage);
            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                updaterInformation.PopupText.Text = progressText;
                updaterInformation.progressBar.Value = e.ProgressPercentage;
            });
        }

        private void ad_UpdateCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show("The update of the application's latest version was cancelled.");
                return;
            }
            else if (e.Error != null)
            {
                MessageBox.Show("ERROR: Could not install the latest version of the application. Reason: \n" + e.Error.Message + "\nPlease report this error to the system administrator.");
                return;
            }

            MessageBoxResult dr = MessageBox.Show("The application has been updated. Restart? (If you do not restart now, the new version will not take effect until after you quit and launch the application again.)", "Restart Application", MessageBoxButton.OKCancel);
            if (MessageBoxResult.OK == dr)
            {
                string shortcutFile = GetShortcutPath();
                Process proc = new Process { StartInfo = { FileName = shortcutFile, UseShellExecute = true } };

                proc.Start();
                Application.Current.Shutdown();
            }
        }

        private static string GetShortcutPath()
        {
            return String.Format(@"{0}\{1}\{2}.appref-ms", Environment.GetFolderPath(Environment.SpecialFolder.Programs), GetPublisher(), GetDeploymentInfo().Name.Replace(".application", ""));
        }

        private static string GetPublisher()
        {
            XDocument xDocument;
            using (var memoryStream = new MemoryStream(AppDomain.CurrentDomain.ActivationContext.DeploymentManifestBytes))
            using (var xmlTextReader = new XmlTextReader(memoryStream))
                xDocument = XDocument.Load(xmlTextReader);

            if (xDocument.Root == null)
                return null;

            var description = xDocument.Root.Elements().First(e => e.Name.LocalName == "description");
            var publisher = description.Attributes().First(a => a.Name.LocalName == "publisher");
            return publisher.Value;
        }

        private static ApplicationId GetDeploymentInfo()
        {
            var appSecurityInfo = new System.Security.Policy.ApplicationSecurityInfo(AppDomain.CurrentDomain.ActivationContext);
            return appSecurityInfo.DeploymentId;
        }
    }
}
