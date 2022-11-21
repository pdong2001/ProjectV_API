using AutoMapper;
using Data.Models;
using Services.Contracts.Users;

namespace API
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>(MemberList.None)
            .ForMember(u => u.Roles, o => o.MapFrom(s => string.IsNullOrEmpty(s.Roles) ? null : s.Roles.Split(',', StringSplitOptions.TrimEntries)));

            CreateMap<CreateUpdateUserDto, User>(MemberList.None)
                .ForMember(u => u.Roles, o => o.MapFrom(s => s.Roles == null ? null : String.Join(',', s.Roles)));
        }
    }
}
