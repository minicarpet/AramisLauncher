using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AramisLauncher.DownloadManager
{
    public class FileDownloadInformation
    {
        public string url;
        public string outputPath;
    }

    class DownloadManager
    {
        private static List<FileDownloadInformation> fileToDownload = new List<FileDownloadInformation>();
        private ProgressBar progressBar;
        private TextBlock textBlock;

        public DownloadManager(ProgressBar newProgressBar, TextBlock newTextBlock)
        {

        }
    }
}
