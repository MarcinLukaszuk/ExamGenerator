using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGenerator.DocumentManager.UnZipArchive
{
    public static class ArchiveUnZiper
    {
        public static List<Bitmap> GetBitmapsFromZipArchive(string zipArchivePath)
        {
            List<Bitmap> bitmaps = new List<Bitmap>();
            try
            {
                string tmpDirectory = string.Empty;
                if (zipArchivePath.Contains(".zip"))
                {
                    tmpDirectory = zipArchivePath.Replace(".zip", "");
                    if (Directory.Exists(tmpDirectory))
                    {
                        Directory.Delete(tmpDirectory, true);
                    }
                    ZipFile.ExtractToDirectory(zipArchivePath, tmpDirectory);
                }
                if (tmpDirectory != string.Empty)
                {
                    bitmaps = DirSearch(tmpDirectory).Select(x => LoadImage(x)).ToList();
                }
                Directory.Delete(tmpDirectory, true);
            }
            catch (Exception ex) { }
            return bitmaps;
        }

        private static Bitmap LoadImage(string imagePath)
        {
            Bitmap retval = null;
            using (Stream stream = new FileStream(imagePath, FileMode.Open, FileAccess.ReadWrite))
            {
                using (Bitmap bitmap = new Bitmap(stream))
                {
                    Rectangle cloneRect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                    retval = bitmap.Clone(cloneRect, bitmap.PixelFormat);
                }
            }
            return retval;
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
                foreach (string f in Directory.GetFiles(sDir))
                {
                    bitmapPaths.Add(f);
                }
            }
            catch (Exception ex)
            {
            }
            return bitmapPaths;
        }
    }
}
