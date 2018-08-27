using System;

namespace Common.Lib.Infastructure
{
    public abstract class ModelBase
    {
        public int CreatedById { get; set; }
        public int ModifiedById { get; set; }
        public DateTime DateCreated { get; private set; }
        public DateTime DateModified { get; private set; }
        public int Version { get; private set; }
    }
}
