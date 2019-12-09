using BrushCreator.Model;
using Microsoft.Win32;
using Paint.Utility.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BrushCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            ShowMessage();
            if (ParseInt() == 1)
            {
                BrushType brushType = GetBrushTypeFromConsole();
                WriteableBitmap brush = GetImage();
                string fileName = GetFileName();
                Serializator.Serialize(new KeyValuePair<BrushType, WriteableBitmap>(brushType, brush), fileName);
                ShowResultMessage(fileName);
                Console.ReadKey();
            }
            else
            {
                return;
            }
        }

        private static WriteableBitmap GetImage()
        {
            while(true)
            {
                Console.Clear();
                ShowImageMessage();
                string input = Console.ReadLine();
                if (File.Exists(input))
                {
                    if (Path.GetExtension(input) == ".png")
                    {
                        BitmapImage bitmapImage = new BitmapImage(new Uri(input));
                        bitmapImage.CreateOptions = BitmapCreateOptions.None;
                        WriteableBitmap resultBitmap = new WriteableBitmap(bitmapImage);
                        if (resultBitmap.PixelWidth == 100 && resultBitmap.PixelHeight == 100)
                        {
                            return resultBitmap;
                        }
                    }
                }
            }
        }

        private static string GetFileName()
        {
            return Path.GetRandomFileName() + ".PaintBrush";
        }

        private static BrushType GetBrushTypeFromConsole()
        {
            while(true)
            {
                Console.Clear();
                ShowBrushType();
                string input = Console.ReadLine();
                int number = 0;
                if (!int.TryParse(input, out number))
                {
                    continue;
                }
                else
                {
                    if (number >= 1 && number <= 6) 
                    {
                        return GetBrushType(number);
                    }
                }
            }
        }

        private static BrushType GetBrushType(int index)
        {
            switch(index)
            {
                case 1: return BrushType.MARKER;
                case 2: return BrushType.FOUNTAINPEN;
                case 3: return BrushType.OILBRUSH;
                case 4: return BrushType.WATERCOLOR;
                case 5: return BrushType.PIXELPEN;
                case 6: return BrushType.PENCIL;
                default: return BrushType.MARKER;
            }
        }

        private static int ParseInt()
        {
            string input = Console.ReadLine();
            int number = 0;
            if (!int.TryParse(input, out number))
            {
                Main(null);
            }
            if (number != 1 && number != 0)
            {
                Main(null);
            }
            return number;
        }

        private static void ShowResultMessage(string fileName)
        {
            Console.Clear();
            Console.Write("Операция успешна!\n" +
                $"Файл с названием \"{fileName}\" находится в папке \"{Environment.CurrentDirectory}\"\n");
        }

        private static void ShowImageMessage()
        {
            Console.Write("Введите путь к Вашему изображению:\n" +
                "Пояснение: только png, размер 100х100 пикселей, " +
                "кисть создается по разнице между прозрачным и непрозрачным фоном.\n" +
                "Ввод => ");
        }

        private static void ShowBrushType()
        {
            Console.Write("Выберите желаемый тип кисти:\n" +
                "1 - Маркер,\n" +
                "2 - Перьевая ручка,\n" +
                "3 - Кисть для масляных красок,\n" +
                "4 - Акварель,\n" +
                "5 - Пиксельное перо,\n" +
                "6 - Карандаш.\n" +
                "Ввод => ");
        }

        private static void ShowMessage()
        {
            Console.Write("Добро пожаловать в конфигуратор кистей для PainD!\n" +
                "Введите 1 для продолжения и 0 для выхода:\n" +
                "Ввод => ");
        }
    }
}
