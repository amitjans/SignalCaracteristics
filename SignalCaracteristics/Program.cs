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
            List<string> lines = new List<string>();
            lines.Add("@RELATION activity");
            lines.Add("");
            lines.Add("@ATTRIBUTE s1mediaaccx\tREAL");
            lines.Add("@ATTRIBUTE s1mediaaccy\tREAL");
            lines.Add("@ATTRIBUTE s1mediaaccz\tREAL");
            lines.Add("@ATTRIBUTE s1magxyz\tREAL");
            lines.Add("@ATTRIBUTE s1sdmagxyz\tREAL");
            lines.Add("@ATTRIBUTE s2mediaaccx\tREAL");
            lines.Add("@ATTRIBUTE s2mediaaccy\tREAL");
            lines.Add("@ATTRIBUTE s2mediaaccz\tREAL");
            lines.Add("@ATTRIBUTE s2magxyz\tREAL");
            lines.Add("@ATTRIBUTE s2sdmagxyz\tREAL");
            lines.Add("@ATTRIBUTE class\t{caminando,subiendo-pendiente,bajando-pendiente,subiendo-escaleras,bajando-escaleras}");
            lines.Add("");
            lines.Add("@DATA");
            
            //Console.WriteLine("Actividad\n[1] caminando\n[2] subiendo-pendiente\n[3] bajando-pendiente\n[4]subiendo-escaleras\n[5] bajando-escaleras");
            //var act = Console.ReadLine();
            //using (var reader = new StreamReader(@"C:\test.csv"))
            var act = "";
            foreach (string dir in Directory.GetDirectories(System.IO.Directory.GetCurrentDirectory()))
            {
                act = dir.Substring(System.IO.Directory.GetCurrentDirectory().Length + 1);
                var files = Directory.GetFiles(dir);
                foreach (var item in files)
                {
                    if (item.Contains("csv"))
                    {
                        Console.WriteLine(Path.GetFileName(item));
                        using (var reader = new StreamReader(item))
                        {
                            List<float> s1x = new List<float>();
                            List<float> s1y = new List<float>();
                            List<float> s1z = new List<float>();
                            List<float> s1mag = new List<float>();
                            List<float> s2x = new List<float>();
                            List<float> s2y = new List<float>();
                            List<float> s2z = new List<float>();
                            List<float> s2mag = new List<float>();
                            var line = reader.ReadLine();
                            var values = line.Split(',');
                            while (!reader.EndOfStream)
                            {
                                line = reader.ReadLine();
                                values = line.Split(',');
                                if (int.Parse(values[0]) == 3)
                                {
                                    s1x.Add(float.Parse(values[3], CultureInfo.InvariantCulture));
                                    s1y.Add(float.Parse(values[4], CultureInfo.InvariantCulture));
                                    s1z.Add(float.Parse(values[5], CultureInfo.InvariantCulture));
                                }
                                else if (int.Parse(values[0]) == 4)
                                {
                                    s2x.Add(float.Parse(values[3], CultureInfo.InvariantCulture));
                                    s2y.Add(float.Parse(values[4], CultureInfo.InvariantCulture));
                                    s2z.Add(float.Parse(values[5], CultureInfo.InvariantCulture));
                                }
                            }
                            s1mag = Magnitud(s1x, s1y, s1z);
                            s2mag = Magnitud(s2x, s2y, s2z);
                            lines.Add(FloatFormat(Media(s1x)) + "," + FloatFormat(Media(s1y)) + "," + FloatFormat(Media(s1z)) + "," + FloatFormat(Media(s1mag)) + "," + FloatFormat(SD(s1mag, Media(s1mag))) + "," + FloatFormat(Media(s2x)) + "," + FloatFormat(Media(s2y)) + "," + FloatFormat(Media(s2z)) + "," + FloatFormat(Media(s2mag)) + "," + FloatFormat(SD(s2mag, Media(s2mag))) + "," + act);
                            Console.WriteLine();
                        }
                    }
                }
            }
            System.IO.File.WriteAllLines(System.IO.Directory.GetCurrentDirectory() + "\\actividad.arff", lines);
        }

        private static string FloatFormat(float value){
            return value.ToString().Replace(",", ".");
        }

        private static string Actividad(int act)
        {
            switch (act)
            {
                case 1:
                    return "caminando";
                case 2:
                    return "subiendo-pendiente";
                case 3:
                    return "bajando-pendiente";
                case 4:
                    return "subiendo-escaleras";
                default:
                    return "bajando-escaleras";
            }
        }

        private static float Media(List<float> list)
        {
            var temp = 0.0;
            foreach (var item in list)
            {
                temp += item;
            }
            temp /= list.Count;
            return (float)temp;
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

        private static List<float> Magnitud(List<float> x, List<float> y, List<float> z)
        {
            List<float> mag = new List<float>();
            for (int i = 0; i < x.Count; i++)
            {
                mag.Add(MathF.Sqrt(MathF.Pow(x[i], 2) + MathF.Pow(y[i], 2) + MathF.Pow(z[i], 2)));
            }
            return mag;
        }
    }
}
