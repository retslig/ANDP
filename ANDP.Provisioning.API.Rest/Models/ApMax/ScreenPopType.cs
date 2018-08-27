using System.Collections.Generic;

namespace ANDP.Provisioning.API.Rest.Models.ApMax
{
    public class ScreenPopType
    {
        public string Description { get; set; }
        public string NpaNxx { get; set; }
        public List<ScreenPopServerType> ScreenPopServerTypes { get; set; }

    }
}