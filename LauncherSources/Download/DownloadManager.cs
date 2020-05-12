using AramisLauncher.Common;
using AramisLauncher.JSON;
using AramisLauncher.Minecraft;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace AramisLauncher.Download
{
    class DownloadManager
    {
        public class FileDownloadInformation
        {
            public string url;
            public string outputPath;
        }

        private static WebClient webClient = new WebClient();
        private static List<FileDownloadInformation> fileToDownload = new List<FileDownloadInformation>();

        public static List<string> nativesToExtract = new List<string>();
        public static List<string> classPathList = new List<string>();

        public DownloadManager()
        {

        }

        public static void startDownload()
        {
            InsalledPackage insalledPackage = null;
            try
            {
                insalledPackage = CommonData.launcherProfileJson.installedPackageVersion.Find(x => x.packageName == CommonData.packageName);
            }
            catch(Exception)
            {

            }

            if (insalledPackage != null && insalledPackage.packageVersion != CommonData.packageVersion)
            {
                /* package version does not correspond to the recorded one, delete files */
                foreach (string filePath in Directory.GetFiles(CommonData.packageFolder))
                {
                    if (!filePath.Contains("launcher_profile.json") && !filePath.Contains("launcher_log.txt"))
                        System.IO.File.Delete(filePath);
                }

                foreach (string path in Directory.GetDirectories(CommonData.packageFolder))
                {
                    Directory.Delete(path, true);
                }

                CommonData.launcherProfileJson.installedPackageVersion = null;
                CommonData.saveLauncherProfile();
            }

            /* download necessary files step by step */
            try
            {
                downloadAssets();
                downloadLibraries();
                downloadMinecraft();
                downloadForgeLibrairies();
                downloadForgeMods();
                downloadConfigs();
            }
            catch(Exception)
            {
            }
        }

        private static void downloadAssets()
        {
            int currentAsset = 0;

            /* Create directories */
            if (!Directory.Exists(CommonData.assetsIndexFolder))
            {
                Directory.CreateDirectory(CommonData.assetsIndexFolder);
            }

            System.IO.File.WriteAllText(CommonData.assetsIndexFolder + ManifestManager.minecraftVersionJson.Assets + ".json", ManifestManager.minecrafetVersionAssetsData);

            HomeUserControl.ChangeDownLoadDescriptor("Étape 1/12 : Vérification des assets...");
            HomeUserControl.ChangeProgressBarValue(0);
            fileToDownload.Clear();
            ManifestManager.assetsInformation.ForEach(delegate (AssetInformation assetInformation)
            {
                FileDownloadInformation fileDownloadInformation = new FileDownloadInformation();
                string assetPath = CommonData.assetsObjectFolder + assetInformation.GetFolder() + "/" + assetInformation.Hash;
                /* Check if object were already download */
                if (System.IO.File.Exists(assetPath))
                {
                    /* Check sha of file */
                    if (assetInformation.Hash != Metadata.GetSha1(assetPath))
                    {
                        /* File sha is incorrect */
                        fileDownloadInformation.url = "http://resources.download.minecraft.net/" + assetInformation.GetFolder() + "/" + assetInformation.Hash;
                        fileDownloadInformation.outputPath = assetPath;
                        fileToDownload.Add(fileDownloadInformation);
                    }
                }
                else
                {
                    fileDownloadInformation.url = "http://resources.download.minecraft.net/" + assetInformation.GetFolder() + "/" + assetInformation.Hash;
                    fileDownloadInformation.outputPath = assetPath;
                    fileToDownload.Add(fileDownloadInformation);
                }

                HomeUserControl.ChangeProgressBarValue(++currentAsset * 100.0 / ManifestManager.assetsInformation.Count);
            });

            /* download assets */
            currentAsset = 0;
            HomeUserControl.ChangeDownLoadDescriptor("Étape 2/12 : Téléchargement des assets...");
            HomeUserControl.ChangeProgressBarValue(0);
            fileToDownload.ForEach(delegate (FileDownloadInformation assetFile)
            {
                /* Create dir if not exist */
                if (!Directory.Exists(Path.GetDirectoryName(assetFile.outputPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(assetFile.outputPath));
                }

                /* Download file asset */
                webClient.DownloadFile(assetFile.url, assetFile.outputPath);

                HomeUserControl.ChangeProgressBarValue(++currentAsset * 100.0 / fileToDownload.Count);
            });
        }

        private static void downloadLibraries()
        {
            int currentAsset = 0;

            classPathList.Clear();
            nativesToExtract.Clear();
            fileToDownload.Clear();

            /* Create directories if needed */
            if (!Directory.Exists(CommonData.libraryFolder))
            {
                Directory.CreateDirectory(CommonData.libraryFolder);
            }

            HomeUserControl.ChangeDownLoadDescriptor("Étape 3/12 : Vérification des Librairies...");
            HomeUserControl.ChangeProgressBarValue(0);
            currentAsset = 0;
            foreach (Library library in ManifestManager.minecraftVersionJson.Libraries)
            {
                bool libraryNeeded = false;

                /* first check if this lib is needed */
                if (library.Rules != null)
                {
                    foreach (LibraryRule libraryRule in library.Rules)
                    {
                        if (libraryRule.Action == JSON.Action.Allow)
                        {
                            if (libraryRule.Os == null)
                            {
                                libraryNeeded = true;
                            }
                        }
                    }
                }
                else
                {
                    libraryNeeded = true;
                }

                if (libraryNeeded)
                {
                    System.Uri libraryUrl = null;
                    string filePath = "";
                    string sha = "";

                    /* check if file is native or not */
                    if (library.Downloads.Classifiers != null)
                    {
                        /* File is native */
                        if (library.Downloads.Classifiers.NativesWindows != null)
                        {
                            /* File is windows native */
                            libraryUrl = library.Downloads.Classifiers.NativesWindows.Url;
                            filePath = CommonData.libraryFolder + library.Downloads.Classifiers.NativesWindows.Path;
                            sha = library.Downloads.Classifiers.NativesWindows.Sha1;
                            nativesToExtract.Add(filePath);
                        }
                    }
                    else
                    {
                        /* File is library */
                        libraryUrl = library.Downloads.Artifact.Url;
                        filePath = CommonData.libraryFolder + library.Downloads.Artifact.Path;
                        sha = library.Downloads.Artifact.Sha1;
                        classPathList.Add(filePath);
                    }

                    if (libraryUrl != null)
                    {
                        FileDownloadInformation fileDownloadInformation = new FileDownloadInformation();
                        /* Check if version were already download */
                        if (System.IO.File.Exists(filePath))
                        {
                            /* Check sha of file */
                            if (sha != Metadata.GetSha1(filePath))
                            {
                                /* File sha is incorrect, download again */
                                fileDownloadInformation.url = libraryUrl.AbsoluteUri;
                                fileDownloadInformation.outputPath = filePath;
                                fileToDownload.Add(fileDownloadInformation);
                            }
                        }
                        else
                        {
                            fileDownloadInformation.url = libraryUrl.AbsoluteUri;
                            fileDownloadInformation.outputPath = filePath;
                            fileToDownload.Add(fileDownloadInformation);
                        }
                    }
                }
                HomeUserControl.ChangeProgressBarValue(++currentAsset * 100.0 / ManifestManager.minecraftVersionJson.Libraries.Length);
            }

            HomeUserControl.ChangeDownLoadDescriptor("Étape 4/12 : Téléchargement des Librairies...");
            HomeUserControl.ChangeProgressBarValue(0);
            currentAsset = 0;
            fileToDownload.ForEach(delegate (FileDownloadInformation libFile)
            {
                /* Create dir if not exist */
                if (!Directory.Exists(Path.GetDirectoryName(libFile.outputPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(libFile.outputPath));
                }

                /* Download file asset */
                webClient.DownloadFile(libFile.url, libFile.outputPath);

                HomeUserControl.ChangeProgressBarValue(++currentAsset * 100.0 / fileToDownload.Count);
            });
        }

        private static void downloadMinecraft()
        {
            fileToDownload.Clear();
            FileDownloadInformation gameDownloadInformation = new FileDownloadInformation();
            FileDownloadInformation logconfigDownloadInformation = new FileDownloadInformation();

            if (!Directory.Exists(CommonData.versionFolder + ManifestManager.minecraftVersionJson.Id))
            {
                Directory.CreateDirectory(CommonData.versionFolder + ManifestManager.minecraftVersionJson.Id);
            }

            HomeUserControl.ChangeDownLoadDescriptor("Étape 5/12 : Vérification des fichiers du jeu...");
            string gameFilePath = CommonData.versionFolder + ManifestManager.minecraftVersionJson.Id + "/" + ManifestManager.minecraftVersionJson.Id + ".jar";
            gameDownloadInformation.url = ManifestManager.minecraftVersionJson.Downloads.Client.Url.AbsoluteUri;
            gameDownloadInformation.outputPath = gameFilePath;

            /* Check if version were already download */
            if (System.IO.File.Exists(gameFilePath))
            {
                /* Check sha of file */
                if (ManifestManager.minecraftVersionJson.Downloads.Client.Sha1 != Metadata.GetSha1(gameFilePath))
                {
                    /* File sha is incorrect */
                    fileToDownload.Add(gameDownloadInformation);
                }
            }
            else
            {
                fileToDownload.Add(gameDownloadInformation);
            }

            if (ManifestManager.minecraftVersionJson.Logging.Client.File != null)
            {
                if (!Directory.Exists(CommonData.assetsLogConfigFolder))
                {
                    Directory.CreateDirectory(CommonData.assetsLogConfigFolder);
                }

                logconfigDownloadInformation.url = ManifestManager.minecraftVersionJson.Logging.Client.File.Url.AbsoluteUri;
                logconfigDownloadInformation.outputPath = CommonData.assetsLogConfigFolder + ManifestManager.minecraftVersionJson.Logging.Client.File.Id;

                if (System.IO.File.Exists(logconfigDownloadInformation.outputPath))
                {
                    /* Check sha of file */
                    if (ManifestManager.minecraftVersionJson.Logging.Client.File.Sha1 != Metadata.GetSha1(logconfigDownloadInformation.outputPath))
                    {
                        fileToDownload.Add(logconfigDownloadInformation);
                    }
                }
                else
                {
                    fileToDownload.Add(logconfigDownloadInformation);
                }
            }

            HomeUserControl.ChangeDownLoadDescriptor("Étape 6/12 : Téléchargement des fichiers du jeu...");
            /* record json into this same path if not exist */
            string gameJSONFilePath = CommonData.versionFolder + ManifestManager.minecraftVersionJson.Id + "/" + ManifestManager.minecraftVersionJson.Id + ".json";

            if (!System.IO.File.Exists(gameJSONFilePath))
            {
                /* record the json file */
                System.IO.File.WriteAllText(gameJSONFilePath, ManifestManager.manifestVersionData);
            }

            DownloadProgressChangedEventHandler eventHandler = new DownloadProgressChangedEventHandler(DownloadProgressCallback);
            fileToDownload.ForEach(delegate (FileDownloadInformation libFile)
            {
                /* Create dir if not exist */
                if (!Directory.Exists(Path.GetDirectoryName(libFile.outputPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(libFile.outputPath));
                }

                /* Download file asset */
                webClient.DownloadProgressChanged += eventHandler;
                webClient.DownloadFileAsync(new System.Uri(libFile.url), libFile.outputPath);
                while (webClient.IsBusy) { }
            });

            webClient.DownloadProgressChanged -= eventHandler;
        }

        private static void downloadForgeLibrairies()
        {
            fileToDownload.Clear();
            HomeUserControl.ChangeDownLoadDescriptor("Étape 7/12 : Vérification des fichiers forge...");
            HomeUserControl.ChangeProgressBarValue(0);
            int currentAsset = 0;
            foreach (ForgeLibrary library in ManifestManager.forgeVersionJson.Libraries)
            {
                FileDownloadInformation fileDownloadInformation = new FileDownloadInformation();

                string[] tempSplitted = library.Name.Split(':');
                if (tempSplitted.Length == 3)
                {
                    string completedFilePath = tempSplitted[0].Replace('.', '/') + "/" + tempSplitted[1] + "/" + tempSplitted[2] + "/" + tempSplitted[1] + "-" + tempSplitted[2] + ".jar";

                    if (library.Url != null)
                    {
                        fileDownloadInformation.url = library.Url.AbsoluteUri + completedFilePath;
                    }
                    else
                    {
                        fileDownloadInformation.url = "https://libraries.minecraft.net/" + completedFilePath;
                    }
                    fileDownloadInformation.outputPath = CommonData.libraryFolder + completedFilePath;
                    classPathList.Add(fileDownloadInformation.outputPath);

                    if (System.IO.File.Exists(fileDownloadInformation.outputPath))
                    {
                        System.IO.File.Delete(fileDownloadInformation.outputPath);
                    }

                    fileToDownload.Add(fileDownloadInformation);
                }

                HomeUserControl.ChangeProgressBarValue(++currentAsset * 100.0 / ManifestManager.forgeVersionJson.Libraries.Length);
            }

            HomeUserControl.ChangeDownLoadDescriptor("Étape 8/12 : Téléchargement des fichiers forge...");
            HomeUserControl.ChangeProgressBarValue(0);
            currentAsset = 0;
            fileToDownload.ForEach(delegate (FileDownloadInformation libFile)
            {
                /* Create dir if not exist */
                if (!Directory.Exists(Path.GetDirectoryName(libFile.outputPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(libFile.outputPath));
                }

                /* Download file asset */
                webClient.DownloadFile(libFile.url, libFile.outputPath);

                HomeUserControl.ChangeProgressBarValue(++currentAsset * 100.0 / fileToDownload.Count);
            });
        }

        private static void downloadForgeMods()
        {
            fileToDownload.Clear();
            HomeUserControl.ChangeDownLoadDescriptor("Étape 9/12 : Vérification des mods forge...");
            HomeUserControl.ChangeProgressBarValue(0);
            int currentAsset = 0;
            foreach (InstalledAddon addon in ManifestManager.packageJson.InstalledAddons)
            {
                FileDownloadInformation fileDownloadInformation = new FileDownloadInformation();

                string filePath = CommonData.modsFolder + addon.InstalledFile.FileName;
                fileDownloadInformation.outputPath = filePath;
                fileDownloadInformation.url = addon.InstalledFile.DownloadUrl.AbsoluteUri;

                if (System.IO.File.Exists(filePath))
                {
                    /* Compare size */
                    if (new FileInfo(filePath).Length != addon.InstalledFile.FileLength)
                    {
                        /* Size is incorrect, add to download list */
                        fileToDownload.Add(fileDownloadInformation);
                    }
                }
                else
                {
                    /* add to download list */
                    fileToDownload.Add(fileDownloadInformation);
                }
            }

            int test = ManifestManager.packageJson.CachedScans.Length;

            HomeUserControl.ChangeDownLoadDescriptor("Étape 10/12 : Téléchargement des mods forge...");
            HomeUserControl.ChangeProgressBarValue(0);
            currentAsset = 0;
            fileToDownload.ForEach(delegate (FileDownloadInformation libFile)
            {
                /* Create dir if not exist */
                if (!Directory.Exists(Path.GetDirectoryName(libFile.outputPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(libFile.outputPath));
                }

                /* Download file asset */
                webClient.DownloadFile(libFile.url, libFile.outputPath);

                HomeUserControl.ChangeProgressBarValue(++currentAsset * 100.0 / fileToDownload.Count);
            });
        }

        private static void downloadConfigs()
        {
            fileToDownload.Clear();
            HomeUserControl.ChangeDownLoadDescriptor("Étape 11/12 : Vérification des configurations...");
            HomeUserControl.ChangeProgressBarValue(0);
            foreach (FileProperty file in ManifestManager.packageConfigurationJson.FileProperties)
            {
                FileDownloadInformation fileDownloadInformation = new FileDownloadInformation();

                string filePath = CommonData.scriptFolder + file.FileName;
                fileDownloadInformation.outputPath = filePath;
                fileDownloadInformation.url = CommonData.packageInfoBaseURL + CommonData.packageName + "/scripts/" + file.FileName;

                if (System.IO.File.Exists(filePath))
                {
                    /* Compare size */
                    if (new FileInfo(filePath).Length != file.FileSize)
                    {
                        /* Size is incorrect, add to download list */
                        fileToDownload.Add(fileDownloadInformation);
                    }
                }
                else
                {
                    /* add to download list */
                    fileToDownload.Add(fileDownloadInformation);
                }
            }

            HomeUserControl.ChangeDownLoadDescriptor("Étape 12/12 : Téléchargement des configurations...");
            HomeUserControl.ChangeProgressBarValue(0);
            int currentConfig = 0;
            fileToDownload.ForEach(delegate (FileDownloadInformation libFile)
            {
                /* Create dir if not exist */
                if (!Directory.Exists(Path.GetDirectoryName(libFile.outputPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(libFile.outputPath));
                }

                /* Download file asset */
                webClient.DownloadFile(libFile.url, libFile.outputPath);

                HomeUserControl.ChangeProgressBarValue(++currentConfig * 100.0 / fileToDownload.Count);
            });

            InsalledPackage newInstalledPackage = null;
            newInstalledPackage.packageName = CommonData.packageName;
            newInstalledPackage.packageVersion = CommonData.packageVersion;

            CommonData.launcherProfileJson.installedPackageVersion.Add(newInstalledPackage);
            CommonData.saveLauncherProfile();
        }

        private static void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e)
        {
            HomeUserControl.ChangeProgressBarValue(e.ProgressPercentage);
            // Displays the operation identifier, and the transfer progress.
            //System.Console.WriteLine("{0}    downloaded {1} of {2} bytes. {3} % complete...",
            //    (string)e.UserState,
            //    e.BytesReceived,
            //    e.TotalBytesToReceive,
            //    e.ProgressPercentage);
        }
    }
}
