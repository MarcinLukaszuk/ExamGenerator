using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExamGenerator.DocumentManager.ZIPValidator
{
    public static class ZIPDirectory
    {
        public static List<Bitmap> GetAllBitmapsOfZipArchive(string zipArchivePath)
        {
            string tmpDirectory = string.Empty;
            List<Bitmap> bitmaps = new List<Bitmap>();
            if (zipArchivePath.Contains(".zip"))
            {
                tmpDirectory = "tmp\\" + zipArchivePath.Replace(".zip", "");
                if (Directory.Exists(tmpDirectory))
                {
                    Directory.Delete(tmpDirectory, true);
                }
                ZipFile.ExtractToDirectory(zipArchivePath, tmpDirectory);
            }
            if (tmpDirectory != string.Empty)
            {
                bitmaps = DirSearch(tmpDirectory).Select(x => new Bitmap(x)).ToList();
            }
            return bitmaps;
        }
        private static List<string> DirSearch(string sDir)
        {
            var bitmapPaths = new List<string>();
            try
            {
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    foreach (string f in Directory.GetFiles(d))
                    {
                        bitmapPaths.Add(f);
                    }
                    bitmapPaths.AddRange(DirSearch(d));
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
            return bitmapPaths;
        }

    }
}
