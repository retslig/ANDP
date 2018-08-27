using System;

namespace Common.Lib.Utility
{
    public class MathHelper
    {

        public decimal TruncateDecimal(decimal value, int precision)
        {
            decimal step = (decimal)Math.Pow(10, precision);
            int tmp = (int)Math.Truncate(step * value);
            return tmp / step;
        }

        public double TruncateDouble(double value, int precision)
        {
            double step = (double)Math.Pow(10, precision);
            int tmp = (int)Math.Truncate(step * value);
            return tmp / step;
        }
    }
}
