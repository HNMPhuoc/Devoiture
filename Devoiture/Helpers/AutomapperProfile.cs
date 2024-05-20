using AutoMapper;
using Devoiture.Models;
using Devoiture.ViewModel;

namespace Devoiture.Helpers
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<DangkyKH_VM, Taikhoan>()
                .ForMember(kh =>
                kh.Email, option => option.MapFrom(DangkyKH_VM =>
                DangkyKH_VM.Email)).ReverseMap();
            CreateMap<ThemxeVM, Xe>()
                .ForMember(xe => xe.Biensoxe, option => option.MapFrom(ThemxeVM =>
                ThemxeVM.Biensoxe)).ReverseMap();
            CreateMap<Taikhoan, ChitietNV_VM>()
            .ForMember(nv => nv.TenQuyen, opt => opt.MapFrom(src => src.IdQuyenNavigation.TenQuyen));
            CreateMap<Taikhoan, ChitietKH_VM>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.HoTen, opt => opt.MapFrom(src => src.HoTen))
                .ForMember(dest => dest.Sdt, opt => opt.MapFrom(src => src.Sdt))
                .ForMember(dest => dest.Ngsinh, opt => opt.MapFrom(src => src.Ngsinh))
                .ForMember(dest => dest.HinhDaiDien, opt => opt.MapFrom(src => src.HinhDaiDien))
                .ForMember(dest => dest.Gioitinh, opt => opt.MapFrom(src => src.Gioitinh))
                .ForMember(dest => dest.Lock, opt => opt.MapFrom(src => src.Lock))
                .ForMember(dest => dest.Online, opt => opt.MapFrom(src => src.Online))
                .ForMember(dest => dest.SoGplxb2, opt => opt.MapFrom(src => src.SoGplxB2))
                .ForMember(dest => dest.HinhGplx, opt => opt.MapFrom(src => src.HinhGplxb2))
                .ForMember(dest => dest.SoCccd, opt => opt.MapFrom(src => src.SoCccd))
                .ForMember(dest => dest.HinhCccd, opt => opt.MapFrom(src => src.HinhCccd))
                .ForMember(dest => dest.Soxe, opt => opt.MapFrom(src => src.Xes.Count));
            CreateMap<Xe, SuaxecuaKH_VM>()
                .ForMember(dest => dest.KhuVucList, opt => opt.Ignore()) // Không ánh xạ thuộc tính này vì chúng không được cần thiết
                .ForMember(dest => dest.MauXeList, opt => opt.Ignore()) // Tương tự như trên
                .ForMember(dest => dest.LoaiXeList, opt => opt.Ignore()); // Tương tự như trên
        }
    }
}
