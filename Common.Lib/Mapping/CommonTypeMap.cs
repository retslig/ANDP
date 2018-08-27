
using AutoMapper;

namespace Common.Lib.Mapping
{
    public class CommonTypeMap : TypeMap
    {
        public CommonTypeMap(TypeInfo sourceType, TypeInfo destinationType, MemberList memberList) : base(sourceType, destinationType, memberList)
        {
        }
    }
}
