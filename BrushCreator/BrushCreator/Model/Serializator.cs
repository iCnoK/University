using Paint.Utility.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BrushCreator.Model
{
    public static class Serializator
    {
        private const int XYSize = 100;
        private const int ColorArraySize = 4;

        public static void Serialize(KeyValuePair<BrushType, WriteableBitmap> valuePair, string fileName)
        {
            BinaryFormatter serializer = new BinaryFormatter();
            FileStream stream;
            
            byte[] bytedImage = new byte[XYSize * ColorArraySize * XYSize];
            valuePair.Value.CopyPixels(bytedImage, XYSize * ColorArraySize, 0);
            stream = new FileStream(fileName, FileMode.OpenOrCreate);
            serializer.Serialize(stream, bytedImage);
            serializer.Serialize(stream, valuePair.Key);
            stream.Close();

            stream.Close();
        }
    }
}