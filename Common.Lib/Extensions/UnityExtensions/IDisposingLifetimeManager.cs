
namespace Common.Lib.Extensions.UnityExtensions
{
    public interface IDisposingLifetimeManager 
    {
        bool AppliesTo(object instance);
        void RemoveValue(object instance);
    }
}
