using AramisLauncher.Common;
using AramisLauncher.Download;
using AramisLauncher.JSON;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace AramisLauncher.Forge
{
    class ForgeInstaller
    {
        private static Process javaProcess = new Process();
        public static void installForge()
        {
            if (ManifestManager.newForgeVersionJson != null)
            {
                HomeUserControl.ChangeDownLoadDescriptor("Installation de forge...");
                /* Create new Process and assign parameters */
                javaProcess.StartInfo = new ProcessStartInfo("javaw");
                javaProcess.StartInfo.CreateNoWindow = true;

                javaProcess.StartInfo.WorkingDirectory = CommonData.minecraftFolder;

                if (!File.Exists(CommonData.libraryFolder + "de/oceanlabs/mcp/mcp_config/1.15.2-20200307.202953/mcp_config-1.15.2-20200307.202953-mappings.txt"))
                {
                    PrepareMCPConfig();
                }
                if (!File.Exists(CommonData.libraryFolder + "net/minecraft/client/1.15.2/client-1.15.2-slim.jar") || !File.Exists(CommonData.libraryFolder + "net/minecraft/client/1.15.2/client-1.15.2-extra.jar"))
                {
                    PrepareClient();
                }
                if (!File.Exists(CommonData.libraryFolder + "net/minecraft/client/1.15.2-20200307.202953/client-1.15.2-20200307.202953-srg.jar"))
                {
                    PrepareClientSRG();
                }
                if (!File.Exists(CommonData.libraryFolder + "net/minecraftforge/forge/1.15.2-31.1.44/forge-1.15.2-31.1.44-client.jar"))
                {
                    InstallPatch();
                }
            }
        }

        private static void PrepareMCPConfig()
        {
            List<string> listClass = new List<string>();
            listClass.Add("libraries/net/minecraftforge/installertools/1.1.4/installertools-1.1.4.jar");
            listClass.Add("libraries/net/md-5/SpecialSource/1.8.5/SpecialSource-1.8.5.jar");
            listClass.Add("libraries/net/sf/jopt-simple/jopt-simple/5.0.4/jopt-simple-5.0.4.jar");
            listClass.Add("libraries/com/google/code/gson/gson/2.8.0/gson-2.8.0.jar");
            listClass.Add("libraries/org/ow2/asm/asm-commons/6.1.1/asm-commons-6.1.1.jar");
            listClass.Add("libraries/com/google/guava/guava/20.0/guava-20.0.jar");
            listClass.Add("libraries/net/sf/opencsv/opencsv/2.3/opencsv-2.3.jar");
            listClass.Add("libraries/org/ow2/asm/asm-analysis/6.1.1/asm-analysis-6.1.1.jar");
            listClass.Add("libraries/org/ow2/asm/asm-tree/6.1.1/asm-tree-6.1.1.jar");
            listClass.Add("libraries/org/ow2/asm/asm/6.1.1/asm-6.1.1.jar");

            List<string> specifigArguments = new List<string>();
            specifigArguments.Add("--task");
            specifigArguments.Add("MCP_DATA");
            specifigArguments.Add("--input");
            specifigArguments.Add(CommonData.libraryFolder + "de/oceanlabs/mcp/mcp_config/1.15.2-20200307.202953/mcp_config-1.15.2-20200307.202953.zip");
            specifigArguments.Add("--output");
            specifigArguments.Add(CommonData.libraryFolder + "de/oceanlabs/mcp/mcp_config/1.15.2-20200307.202953/mcp_config-1.15.2-20200307.202953-mappings.txt");
            specifigArguments.Add("--key");
            specifigArguments.Add("mappings");
            PrepareArguments("net.minecraftforge.installertools.ConsoleTool", listClass, specifigArguments);

            javaProcess.Start();
            javaProcess.WaitForExit();
        }

        private static void PrepareClient()
        {
            if (File.Exists(CommonData.libraryFolder + "net/minecraft/client/1.15.2/client-1.15.2-slim.jar"))
            {
                File.Delete(CommonData.libraryFolder + "net/minecraft/client/1.15.2/client-1.15.2-slim.jar");
            }

            if (File.Exists(CommonData.libraryFolder + "net/minecraft/client/1.15.2/client-1.15.2-extra.jar"))
            {
                File.Delete(CommonData.libraryFolder + "net/minecraft/client/1.15.2/client-1.15.2-extra.jar");
            }

            List<string> listClass = new List<string>();
            listClass.Add("libraries/net/minecraftforge/jarsplitter/1.1.2/jarsplitter-1.1.2.jar");
            listClass.Add("libraries/net/sf/jopt-simple/jopt-simple/5.0.4/jopt-simple-5.0.4.jar");

            List<string> specifigArguments = new List<string>();
            specifigArguments.Add("--input");
            specifigArguments.Add(CommonData.versionFolder + "1.15.2/1.15.2.jar");
            specifigArguments.Add("--slim");
            specifigArguments.Add(CommonData.libraryFolder + "net/minecraft/client/1.15.2/client-1.15.2-slim.jar");
            specifigArguments.Add("--extra");
            specifigArguments.Add(CommonData.libraryFolder + "net/minecraft/client/1.15.2/client-1.15.2-extra.jar");
            specifigArguments.Add("--srg");
            specifigArguments.Add(CommonData.libraryFolder + "de/oceanlabs/mcp/mcp_config/1.15.2-20200307.202953/mcp_config-1.15.2-20200307.202953-mappings.txt");
            PrepareArguments("net.minecraftforge.jarsplitter.ConsoleTool", listClass, specifigArguments);
            javaProcess.Start();
            javaProcess.WaitForExit();
        }

        private static void PrepareClientSRG()
        {
            List<string> listClass = new List<string>();
            listClass.Add("libraries/net/md-5/SpecialSource/1.8.5/SpecialSource-1.8.5.jar");
            listClass.Add("libraries/org/ow2/asm/asm-commons/6.1.1/asm-commons-6.1.1.jar");
            listClass.Add("libraries/net/sf/jopt-simple/jopt-simple/4.9/jopt-simple-4.9.jar");
            listClass.Add("libraries/com/google/guava/guava/20.0/guava-20.0.jar");
            listClass.Add("libraries/net/sf/opencsv/opencsv/2.3/opencsv-2.3.jar");
            listClass.Add("libraries/org/ow2/asm/asm-analysis/6.1.1/asm-analysis-6.1.1.jar");
            listClass.Add("libraries/org/ow2/asm/asm-tree/6.1.1/asm-tree-6.1.1.jar");
            listClass.Add("libraries/org/ow2/asm/asm/6.1.1/asm-6.1.1.jar");

            List<string> specifigArguments = new List<string>();
            specifigArguments.Add("--in-jar");
            specifigArguments.Add(CommonData.libraryFolder + "net/minecraft/client/1.15.2/client-1.15.2-slim.jar");
            specifigArguments.Add("--out-jar");
            specifigArguments.Add(CommonData.libraryFolder + "net/minecraft/client/1.15.2-20200307.202953/client-1.15.2-20200307.202953-srg.jar");
            specifigArguments.Add("--srg-in");
            specifigArguments.Add(CommonData.libraryFolder + "de/oceanlabs/mcp/mcp_config/1.15.2-20200307.202953/mcp_config-1.15.2-20200307.202953-mappings.txt");
            PrepareArguments("net.md_5.specialsource.SpecialSource", listClass, specifigArguments);

            DownloadManager.classPathList.Add(CommonData.libraryFolder + "net/minecraft/client/1.15.2-20200307.202953/client-1.15.2-20200307.202953-srg.jar");

            javaProcess.Start();
            javaProcess.WaitForExit();
        }

        private static void InstallPatch()
        {
            List<string> listClass = new List<string>();
            listClass.Add("libraries/net/minecraftforge/binarypatcher/1.0.12/binarypatcher-1.0.12.jar");
            listClass.Add("libraries/commons-io/commons-io/2.4/commons-io-2.4.jar");
            listClass.Add("libraries/com/google/guava/guava/25.1-jre/guava-25.1-jre.jar");
            listClass.Add("libraries/net/sf/jopt-simple/jopt-simple/5.0.4/jopt-simple-5.0.4.jar");
            listClass.Add("libraries/com/github/jponge/lzma-java/1.3/lzma-java-1.3.jar");
            listClass.Add("libraries/com/nothome/javaxdelta/2.0.1/javaxdelta-2.0.1.jar");
            listClass.Add("libraries/com/google/code/findbugs/jsr305/3.0.2/jsr305-3.0.2.jar");
            listClass.Add("libraries/org/checkerframework/checker-qual/2.0.0/checker-qual-2.0.0.jar");
            listClass.Add("libraries/com/google/errorprone/error_prone_annotations/2.1.3/error_prone_annotations-2.1.3.jar");
            listClass.Add("libraries/com/google/j2objc/j2objc-annotations/1.1/j2objc-annotations-1.1.jar");
            listClass.Add("libraries/org/codehaus/mojo/animal-sniffer-annotations/1.14/animal-sniffer-annotations-1.14.jar");
            listClass.Add("libraries/trove/trove/1.0.2/trove-1.0.2.jar");

            List<string> specifigArguments = new List<string>();
            specifigArguments.Add("--clean");
            specifigArguments.Add(CommonData.libraryFolder + "net/minecraft/client/1.15.2-20200307.202953/client-1.15.2-20200307.202953-srg.jar");
            specifigArguments.Add("--output");
            specifigArguments.Add(CommonData.libraryFolder + "net/minecraftforge/forge/1.15.2-31.1.44/forge-1.15.2-31.1.44-client.jar");
            specifigArguments.Add("--apply");
            specifigArguments.Add(CommonData.libraryFolder + "net/minecraftforge/forge/1.15.2-31.1.44/forge-1.15.2-31.1.44-clientdata.lzma");
            PrepareArguments("net.minecraftforge.binarypatcher.ConsoleTool", listClass, specifigArguments);
            javaProcess.Start();
            javaProcess.WaitForExit();
        }

        private static void PrepareArguments(string mainClass, List<string> classPaths, List<string> specificArgs)
        {
            javaProcess.StartInfo.Arguments = "";
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("-cp ");
            bool shouldAddComma = false;
            foreach (string classPath in classPaths)
            {
                if (shouldAddComma)
                {
                    stringBuilder.Append(';');
                }
                stringBuilder.Append(classPath);
                shouldAddComma = true;
            }

            stringBuilder.Append(" " + mainClass + " ");
            foreach (string arg in specificArgs)
            {
                stringBuilder.Append(arg + " ");
            }
            javaProcess.StartInfo.Arguments = stringBuilder.ToString();
        }
    }
}
