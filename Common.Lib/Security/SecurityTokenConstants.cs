
using System;

namespace Common.Lib.Security
{
    public static class SecurityTokenConstants
    {
        public static TimeSpan TokenLifeTime { get { return new TimeSpan(0, 18, 0, 0); } }
        public static TimeSpan TokenLifeTimeEndOfLifeThreshold { get { return new TimeSpan(0, 0, 15, 0); } }
    }
}
