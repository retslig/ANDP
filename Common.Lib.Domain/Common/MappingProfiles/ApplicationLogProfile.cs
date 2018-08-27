using Common.Lib.Mapping;

namespace Common.Lib.Domain.Common.MappingProfiles
{
    public class ApplicationLogProfile : CommonMappingProfile
    {
        protected override void Configure()
        {
            CreateMap<Data.Repositories.ApplicationLog.ApplicationLog, Domain.Common.Models.ApplicationLog>()
                .ForMember(dst => dst.AppCode, opt => opt.MapFrom(src => src.AppCode))
                .ForMember(dst => dst.DestinationMachineName, opt => opt.MapFrom(src => src.DestinationMachineName))
                .ForMember(dst => dst.ExceptionMessage, opt => opt.MapFrom(src => src.ExceptionMessage))
                .ForMember(dst => dst.LogId, opt => opt.MapFrom(src => src.LogId))
                .ForMember(dst => dst.LoggedDateTime, opt => opt.MapFrom(src => src.LoggedDateTime))
                .ForMember(dst => dst.MessageData, opt => opt.MapFrom(src => src.MessageData))
                .ForMember(dst => dst.MessageType, opt => opt.MapFrom(src => src.MessageType))
                .ForMember(dst => dst.Pid, opt => opt.MapFrom(src => src.Pid))
                .ForMember(dst => dst.SearchKey, opt => opt.MapFrom(src => src.SearchKey))
                .ForMember(dst => dst.SourceMachineName, opt => opt.MapFrom(src => src.SourceMachineName))
                .ForMember(dst => dst.StackTrace, opt => opt.MapFrom(src => src.StackTrace))
                .ForMember(dst => dst.UserCode, opt => opt.MapFrom(src => src.UserCode))
                //.ForMember(dest => dest.UserId, opt => opt.Ignore())
                ;
        }
    }
}
