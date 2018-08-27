using ANDP.Lib.Data.Repositories.Equipment;
using Common.Lib.Domain.Common.Services.ConnectionManager;
using Common.Lib.Domain.Common.Services.ConnectionManager.Socket;
using Common.Lib.Mapping;

namespace ANDP.Lib.Domain.MappingProfiles
{
    public class EquipmentConnectionSettingsProfile : CommonMappingProfile
    {
        protected override void Configure()
        {
            CreateMap<EquipmentConnectionSetting, Common.Lib.Domain.Common.Models.EquipmentConnectionSetting>()
                .ForMember(dest => dest.EquipmentId, opt => opt.Ignore())
                .ForMember(dest => dest.AuthenticationType, opt => opt.MapFrom(src => (AuthenticationType)src.EquipmentAuthenticationTypeId))
                .ForMember(dest => dest.ConnectionType, opt => opt.MapFrom(src => (ConnectionType)src.EquipmentConnectionTypeId))
                .ForMember(dest => dest.IpVersion, opt => opt.MapFrom(src => (IpVersionType)src.EquipmentIpVersionTypeId))
                .ForMember(dest => dest.Encoding, opt => opt.MapFrom(src => (EncodingType)src.EquipmentEncodingTypeId))
                .ForMember(dest => dest.LoginSequences, opt => opt.MapFrom(src => src.EquipmentConnectionLoginSequences))
                .ForMember(dest => dest.LogoutSequences, opt => opt.MapFrom(src => src.EquipmentConnectionLogoutSequences))
                ;
        }
    }
}
