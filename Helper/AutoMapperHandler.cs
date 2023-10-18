using AutoMapper;
using LibraryManagement.API.Modal;
using LibraryManagement.API.Repos.Models;

namespace LibraryManagement.API.Helper
{
    public class AutoMapperHandler : Profile
    {
        public AutoMapperHandler()
        {
            //CreateMap<Book, BookModal>().ForMember(item => item.PriceCheck, opt => opt.MapFrom(item => item.Price > 1100 ? "High Price" : "Good Price")).ReverseMap();
            CreateMap<Book, BookModal>()
              .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
              .ReverseMap();
            CreateMap<User, UserModal>().ReverseMap();
            CreateMap<BookUpdateModal, Book>().ReverseMap();
            CreateMap<Category, CategoryModal>().ReverseMap();
            //CreateMap<BookIssue, IssueDTO>()
            //    .ForMember(dest => dest.Book, opt => opt.MapFrom(src => src.BookId));
        }
    }
}
