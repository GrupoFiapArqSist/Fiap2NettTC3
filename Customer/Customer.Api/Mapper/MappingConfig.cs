using AutoMapper;
using Customer.Domain.Dtos.Auth;
using Customer.Domain.Dtos.User;
using Customer.Domain.Entities;

namespace Customer.Api.Mapper
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                #region Auth
                config.CreateMap<RegisterDto, User>().ReverseMap();
                #endregion

                #region User
                config.CreateMap<UserResponseDto, User>().ReverseMap();
                config.CreateMap<UpdateUserDto, User>().ReverseMap();
                #endregion
            });
            return mappingConfig;
        }
    }
}
