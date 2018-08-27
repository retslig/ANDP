
namespace Common.Lib.MVC.Providers.Resource
{
    public interface IResourceFactory
    {
        string GetLocalResourceObject(string resourceType, string resourceKey);
    }
}
