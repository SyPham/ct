using btn_api.DTO;
using btn_api.Models;
using AutoMapper;
using System;

namespace btn_api.Helpers.AutoMapper
{
    public class DtoToEfMappingProfile : Profile
    {
     
        public DtoToEfMappingProfile()
        {
            CreateMap<LineInfoDto, LineInfo>();
            CreateMap<ButtonDto, Button>();
            CreateMap<WorkerDto, Worker>();

            CreateMap<UserForDetailDto, User>();
            CreateMap<BatchDto, User>();
         
            CreateMap<LineDto, Line>();

        
            CreateMap<BuildingDto, Building>();
            CreateMap<BuildingUserDto, BuildingUser>();
           
         
            //CreateMap<AuditTypeDto, MES_Audit_Type_M>();
        }
    }
}