using AutoMapper;
using netCore_Begginer.Models;

namespace netCore_Begginer.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DailyTasks, EditDailyTasks>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember, destMember) => srcMember != null && !Equals(srcMember, destMember)));
        }
    }
}
