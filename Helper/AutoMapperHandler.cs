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
            CreateMap<Book, BookModal>().ReverseMap();
            CreateMap<User, UserModal>().ReverseMap();
        }
    }
}
