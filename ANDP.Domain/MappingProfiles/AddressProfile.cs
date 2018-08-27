using Common.Lib.Mapping;

namespace ANDP.Lib.Domain.MappingProfiles
{
    public class AddressProfile : CommonMappingProfile
    {
        protected override void Configure()
        {
            //CreateMap<Address, Repositories.Order.Add>()

            //    //Ignores
            //    //.ForMember(dest => dest.StatusTypeId, opt => opt.Ignore())

            //    //Mappings that are the same name
            //    //.ForMember(dest => dest.CreatedByUser, opt => opt.MapFrom(src => src.CreatedByUser))

            //;
        }
    }
}
