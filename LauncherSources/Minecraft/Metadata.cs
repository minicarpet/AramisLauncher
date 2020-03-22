using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AramisLauncher.Minecraft
{
    class Metadata
    {
        public static string GetSha1(string fileName)
        {
            var str = "";
            using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
            using (BufferedStream bufferedStream = new BufferedStream(fileStream))
            {
                using (SHA1Managed sha1 = new SHA1Managed())
                {
                    byte[] hash = sha1.ComputeHash(bufferedStream);
                    StringBuilder formatted = new StringBuilder(2 * hash.Length);

                    foreach (byte b in hash)
                    {
                        formatted.AppendFormat("{0:X2}", b);
                    }

                    str = formatted.ToString();
                }
            }

            return str.ToLower();
        }
    }
}
