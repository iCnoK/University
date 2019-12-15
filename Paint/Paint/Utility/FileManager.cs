using Paint.Utility.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace Paint.Utility
{
    public abstract class FileManager
    {
        protected void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        protected bool IsDirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        protected bool IsFileExists(string path)
        {
            return File.Exists(path);
        }
    }

    public class BrushLoader : FileManager
    {
        private const int XYSize = 100;
        private const int ColorArraySize = 4;

        public List<KeyValuePair<BrushType, WriteableBitmap>> WriteableBitmaps { get; private set; }

        public List<WriteableBitmap> GetBrushesOfSpecialType(BrushType type)
        {
            List<WriteableBitmap> result = new List<WriteableBitmap>();
            for (int i = 0; i < WriteableBitmaps.Count; i++)
            {
                if (WriteableBitmaps[i].Key == type)
                {
                    result.Add(WriteableBitmaps[i].Value);
                }
            }
            return result;
        }

        public BrushLoader()
        {
            WriteableBitmaps = new List<KeyValuePair<BrushType, WriteableBitmap>>();
            LoadAllBrushes();
        }

        //public void SaveBrushes()
        //{
        //    Serialize();
        //}

        public void ReloadDatabase()
        {
            LoadAllBrushes();
        }

        public void AddBrush(WriteableBitmap bitmap, BrushType brush)
        {
            WriteableBitmaps.Add(new KeyValuePair<BrushType, WriteableBitmap>
                (brush, bitmap));
        }

        private void LoadAllBrushes()
        {
            if (IsDirectoryExists("Brushes"))
            {
                List<string> fileNames = GetFileNamesOfBrushes();
                for (int i = 0; i < fileNames.Count; i++)
                {
                    Deserialize(fileNames[i]);
                }
            }
            else
            {
                CreateDirectory("Brushes");
            }
        }

        private List<string> GetFileNamesOfBrushes()
        {
            List<string> result = new List<string>();
            DirectoryInfo info = new DirectoryInfo("Brushes");
            foreach (var item in info.GetFiles())
            {
                result.Add(item.FullName);
            }
            return result;
        }

        private void Deserialize(string fileName)
        {
            BinaryFormatter serializer = new BinaryFormatter();
            if (IsFileExists(fileName))
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Open))
                {
                    byte[] bytedImage = serializer.Deserialize(fs) as byte[];
                    WriteableBitmap bitmap = new WriteableBitmap(XYSize, XYSize, 96, 96, System.Windows.Media.PixelFormats.Bgra32, null);

                    BrushType brushType = (BrushType)serializer.Deserialize(fs);

                    bitmap.WritePixels(new System.Windows.Int32Rect(0, 0, XYSize, XYSize), bytedImage, XYSize * ColorArraySize, 0);

                    WriteableBitmaps.Add(new KeyValuePair<BrushType, WriteableBitmap>
                        (brushType, bitmap));
                }
            }
        }

        //private void Serialize()
        //{
        //    BinaryFormatter serializer = new BinaryFormatter();
        //    FileStream stream = null;
        //    int counter = 0;
        //    foreach (var item in WriteableBitmaps)
        //    {
        //        byte[] bytedImage = new byte[XYSize * ColorArraySize * XYSize];
        //        item.Value.CopyPixels(bytedImage, XYSize * ColorArraySize, 0);
        //        stream = new FileStream($"Brushes\\brush{counter++}.bin", FileMode.OpenOrCreate);
        //        serializer.Serialize(stream, bytedImage);
        //        serializer.Serialize(stream, item.Key);
        //        stream.Close();
        //    }
        //    stream.Close();
        //}
    }
}
