using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace HLIOSCore
{
	public static class Helper
	{
		static public double InchToFeet(double valueInInch)
		{
			return valueInInch / 12;
		}

		static public double SquareFeetToMeters(double valueInSF)
		{
			return valueInSF * 0.09290304;
		}

		static public double KgPHaToLbsPAcre(double valueInKgPHa)
		{
			return valueInKgPHa / 1.12;
		}

		static public double MlToG(double valueInMl, double KgPerBu)
		{
			var ret = valueInMl * KgPerBu / 36.369;
			return ret;
		}

		static public ResultsStr Calc(InputParas paras)
		{
			if (paras.CurrCutWidth == -1 || paras.CurrSieveWidth == -1 || paras.CurrSeedLossInput == -1 ||
				paras.CurrCollectingAreasqft == -1 || paras.CurrCrop == null)
			{
                return null;
			}
			var _currSeedLossInG = paras.CurrSeedLossInput;
			if (paras.CurrChoice == MethodChoice.Count)
			{
				_currSeedLossInG = paras.CurrSeedLossInput * paras.CurrCrop.KernelWeight / 1000;
			}
			else if (paras.CurrChoice == MethodChoice.Volume)
			{
				_currSeedLossInG = Helper.MlToG(paras.CurrSeedLossInput, paras.CurrCrop.KgPBushel);
			}


			var concenFactor = paras.CurrCutWidth / Helper.InchToFeet(paras.CurrSieveWidth);

			var collectingAreasi = Helper.SquareFeetToMeters(paras.CurrCollectingAreasqft);

			var lph = 10 * _currSeedLossInG / concenFactor / collectingAreasi;
			var lpalbs = Helper.KgPHaToLbsPAcre(lph);

			Debug.WriteLine("Loss per ha: {0}", lph);
			Debug.WriteLine("Loss per acre lbs: {0}", lpalbs);

			var ret = new ResultsStr();

			ret.LpaLbs = lpalbs.ToString("F");


			var lpabu = lpalbs / paras.CurrCrop.LbsPBushel;
			Debug.WriteLine("Loss per acre bu: {0}", lpabu);
			ret.LpaBu = lpabu.ToString("F");
			if (paras.CurrExpectedYield != -1)
			{
				var percentLoss = lpabu / paras.CurrExpectedYield;
				Debug.WriteLine("Percent loss: {0}", percentLoss);
				ret.PercentLoss = percentLoss.ToString("P");
			}
			if (paras.CurrPrice != -1)
			{
				var lossValue = paras.CurrPrice * lpabu;
				Debug.WriteLine("Loss value: {0}", lossValue);
				ret.LossValue = lossValue.ToString("C");
			}

            return ret;
		}

		static public IEnumerable<string> ReadLines(Func<Stream> streamProvider,
			Encoding encoding)
		{
			using (var stream = streamProvider())
			using (var reader = new StreamReader(stream, encoding))
			{
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					yield return line;
				}
			}
		}
	}
}

