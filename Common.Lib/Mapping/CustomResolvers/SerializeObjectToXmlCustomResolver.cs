using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Common.Lib.Extensions;

namespace Common.Lib.Mapping.CustomResolvers
{
    public class SerializeObjectToXmlCustomResolver : ValueResolver<object, string>
    {
        protected override string ResolveCore(object myObject)
        {
            var xml = myObject.SerializeObjectWithNoNameSpaceToString();
            //myObject
            return xml;
        }
    }
}

