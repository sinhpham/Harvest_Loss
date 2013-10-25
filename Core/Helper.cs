using System;

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
    }
}

