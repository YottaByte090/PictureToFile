using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PictureToFile
{
    class Program
    {
        private static Bitmap input;
        private static List<Color> color = new List<Color>();
        private static List<Byte> output = new List<Byte>();

        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                if (File.Exists(args[0]))
                {
                    try
                    {
                        input = (Bitmap)Image.FromFile("input.png");
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("불러올 수 있는 파일이 없습니다.");
                        Console.ReadKey();
                        return;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(args[0] + " 은(는) 올바른 파일이 아닙니다.");
                    Console.ReadKey();
                    return;
                }
            }
            else if (args.Length > 1)
            {
                foreach (String arg in args)
                {
                    if (File.Exists(arg))
                    {
                        input = (Bitmap)Image.FromFile(arg);
                        break;
                    }
                }

                if (input == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("불러올 수 있는 파일이 없습니다.");
                    Console.ReadKey();
                    return;
                }
            }
            else
            {
                try
                {
                    input = (Bitmap)Image.FromFile("input.png");
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("input.png 파일을 불러올 수 없습니다.");
                    Console.ReadKey();
                    return;
                }
            }

            Stopwatch timer = new Stopwatch();
            int x;
            int y;

            Console.WriteLine(String.Format("파일 로드를 시작합니다.\n해상도 : {0}x{1}\n", input.Width, input.Height));
            timer.Start();

            for (x = 0; x < input.Width; x++)
            {
                for (y = 0; y < input.Height; y++)
                {
                    color.Add(input.GetPixel(x, y));
                }
            }

            int i;
            bool stop = false;

            Console.WriteLine(String.Format("파일 로드를 마쳤습니다.\n픽셀 수 : {0}\n", color.Count));
            Console.WriteLine("사진을 데이터로 변환합니다.");

            for (i = 0; (i < color.Count) && (!stop); i++)
            {
                Color currentPixel = color[i];

                switch (currentPixel.A)
                {
                    case 255:
                        output.Add(currentPixel.R);
                        output.Add(currentPixel.G);
                        output.Add(currentPixel.B);
                        break;
                    case 254:
                    case 3:
                        output.Add(currentPixel.R);
                        output.Add(currentPixel.G);
                        output.Add(currentPixel.B);
                        stop = true;
                        break;
                    case 252:
                    case 2:
                        output.Add(currentPixel.R);
                        output.Add(currentPixel.G);
                        stop = true;
                        break;
                    case 253:
                    case 1:
                        output.Add(currentPixel.R);
                        stop = true;
                        break;
                }
            }

            Console.WriteLine("사진을 성공적으로 변환했습니다.\n");

            /*
             * Depracted Source
             * 
             * Console.WriteLine("마무리 작업을 시작합니다.");
             * 
             * int offset;
             * for (offset = output.Count - 1; offset > 0; offset--)
             * {
             *     if (output[offset].ToString().Equals("0"))
             *     {
             *         output.RemoveAt(offset);
             *     }
             *     else
             *     {
             *         break;
             *     }
             * }
             * 
             * Console.WriteLine("마무리 작업을 완료했습니다\n");
             */

            Console.WriteLine("파일로 저장합니다.");

            File.WriteAllBytes("output.bin", output.ToArray());

            timer.Stop();
            Console.WriteLine(String.Format("변환 완료! {0}ms", timer.ElapsedMilliseconds.ToString()));
            Console.ReadKey();
        }
    }
}