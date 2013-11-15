using System;

namespace HLIOSCore
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

	public class InputParas
	{
		public MethodChoice CurrChoice { get; set; }

		public double CurrCutWidth { get; set; }

		public double CurrSieveWidth { get; set; }

		public double CurrCollectingAreasqft { get; set; }

		public double CurrExpectedYield { get; set; }

		public double CurrPrice { get; set; }

		public double CurrSeedLossInput { get; set; }

		public Crop CurrCrop{ get; set; }
	}

	public class ResultsStr
	{
		public string LpaLbs{ get; set; }

		public string LpaBu{ get; set; }

		public string PercentLoss{ get; set; }

		public string LossValue{ get; set; }
	}

	public enum MethodChoice
	{
		Weight,
		Volume,
		Count
	}


}

