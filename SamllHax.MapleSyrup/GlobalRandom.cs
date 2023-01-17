using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup
{
    public static class GlobalRandom
    {
        private static Random _random = new Random();

        public static int GetNext(int maxValue)
        {
            return _random.Next(maxValue);
        }

        public static int GetNext(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue);
        }

        public static double GetNextDouble()
        {
            return _random.NextDouble();
        }

        public static double GetNextDouble(double minValue, double maxValue)
        {
            var range = maxValue - minValue;
            return minValue + _random.NextDouble() * range;
        }

        public static T GetRandomElement<T>(IEnumerable<T> enumerable)
        {
            var maxValue = enumerable.Count();
            var index = GetNext(maxValue);
            return enumerable.ElementAt(index);
        }
    }
}
