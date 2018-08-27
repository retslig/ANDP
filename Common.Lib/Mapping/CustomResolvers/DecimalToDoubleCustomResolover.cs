using System;

namespace Common.Lib.Mapping.CustomResolvers
{
    using AutoMapper;

    public class DecimalToDoubleCustomResolver : ValueResolver<decimal, double>
    {
        protected override double ResolveCore(decimal source)
        {
            return Convert.ToDouble(source);
        }

    }
}
