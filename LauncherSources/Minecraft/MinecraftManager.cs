using AramisLauncher.Common;
using AramisLauncher.Download;
using AramisLauncher.JSON;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Windows.Controls;
using System.Windows.Threading;

namespace AramisLauncher.Minecraft
{
    class MinecraftManager
    {
        private static Process minecraftGame = new Process();
        public MinecraftManager()
        {
        }

        public static void StartMinecraft()
        {
            HomeUserControl.ChangeDownLoadDescriptor("Démarrage de minecraft...");
            CreateNatives();
            /* Create new Process and assign parameters */
            minecraftGame.StartInfo = new ProcessStartInfo("javaw");
            minecraftGame.StartInfo.CreateNoWindow = true;

            minecraftGame.EnableRaisingEvents = true;
            minecraftGame.Exited += Process_Exited;
            minecraftGame.StartInfo.WorkingDirectory = CommonData.minecraftFolder;
            if (ManifestManager.forgeVersionJson != null)
            {
                minecraftGame.StartInfo.Arguments = CreateCommand();
            }
            else
            {
                minecraftGame.StartInfo.Arguments = ParseArguments();
            }
            minecraftGame.Start();
            HomeUserControl.ChangeDownLoadDescriptor("Minecraft est lancé...");
        }

        public static void StopMinecraft()
        {
            try
            {
                if (!minecraftGame.HasExited)
                {
                    minecraftGame.Kill();
                }
            }
            catch (Exception)
            {

            }
        }

        private static string CreateCommand()
        {
            StringBuilder arguments = new StringBuilder();

            arguments.Append("-Xmx" + 3 + "G ");

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

            arguments.Append(";" + CommonData.minecraftFolder + "versions/" + ManifestManager.minecraftVersionJson.Id + "/" + ManifestManager.minecraftVersionJson.Id + ".jar ");

            //arguments.Append(ManifestManager.minecraftVersionJson.MainClass + " ");
            arguments.Append(ManifestManager.forgeVersionJson.MainClass + " ");

            arguments.Append("--username ");
            arguments.Append(CommonData.launcherProfileJson.authenticationDatabase.selectedProfile.name + " ");
            //arguments.Append("--userProperties ");
            //arguments.Append(JsonConvert.SerializeObject(CommonData.launcherProfileJson.property));
            arguments.Append("--version ");
            arguments.Append(ManifestManager.minecraftVersionJson.Id + " ");
            arguments.Append("--gameDir ");
            arguments.Append(CommonData.packageFolder + " ");
            arguments.Append("--assetsDir ");
            arguments.Append(CommonData.minecraftFolder + "assets/ ");
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

            arguments.Append("--server ");
            arguments.Append(CommonData.packageServerAddress.Split(':')[0] + " ");
            arguments.Append("--port ");
            arguments.Append(CommonData.packageServerAddress.Split(':')[1] + " ");

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
                catch (Exception)
                {

                }
            }
        }

