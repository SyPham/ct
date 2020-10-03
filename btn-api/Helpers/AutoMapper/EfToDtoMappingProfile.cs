using btn_api.DTO;
using btn_api.Models;
using AutoMapper;
using System.Linq;

namespace btn_api.Helpers.AutoMapper
{
    public class EfToDtoMappingProfile : Profile
    {
        char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        public EfToDtoMappingProfile()
        {
            CreateMap<LineInfo, LineInfoDto>().ForMember(d => d.LineName, o => o.MapFrom(s => s.Building.Name)); 
            CreateMap<Button, ButtonDto>();
            CreateMap<Worker, WorkerDto>();
            CreateMap<User, UserForDetailDto>();
            CreateMap<Line, LineDto>();
            CreateMap<Building, BuildingDto>();
            CreateMap<BuildingUser, BuildingUserDto>();

        }

    }
}