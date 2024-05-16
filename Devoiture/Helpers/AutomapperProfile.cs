using AutoMapper;
using Devoiture.Models;
using Devoiture.ViewModel;

namespace Devoiture.Helpers
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<DangkyKH_VM,Taikhoan>()
                .ForMember(kh => 
                kh.Email, option => option.MapFrom(DangkyKH_VM => 
                DangkyKH_VM.Email)).ReverseMap();
            CreateMap<ThemxeVM, Xe>()
                .ForMember(xe => xe.Biensoxe, option => option.MapFrom(ThemxeVM => 
                ThemxeVM.Biensoxe)).ReverseMap();
        }
    }
}
