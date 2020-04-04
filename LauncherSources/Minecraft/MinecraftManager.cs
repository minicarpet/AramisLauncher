using AramisLauncher.Common;
using AramisLauncher.Download;
using AramisLauncher.JSON;
using AramisLauncher.Logger;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace AramisLauncher.Minecraft
{
    class MinecraftManager
    {
        public static Process minecraftGame = new Process();
        public MinecraftManager()
        {
            /* Create new Process and assign parameters */
            minecraftGame.StartInfo = new ProcessStartInfo("javaw");
            minecraftGame.StartInfo.CreateNoWindow = true;

            minecraftGame.EnableRaisingEvents = true;
            minecraftGame.Exited += Process_Exited;
        }
        public static void StartMinecraft()
        {
            LoggerManager.log("Start Minecraft !");
            MainWindow.ChangeDownLoadDescriptor("Démarrage de minecraft...");
            CreateNatives();

            minecraftGame.StartInfo.WorkingDirectory = CommonData.aramisFolder;
            minecraftGame.StartInfo.Arguments = CreateCommand();
            minecraftGame.Start();
            MainWindow.ChangeDownLoadDescriptor("Minecraft is running...");
        }

        private static string CreateCommand()
        {
            StringBuilder arguments = new StringBuilder();

            arguments.Append("-Xmx" + 8 + "G ");

            arguments.Append("-Djava.library.path=" + CommonData.nativeFolder + " ");
            arguments.Append("-Dorg.lwjgl.librarypath=" + CommonData.nativeFolder + " ");
            arguments.Append("-cp ");

            /* add every class */
            bool shouldAddComma = false;
            DownloadManager.classPathList.ForEach(delegate (string library)
            {
                if (shouldAddComma)
                {
                    arguments.Append(';');
                }
                arguments.Append(library);
                shouldAddComma = true;
            });

            arguments.Append(";" + CommonData.aramisFolder + "versions/" + ManifestManager.minecraftVersionJson.Id + "/" + ManifestManager.minecraftVersionJson.Id + ".jar ");

            //arguments.Append(ManifestManager.minecraftVersionJson.MainClass + " ");
            arguments.Append(ManifestManager.forgeVersionJson.MainClass + " ");
            
            arguments.Append("--username ");
            arguments.Append(CommonData.launcherProfileJson.authenticationDatabase.selectedProfile.name + " ");
            //arguments.Append("--userProperties ");
            //arguments.Append(JsonConvert.SerializeObject(CommonData.launcherProfileJson.property));
            arguments.Append("--server ");
            arguments.Append("aramiscraft.omgserv.net ");
            arguments.Append("--port ");
            arguments.Append("10554 ");
            arguments.Append("--version ");
            arguments.Append(ManifestManager.minecraftVersionJson.Id + " ");
            arguments.Append("--gameDir ");
            arguments.Append(CommonData.aramisFolder + " ");
            arguments.Append("--assetsDir ");
            arguments.Append(CommonData.aramisFolder + "assets/ ");
            arguments.Append("--assetIndex ");
            arguments.Append(ManifestManager.minecraftVersionJson.Assets + " ");
            arguments.Append("--uuid ");
            Guid guid = new Guid(CommonData.launcherProfileJson.authenticationDatabase.selectedProfile.id);
            arguments.Append(guid + " ");
            arguments.Append("--accessToken ");
            arguments.Append(CommonData.launcherProfileJson.authenticationDatabase.accessToken + " ");
            arguments.Append("--userType ");
            arguments.Append("mojang ");
            arguments.Append("--tweakClass net.minecraftforge.fml.common.launcher.FMLTweaker ");
            arguments.Append("--versionType ");
            arguments.Append("Forge ");

            arguments.Append("--width " + 1920 + " ");
            arguments.Append("--height " + 1080 + " ");

            LoggerManager.log("Using arguments : " + arguments.ToString());
            return arguments.ToString();
        }

        private static void CreateNatives()
        {
            /* Create natives from library to avoid a problem between version */
            /* Create directories for natives if needed */
            if (!Directory.Exists(CommonData.nativeFolder))
            {
                Directory.CreateDirectory(CommonData.nativeFolder);
            }

            foreach (string fileToExtract in DownloadManager.nativesToExtract)
            {
                try
                {
                    ZipFile.ExtractToDirectory(fileToExtract, CommonData.nativeFolder);
                    foreach (string directory in Directory.GetDirectories(CommonData.nativeFolder))
                    {
                        Directory.Delete(directory, true);
                    }
                }
                catch (Exception ex)
                {
                    LoggerManager.log(ex.ToString());
                }
            }
        }

        private void ParseArguments()
        {
            /* Actually do not parse command and force use command */
            StringBuilder arguments = new StringBuilder();
            Arguments minecraftArguments = ManifestManager.minecraftVersionJson.Arguments;
            if (minecraftArguments == null)
            {
                /* try to get arguments of old version */
                string oldMinecraftArguments = ManifestManager.minecraftVersionJson.minecraftArguments;
                if (oldMinecraftArguments != null)
                {

                }
            }
            else
            {

            }

            foreach (JvmElement jvmElement in minecraftArguments.Jvm)
            {
                string argumentsToAdd = "";

                argumentsToAdd = jvmElement.String;
                if (argumentsToAdd == null)
                {
                    /** \todo: Should be parsed... */
                }
                else
                {
                    if (argumentsToAdd.Contains("$"))
                    {
                        if (argumentsToAdd != "${classpath}")
                        {
                            foreach (string argument in argumentsToAdd.Split('$'))
                            {
                                if (argument.Contains("Dminecraft"))
                                {

                                }
                                else
                                {
                                    if (argument.Contains("{"))
                                    {
                                        switch (argument)
                                        {
                                            case "{natives_directory}":
                                                arguments.Append(CommonData.nativeFolder + " ");
                                                break;
                                            case "{launcher_name}":
                                                break;
                                            case "{launcher_version}":
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        arguments.Append(argument);
                                    }
                                }
                            }
                        }
                        else
                        {
                            /* add every class */
                            bool shouldAddComma = false;
                            DownloadManager.classPathList.ForEach(delegate (string library)
                            {
                                if (shouldAddComma)
                                {
                                    arguments.Append(';');
                                }
                                arguments.Append(library);
                                shouldAddComma = true;
                            });

                            arguments.Append(";" + CommonData.aramisFolder + "versions/" + ManifestManager.minecraftVersionJson.Id + "/" + ManifestManager.minecraftVersionJson.Id + ".jar ");

                            arguments.Append(ManifestManager.minecraftVersionJson.MainClass + " ");
                        }
                    }
                    else
                    {
                        arguments.Append(argumentsToAdd + " ");
                    }
                }
            }

            foreach (GameElement gameElement in minecraftArguments.Game)
            {
                string argumentsToAdd = "";

                argumentsToAdd = gameElement.String;
                if (argumentsToAdd == null)
                {
                    /** \todo: Should be parsed... */
                }
                else
                {
                    if (argumentsToAdd.Contains("$"))
                    {
                        switch (argumentsToAdd)
                        {
                            case "${auth_player_name}":
                                arguments.Append(CommonData.launcherProfileJson.authenticationDatabase.selectedProfile.name + " ");
                                break;
                            case "${version_name}":
                                arguments.Append(ManifestManager.minecraftVersionJson.Id + " ");
                                break;
                            case "${game_directory}":
                                arguments.Append(CommonData.aramisFolder + " ");
                                break;
                            case "${assets_root}":
                                arguments.Append(CommonData.aramisFolder + "assets/ ");
                                break;
                            case "${assets_index_name}":
                                arguments.Append(ManifestManager.minecraftVersionJson.Assets + " ");
                                break;
                            case "${auth_uuid}":
                                arguments.Append(CommonData.launcherProfileJson.authenticationDatabase.selectedProfile.id + " ");
                                break;
                            case "${auth_access_token}":
                                arguments.Append(CommonData.launcherProfileJson.authenticationDatabase.accessToken + " ");
                                break;
                            case "${user_type}":
                                arguments.Append("mojang ");
                                break;
                            case "${version_type}":
                                arguments.Append("Vanilla ");
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        arguments.Append(argumentsToAdd + " ");
                    }
                }
            }
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            Directory.Delete(CommonData.nativeFolder, true);
            MainWindow.ChangeDownLoadDescriptor("Minecraft n'est pas lancé...");
        }
    }
}
