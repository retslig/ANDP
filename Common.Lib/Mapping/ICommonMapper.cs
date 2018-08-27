
using Common.Lib.Mapping;

namespace Common.Lib.Mapping
{
    public interface ICommonMapper
    {
        CommonTypeMap FindTypeMapFor(System.Type sourceType, System.Type destinationType);
        CommonTypeMap FindTypeMapFor<TSource, TDestination>();
        CommonTypeMap[] GetAllTypeMaps();
        TDestination Map<TSource, TDestination>(TSource source);
        object Map(object source, System.Type sourceType, System.Type destinationType);
    }
}
