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
                        Media(x, "Media Eje x");
                        Media(y, "Media Eje y");
                        Media(z, "Media Eje z");

                        Moda(x, "Moda Eje x");
                        Moda(y, "Moda Eje y");
                        Moda(z, "Moda Eje z");

                        Console.WriteLine();
                    }
                }
            }
            Console.Read();
        }

        private static void Media(List<float> list, string label)
        {
            var temp = 0.0;
            foreach (var item in list)
            {
                temp += item;
            }

            temp /= list.Count;

            Console.WriteLine(label + ": " + temp.ToString());
        }

        private static void Moda(List<float> list, string label)
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

            Console.WriteLine(label + ": " + key.ToString() + " con " + temp[key] + " aparici" + (temp[key] > 1 ? "ones" : "ón"));
        }

    }
}
