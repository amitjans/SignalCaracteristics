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
            Console.WriteLine("Generador de .arff para Weka\nSeleccione los sensores separados por coma");
            var sensores = Console.ReadLine().Split(",");
            var sen = new List<int>();
            for (int i = 0; i < sensores.Length; i++)
            {
                sen.Add(int.Parse(sensores[i]));
            }
            List<string> lines = new List<string>();
            lines.Add("@RELATION activity");
            lines.Add("");
            for (int i = 0; i < sen.Count; i++)
            {
                lines.Add("@ATTRIBUTE s" + sen[i] + "mediaaccx\tREAL");
                lines.Add("@ATTRIBUTE s" + sen[i] + "mediaaccy\tREAL");
                lines.Add("@ATTRIBUTE s" + sen[i] + "mediaaccz\tREAL");
                lines.Add("@ATTRIBUTE s" + sen[i] + "magxyz\tREAL");
                lines.Add("@ATTRIBUTE s" + sen[i] + "sdmagxyz\tREAL");
            }
            lines.Add("@ATTRIBUTE class\t{caminando,subiendo-pendiente,bajando-pendiente,subiendo-escaleras,bajando-escaleras}");
            lines.Add("");
            lines.Add("@DATA");
            foreach (string dir in Directory.GetDirectories(System.IO.Directory.GetCurrentDirectory()))
            {
                var act = dir.Substring(System.IO.Directory.GetCurrentDirectory().Length + 1);
                var files = Directory.GetFiles(dir);
                foreach (var item in files)
                {
                    if (item.Contains("csv"))
                    {
                        Console.WriteLine(Path.GetFileName(item));
                        using (var reader = new StreamReader(item))
                        {
                            List<SensorData> s = new List<SensorData>();
                            foreach (var i in sen)
                            {
                                s.Add(new SensorData());
                            }
                            var line = reader.ReadLine();
                            var values = line.Split(',');
                            while (!reader.EndOfStream)
                            {
                                line = reader.ReadLine();
                                values = line.Split(',');

                                if (sen.Contains(int.Parse(values[0])))
                                {
                                    var index = sen.IndexOf(int.Parse(values[0]));
                                    s[index].AddX(float.Parse(values[3].Trim(), CultureInfo.InvariantCulture));
                                    s[index].AddY(float.Parse(values[4].Trim(), CultureInfo.InvariantCulture));
                                    s[index].AddZ(float.Parse(values[5].Trim(), CultureInfo.InvariantCulture));
                                }
                            }

                            var row = "";
                            for (int i = 0; i < sen.Count; i++)
                            {
                                row += s[i].ToString() + ",";
                            }
                            if (!row.Contains("NaN"))
                            {
                                lines.Add(row + act);
                            }
                            Console.WriteLine();
                        }
                    }
                }
            }
            System.IO.File.WriteAllLines(System.IO.Directory.GetCurrentDirectory() + "\\actividad.arff", lines);
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
    }
}
