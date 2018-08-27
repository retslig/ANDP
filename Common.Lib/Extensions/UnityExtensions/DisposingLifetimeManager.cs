
using System;
using Microsoft.Practices.Unity;

namespace Common.Lib.Extensions.UnityExtensions
{
    public abstract class DisposingLifetimeManager : SynchronizedLifetimeManager
    {
        public abstract bool AppliesTo(object instance);
        public abstract void RemoveValue(object instance);
    }
}