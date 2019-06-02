using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace SignalCaracteristics
{
    class Program
    {
        static void Main(string[] args)
        {
            var files = Directory.GetFiles(System.IO.Directory.GetCurrentDirectory());
            //using (var reader = new StreamReader(@"C:\test.csv"))
            foreach (var item in files)
            {
                if (item.Contains("csv"))
                {
                    Console.WriteLine(Path.GetFileName(item));
                    using (var reader = new StreamReader(item))
                    {
                        List<float> x = new List<float>();
                        List<float> y = new List<float>();
                        List<float> z = new List<float>();
                        var line = reader.ReadLine();
                        var values = line.Split(',');
                        while (!reader.EndOfStream)
                        {
                            line = reader.ReadLine();
                            values = line.Split(',');

                            x.Add(float.Parse(values[3], CultureInfo.InvariantCulture));
                            y.Add(float.Parse(values[4], CultureInfo.InvariantCulture));
                            z.Add(float.Parse(values[5], CultureInfo.InvariantCulture));
                        }

                        var mediax = Media(x);
                        var mediay = Media(y);
                        var mediaz = Media(z);

                        Console.WriteLine("Media Eje x: " + mediax + ", SD: " + SD(x, mediax));
                        Console.WriteLine("Media Eje y: " + mediay + ", SD: " + SD(y, mediay));
                        Console.WriteLine("Media Eje z: " + mediaz + ", SD: " + SD(z, mediaz));

                        var modax = Moda(x);
                        var moday = Moda(y);
                        var modaz = Moda(z);

                        Console.WriteLine("Moda Eje x: " + modax[0].ToString() + " con " + modax[1].ToString() + " aparici" + (modax[1] > 1 ? "ones" : "ón"));
                        Console.WriteLine("Moda Eje x: " + moday[0].ToString() + " con " + moday[1].ToString() + " aparici" + (moday[1] > 1 ? "ones" : "ón"));
                        Console.WriteLine("Moda Eje x: " + modaz[0].ToString() + " con " + modaz[1].ToString() + " aparici" + (modaz[1] > 1 ? "ones" : "ón"));

                        Max(x, "Maximo Eje x");
                        Max(y, "Maximo Eje y");
                        Max(z, "Maximo Eje z");

                        Min(x, "Minimo Eje x");
                        Min(y, "Minimo Eje y");
                        Min(z, "Minimo Eje z");

                        Console.WriteLine();
                    }
                }
            }
            Console.Read();
        }

        private static float Media(List<float> list)
        {
            var temp = 0.0;
            foreach (var item in list)
            {
                temp += item;
            }
            temp /= list.Count;
            return (float) temp;
        }

        private static float[] Moda(List<float> list)
        {
            var temp = new Dictionary<float, int>();
            foreach (var item in list)
            {
                int value = 0;
                if (temp.TryGetValue(item, out value))
                {
                    temp[item] = value++;
                }
                else
                {
                    temp.Add(item, 1);
                }
            }

            int cant = 0;
            float key = 0;

            foreach (var item in temp.Keys)
            {
                if (cant < temp[item])
                {
                    cant = temp[item];
                    key = item;
                }
            }
            return new float[] { key, cant };
        }

        private static void Max(List<float> list, string label)
        {
            var temp = float.MinValue;
            foreach (var item in list)
            {
                if (item > temp)
                {
                    temp = item;
                }
            }
            Console.WriteLine(label + ": " + temp);
        }

        private static void Min(List<float> list, string label)
        {
            var temp = float.MaxValue;
            foreach (var item in list)
            {
                if (item < temp)
                {
                    temp = item;
                }
            }
            Console.WriteLine(label + ": " + temp);
        }

        private static float SD(List<float> list, float media)
        {
            var temp = 0.0;
            foreach (var item in list)
            {
                temp += MathF.Pow(item - media, 2);
            }
            temp /= list.Count;
            return MathF.Sqrt((float)temp);
        }
    }
}
