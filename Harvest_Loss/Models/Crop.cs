using System;

namespace Harvest_Loss
{
    public class Crop
    {
        public string Name { get; set; }

        public double LbsPBushel{ get; set; }

        public double BushelPTonne{ get; set; }

        public double KgPBushel{ get; set; }

        public double KernelWeight{ get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}

