using AutoMapper;
using Data.Dtos;
using Data.Entities;

namespace WebApi
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<ApiUser, UserDto>().ReverseMap();
        }
    }
}
