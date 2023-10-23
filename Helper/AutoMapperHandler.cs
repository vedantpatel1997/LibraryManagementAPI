using AutoMapper;
using LibraryManagement.API.Modal;
using LibraryManagement.API.Repos.Models;

namespace LibraryManagement.API.Helper
{
    public class AutoMapperHandler : Profile
    {
        public AutoMapperHandler()
        {
            CreateMap<Book, BookModal>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title.Trim()))
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author.Trim()))
                .ReverseMap();

            CreateMap<User, UserModal>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName.Trim()))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName.Trim()))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.Trim()))
                .ReverseMap();

            CreateMap<BookUpdateModal, Book>().ReverseMap();
            CreateMap<Category, CategoryModal>().ReverseMap();
            CreateMap<Address, AddressModal>().ReverseMap();

            CreateMap<BookIssue, IssueDTO>()
                .ForMember(dest => dest.Book, opt => opt.MapFrom(src => src.Book))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));
        }
    }
}
