using System;
using System.Collections.Generic;
using System.Text;

namespace SignalCaracteristics
{
    class SensorData
    {
        public List<float> x { get; set; }
        public List<float> y { get; set; }
        public List<float> z { get; set; }
        public List<float> mag { get; set; }

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

        public SensorData()
        {
            this.x = new List<float>();
            this.y = new List<float>();
            this.z = new List<float>();
            this.mag = new List<float>();
        }

        public void AddX(float value) {
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
    }
}
