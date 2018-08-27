using System;

namespace Common.Lib.Mapping.CustomResolvers
{
    using AutoMapper;

    public class ByteToDecimalCustomResolver : ValueResolver<byte, decimal>
    {
        protected override decimal ResolveCore(byte source)
        {
            return Convert.ToDecimal(source);
        }
    }
}
