using AramisLauncher.Common;
using AramisLauncher.JSON;
using AramisLauncher.Logger;
using AramisLauncher.Minecraft;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace AramisLauncher.Download
{
    public class FileDownloadInformation
    {
        public string url;
        public string outputPath;
    }
    class DownloadManager
    {
        public static string libraryFolder = CommonData.aramisFolder + "libraries/";

        public static List<string> nativesToExtract = new List<string>();

        private static List<FileDownloadInformation> fileToDownload = new List<FileDownloadInformation>();

        private static WebClient webClient = new WebClient();
        private static string assetsLogConfigFolder = CommonData.aramisFolder + "assets/log_configs/";
        private static string assetsIndexFolder = CommonData.aramisFolder + "assets/indexes/";
        private static string assetsObjectFolder = CommonData.aramisFolder + "assets/objects/";
        private static string versionFolder = CommonData.aramisFolder + "versions/";

        public static List<string> classPathList = new List<string>();

        public DownloadManager()
        {
            
        }

        public static void startDownload(int indexVersion)
        {
            LoggerManager.log("Start Download !");

            ManifestManager.GetManifestVersion(ManifestManager.minecraftVersions[indexVersion]);

            /* download necessary files step by step */
            downloadAssets(indexVersion);
            downloadLibraries(indexVersion);
            downloadMinecraft(indexVersion);
            downloadForge(indexVersion);
        }

        private static void downloadAssets(int indexVersion)
        {
            int currentAsset = 0;

            LoggerManager.log("Start Assets Download !");

            /* Create directories */
            if (!Directory.Exists(assetsIndexFolder))
            {
                Directory.CreateDirectory(assetsIndexFolder);
            }

            /* Download assets manifest */
            string assetsManifest = webClient.DownloadString(ManifestManager.minecrafetVersionAssets);
            System.IO.File.WriteAllText(assetsIndexFolder + ManifestManager.minecraftVersionJson.Assets + ".json", assetsManifest);

            MainWindow.ChangeDownLoadDescriptor("Step 1/6 : Vérification des assets...");
            fileToDownload.Clear();
            MainWindow.ChangeProgressBarValue(0);
            ManifestManager.assetsInformation.ForEach(delegate (AssetInformation assetInformation)
            {
                FileDownloadInformation fileDownloadInformation = new FileDownloadInformation();
                string assetPath = assetsObjectFolder + assetInformation.GetFolder() + "/" + assetInformation.Hash;
                /* Check if object were already download */
                if (System.IO.File.Exists(assetPath))
                {
                    /* Check sha of file */
                    if (assetInformation.Hash != Metadata.GetSha1(assetPath))
                    {
                        /* File sha is incorrect */
                        LoggerManager.log("File " + assetInformation.Hash + " sha is incorrect");
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

                MainWindow.ChangeProgressBarValue(++currentAsset * 100.0 / ManifestManager.assetsInformation.Count);
            });

            /* download assets */
            MainWindow.ChangeDownLoadDescriptor("Step 2/6 : Téléchargement des assets...");
            currentAsset = 0;
            MainWindow.ChangeProgressBarValue(0);
            fileToDownload.ForEach(delegate (FileDownloadInformation assetFile)
            {
                /* Create dir if not exist */
                if (!Directory.Exists(Path.GetDirectoryName(assetFile.outputPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(assetFile.outputPath));
                }

                /* Download file asset */
                webClient.DownloadFile(assetFile.url, assetFile.outputPath);

                MainWindow.ChangeProgressBarValue(++currentAsset * 100.0 / fileToDownload.Count);
            });
        }

        private static void downloadLibraries(int indexVersion)
        {
            int currentAsset = 0;

            LoggerManager.log("Start Libraries Download !");
            classPathList.Clear();
            nativesToExtract.Clear();
            fileToDownload.Clear();

            /* Create directories if needed */
            if (!Directory.Exists(libraryFolder))
            {
                Directory.CreateDirectory(libraryFolder);
            }

            MainWindow.ChangeDownLoadDescriptor("Step 3/6 : Vérification des Librairies...");
            currentAsset = 0;
            MainWindow.ChangeProgressBarValue(0);
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

                 if(libraryNeeded)
                 {
                    System.Uri libraryUrl = null;
                    string filePath = "";
                    string sha = "";

                    /* check if file is native or not */
                    if(library.Downloads.Classifiers != null)
                    {
                        /* File is native */
                        if(library.Downloads.Classifiers.NativesWindows != null)
                        {
                            /* File is windows native */
                            libraryUrl = library.Downloads.Classifiers.NativesWindows.Url;
                            filePath = libraryFolder + library.Downloads.Classifiers.NativesWindows.Path;
                            sha = library.Downloads.Classifiers.NativesWindows.Sha1;
                            nativesToExtract.Add(filePath);
                        }
                    }
                    else
                    {
                        /* File is library */
                        libraryUrl = library.Downloads.Artifact.Url;
                        filePath = libraryFolder + library.Downloads.Artifact.Path;
                        sha = library.Downloads.Artifact.Sha1;
                        classPathList.Add(filePath);
                    }

                    if(libraryUrl != null)
                    {
                        FileDownloadInformation fileDownloadInformation = new FileDownloadInformation();
                        /* Check if version were already download */
                        if (System.IO.File.Exists(filePath))
                        {
                            /* Check sha of file */
                            if (sha != Metadata.GetSha1(filePath))
                            {
                                /* File sha is incorrect, download again */
                                LoggerManager.log("File " + filePath + ".jar sha is incorrect");
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
                MainWindow.ChangeProgressBarValue(++currentAsset * 100.0 / ManifestManager.minecraftVersionJson.Libraries.Length);
            }

            MainWindow.ChangeDownLoadDescriptor("Step 4/6 : Téléchargement des Librairies...");
            currentAsset = 0;
            MainWindow.ChangeProgressBarValue(0);
            fileToDownload.ForEach(delegate (FileDownloadInformation libFile)
            {
                /* Create dir if not exist */
                if (!Directory.Exists(Path.GetDirectoryName(libFile.outputPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(libFile.outputPath));
                }

                /* Download file asset */
                webClient.DownloadFile(libFile.url, libFile.outputPath);

                MainWindow.ChangeProgressBarValue(++currentAsset * 100.0 / fileToDownload.Count);
            });
        }

        private static  void downloadMinecraft(int indexVersion)
        {
            LoggerManager.log("Start Game Download !");
            fileToDownload.Clear();
            FileDownloadInformation gameDownloadInformation = new FileDownloadInformation();
            FileDownloadInformation logconfigDownloadInformation = new FileDownloadInformation();

            if (!Directory.Exists(versionFolder + ManifestManager.minecraftVersionJson.Id))
            {
                Directory.CreateDirectory(versionFolder + ManifestManager.minecraftVersionJson.Id);
            }

            MainWindow.ChangeDownLoadDescriptor("Step 5/6 : Vérification des fichiers du jeu...");
            string gameFilePath = versionFolder + ManifestManager.minecraftVersionJson.Id + "/" + ManifestManager.minecraftVersionJson.Id + ".jar";
            gameDownloadInformation.url = ManifestManager.minecraftVersionJson.Downloads.Client.Url.AbsoluteUri;
            gameDownloadInformation.outputPath = gameFilePath;

            /* Check if version were already download */
            if (System.IO.File.Exists(gameFilePath))
            {
                /* Check sha of file */
                if (ManifestManager.minecraftVersionJson.Downloads.Client.Sha1 != Metadata.GetSha1(gameFilePath))
                {
                    /* File sha is incorrect */
                    LoggerManager.log("File " + ManifestManager.minecraftVersionJson.Id + ".jar sha is incorrect");
                    fileToDownload.Add(gameDownloadInformation);
                }
            }
            else
            {
                fileToDownload.Add(gameDownloadInformation);
            }

            if (ManifestManager.minecraftVersionJson.Logging.Client.File != null)
            {
                if (!Directory.Exists(assetsLogConfigFolder))
                {
                    Directory.CreateDirectory(assetsLogConfigFolder);
                }

                logconfigDownloadInformation.url = ManifestManager.minecraftVersionJson.Logging.Client.File.Url.AbsoluteUri;
                logconfigDownloadInformation.outputPath = assetsLogConfigFolder + ManifestManager.minecraftVersionJson.Logging.Client.File.Id;

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

            MainWindow.ChangeDownLoadDescriptor("Step 6/6 : Téléchargement des fichiers du jeu...");
            /* record json into this same path if not exist */
            string gameJSONFilePath = versionFolder + ManifestManager.minecraftVersionJson.Id + "/" + ManifestManager.minecraftVersionJson.Id + ".json";

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
                while(webClient.IsBusy) { }
            });

            webClient.DownloadProgressChanged -= eventHandler;
        }

        private static void downloadForge(int indexVersion)
        {
            
        }

        private static void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e)
        {
            MainWindow.ChangeProgressBarValue(e.ProgressPercentage);
            // Displays the operation identifier, and the transfer progress.
            //System.Console.WriteLine("{0}    downloaded {1} of {2} bytes. {3} % complete...",
            //    (string)e.UserState,
            //    e.BytesReceived,
            //    e.TotalBytesToReceive,
            //    e.ProgressPercentage);
        }
    }
}
