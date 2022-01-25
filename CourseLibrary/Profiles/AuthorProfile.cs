using AutoMapper;
using CourseLibrary.Entities;
using CourseLibrary.Helpers;
using CourseLibrary.Models.DtosForGet;

namespace CourseLibrary.Profiles
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<Author, AuthorDto>()
                .ForMember(dest => dest.Name,
                opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.Age,
                opt => opt.MapFrom(src => src.DateOfBirth.GetCurrentAge()));
        }
    }
}
