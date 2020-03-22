using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AramisLauncher.Minecraft
{
    class AssetInformation
    {
        public string Title;
        public string Hash;
        public long Size;
        public string GetFolder()
        {
            /* Asset folder should be equal to first two characters of Hash */
            string folderPath = "";
            folderPath += Hash[0];
            folderPath += Hash[1];
            return folderPath;
        }
    }
}
