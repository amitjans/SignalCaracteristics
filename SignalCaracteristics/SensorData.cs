using System;
using System.Collections.Generic;

namespace SignalCaracteristics
{
    internal class SensorData
    {
        public List<float> x { get; set; }
        public List<float> y { get; set; }
        public List<float> z { get; set; }

        private float mediax
        {
            get
            {
                var temp = 0.0;
                foreach (var item in this.x)
                {
                    temp += item;
                }
                temp /= this.x.Count;
                return (float)temp;
            }
        }

        private float mediay
        {
            get
            {
                var temp = 0.0;
                foreach (var item in this.y)
                {
                    temp += item;
                }
                temp /= this.y.Count;
                return (float)temp;
            }
        }

        private float mediaz
        {
            get
            {
                var temp = 0.0;
                foreach (var item in this.z)
                {
                    temp += item;
                }
                temp /= this.z.Count;
                return (float)temp;
            }
        }

        private List<float> magnitud
        {
            get
            {
                List<float> mag = new List<float>();
                for (int i = 0; i < x.Count; i++)
                {
                    mag.Add(MathF.Sqrt(MathF.Pow(x[i], 2) + MathF.Pow(y[i], 2) + MathF.Pow(z[i], 2)));
                }
                return mag;
            }
        }

        public SensorData()
        {
            this.x = new List<float>();
            this.y = new List<float>();
            this.z = new List<float>();
        }

        public void AddX(float value)
        {
            x.Add(value);
        }

        public void AddY(float value)
        {
            y.Add(value);
        }

        public void AddZ(float value)
        {
            z.Add(value);
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

        public override string ToString()
        {
            return FloatFormat(mediax) + "," + FloatFormat(mediay) + "," + FloatFormat(mediaz) + "," + FloatFormat(Media(magnitud)) + "," + FloatFormat(SD(magnitud, Media(magnitud)));
        }

        private string FloatFormat(float value)
        {
            return value.ToString().Replace(",", ".");
        }

        private float Media(List<float> list) {
            var temp = 0.0;
            foreach (var item in list)
            {
                temp += item;
            }
            temp /= list.Count;
            return (float)temp;
        }
    }
}