using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PerformanceLabsFramework.Helpers
{
    internal class PerformanceLabsMathUtility
    {
        internal Double CalculateStandardDeviation(IList<Double> listValues)
        {
            Double result = 0;
            if (listValues.Count > 0)
            {
                Double avg = listValues.Average();
                Double sumOfSquaredDifferences = listValues.Sum(d => Math.Pow(d - avg, 2));
                result = Math.Sqrt(((sumOfSquaredDifferences) / (listValues.Count() - 1)));  //OR listValues.Count()
            }
            return result;
        }

        internal Double CalculateZScore(IList<Double> listValues, Double value)
        {
            Double result = 0;

            Double stdDev = CalculateStandardDeviation(listValues);
            Double avg =  listValues.Average();

            result = (value - avg) / stdDev;

            return result;
        }
    }
}
