using ANDP.Lib.Data.Repositories.Equipment;
using Common.Lib.Mapping;

namespace ANDP.Lib.Domain.MappingProfiles
{
    public class EquipmentConnectionLoginSequencesToLoginSequencesProfile : CommonMappingProfile
    {
        protected override void Configure()
        {
            CreateMap<EquipmentConnectionLoginSequence, Common.Lib.Domain.Common.Models.LogSequence>()
                .ForMember(dest => dest.Command, opt => opt.MapFrom(src => src.Command))
                .ForMember(dest => dest.ExpectedResponse, opt => opt.MapFrom(src => src.ExpectedResponse))
                .ForMember(dest => dest.Timeout, opt => opt.MapFrom(src => src.Timeout))
                .ForMember(dest => dest.SequenceNumber, opt => opt.MapFrom(src => src.SequenceNumber))
                ;
        }
    }
}