        private static string ParseArguments()
        {
            /* Actually do not parse command and force use command */
            StringBuilder arguments = new StringBuilder();

            Arguments minecraftArguments = ManifestManager.minecraftVersionJson.Arguments;
            if (minecraftArguments == null)
            {
                arguments = new StringBuilder(CreateCommand());
            }
            else
            {
                foreach (JvmElement jvmElement in minecraftArguments.Jvm)
                {
                    if (jvmElement.String == null)
                    {
                        if (jvmElement.JvmClass != null)
                        {
                            if (jvmElement.JvmClass.Rules[0].Os.Name == "windows" && jvmElement.JvmClass.Rules[0].Action == JSON.Action.Allow)
                            {
                                if (jvmElement.JvmClass.Rules[0].Os.Version == null || (jvmElement.JvmClass.Rules[0].Os.Version != null && jvmElement.JvmClass.Rules[0].Os.Version.Contains("10")))
                                {
                                    if (jvmElement.JvmClass.Value.String != null)
                                    {
                                        arguments.Append(jvmElement.JvmClass.Value.String + " ");
                                    }
                                    else
                                    {
                                        foreach (string args in jvmElement.JvmClass.Value.StringArray)
                                        {
                                            if (args.Contains(" "))
                                            {
                                                arguments.Append("\"");
                                            }
                                            arguments.Append(args);
                                            if (args.Contains(" "))
                                            {
                                                arguments.Append("\"");
                                            }

                                            arguments.Append(" ");
                                        }
                                    }
                                }
                            }
                            else if (jvmElement.JvmClass.Rules[0].Os.Arch == "x86")
                            {
                                arguments.Append(jvmElement.JvmClass.Value.String + " ");
                            }
                        }
                    }
                    else
                    {
                        if (jvmElement.String.Contains("${natives_directory}"))
                        {
                            arguments.Append(jvmElement.String.Replace("${natives_directory}", CommonData.nativeFolder + " "));
                        }
                        else if (jvmElement.String.Contains("-cp"))
                        {
                            arguments.Append(jvmElement.String + " ");
                        }
                        else if (jvmElement.String.Contains("${classpath}"))
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

                            arguments.Append(";" + CommonData.versionFolder + ManifestManager.minecraftVersionJson.Id + "/" + ManifestManager.minecraftVersionJson.Id + ".jar ");
                        }
                    }
                }

                arguments.Append("-Xmx3G ");
                arguments.Append("-XX:+UnlockExperimentalVMOptions ");
                arguments.Append("-XX:+UseG1GC ");
                arguments.Append("-XX:G1NewSizePercent=20 ");
                arguments.Append("-XX:G1ReservePercent=20 ");
                arguments.Append("-XX:MaxGCPauseMillis=50 ");
                arguments.Append("-XX:G1HeapRegionSize=32M ");

                if (ManifestManager.forgeVersionJson != null)
                {
                    arguments.Append(ManifestManager.forgeVersionJson.MainClass + " ");
                }
                else
                {
                    arguments.Append(ManifestManager.newForgeVersionJson.MainClass + " ");
                }

                foreach (GameElement gameElement in minecraftArguments.Game)
                {
                    if (gameElement.String != null)
                    {
                        if (gameElement.String.Contains("--"))
                        {
                            arguments.Append(gameElement.String + " ");
                        }
                        else
                        {
                            if (String.Equals(gameElement.String, "${auth_player_name}"))
                            {
                                arguments.Append(CommonData.launcherProfileJson.authenticationDatabase.selectedProfile.name + " ");
                            }
                            else if (String.Equals(gameElement.String, "${version_name}"))
                            {
                                arguments.Append(ManifestManager.minecraftVersionJson.Id + "-" + ManifestManager.packageJson.BaseModLoader.Name + " ");
                            }
                            else if (String.Equals(gameElement.String, "${game_directory}"))
                            {
                                arguments.Append(CommonData.packageFolder + " ");
                            }
                            else if (String.Equals(gameElement.String, "${assets_root}"))
                            {
                                arguments.Append(CommonData.assetsFolder + " ");
                            }
                            else if (String.Equals(gameElement.String, "${assets_index_name}"))
                            {
                                arguments.Append(ManifestManager.minecraftVersionJson.Assets + " ");
                            }
                            else if (String.Equals(gameElement.String, "${auth_uuid}"))
                            {
                                Guid guid = new Guid(CommonData.launcherProfileJson.authenticationDatabase.selectedProfile.id);
                                arguments.Append(guid + " ");
                            }
                            else if (String.Equals(gameElement.String, "${auth_access_token}"))
                            {
                                arguments.Append(CommonData.launcherProfileJson.authenticationDatabase.accessToken + " ");
                            }
                            else if (String.Equals(gameElement.String, "${user_type}"))
                            {
                                arguments.Append("mojang ");
                            }
                            else if (String.Equals(gameElement.String, "${version_type}"))
                            {
                                arguments.Append(ManifestManager.minecraftVersionJson.Type + " ");
                            }
                        }
                    }
                }

                /* Not work with 1.15.2 */
                //arguments.Append("--server ");
                //arguments.Append(CommonData.packageServerAddress.Split(':')[0] + " ");
                //arguments.Append("--port ");
                //arguments.Append(CommonData.packageServerAddress.Split(':')[1] + " ");

                foreach (string forgeArguments in ManifestManager.newForgeVersionJson.Arguments.Game)
                {
                    arguments.Append(forgeArguments + " ");
                }
            }

            return arguments.ToString();
        }

        private static void Process_Exited(object sender, EventArgs e)
        {
            try
            {
                Directory.Delete(CommonData.nativeFolder, true);
            }
            catch (Exception)
            {

            }
            HomeUserControl.ChangeDownLoadDescriptor("Minecraft n'est pas lancé...");
            HomeUserControl.ChangeDownloadButtonVisibility(System.Windows.Visibility.Visible);
        }
    }
}
