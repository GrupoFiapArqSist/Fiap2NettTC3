using AutoMapper;
using Event.Domain.Dtos.Event;

namespace Event.Api.Mapper;

public class MappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            #region Auth
            // ajustar
            //config.CreateMap<RegisterDto, User>().ReverseMap();
            #endregion

            #region Event
            config.CreateMap<EventDto, Domain.Entities.Event>().ReverseMap();
            config.CreateMap<AddEventDto, Domain.Entities.Event>().ReverseMap();
            config.CreateMap<UpdateEventDto, Domain.Entities.Event>().ReverseMap();
            #endregion

        });
        return mappingConfig;
    }
}
