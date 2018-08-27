using System.Web;

namespace Common.Lib.MVC.Providers.Resource
{
    public partial class ResourceFactory : IResourceFactory
    {
        public ResourceFactory()
            : this(new HttpContextWrapper(HttpContext.Current))
        {
        }

        public ResourceFactory(HttpContextBase httpContext)
        {
            Context = httpContext;
        }

        public HttpContextBase Context { get; private set; }
    }

    public partial class ResourceFactory
    {
        public string GetLocalResourceObject(string resourceType, string resourceKey)
        {
            return Context.GetLocalResourceObject(resourceType, resourceKey).ToString();
        }
    }
}