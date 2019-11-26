using AutoMapper;
using WebApi.Core.Domain.Entities;
using WebApi.Infrastructure.Identity;

namespace WebApi.Infrastructure.Data.Mapper
{
    public class DataProfile : Profile
    {
        public DataProfile()
        {
            CreateMap<Player, AppUser>().ConstructUsing(u => new AppUser {UserName = u.UserName, Email = u.Email}).ForMember(au=>au.Id,opt=>opt.Ignore());
            CreateMap<AppUser, Player>().ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email)).
                                       ForMember(dest=> dest.PasswordHash, opt=> opt.MapFrom(src=>src.PasswordHash)).
                                       ForAllOtherMembers(opt=>opt.Ignore());
            

        }
    }
}
