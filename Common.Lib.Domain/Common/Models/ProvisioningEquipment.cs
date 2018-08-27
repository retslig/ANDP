
namespace Common.Lib.Domain.Common.Models
{
    public class ProvisioningEquipment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public EquipmentType EquipmentType { get; set; }
        public int CompanyId { get; set; } 
        public EquipmentConnectionSetting EquipmentConnectionSettings { get; set; } 
    }
}
