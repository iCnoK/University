using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paint.Helpers
{
    class FileSystem
    {
        public static Bitmap OpenFile(string fileName)
        {
            Bitmap result = new Bitmap(fileName);
            return result;
        }
    }
}
