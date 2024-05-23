using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Devoiture.Models;

public partial class Devoiture1Context : DbContext
{
    public Devoiture1Context()
    {
    }

    public Devoiture1Context(DbContextOptions<Devoiture1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<HangXe> HangXes { get; set; }

    public virtual DbSet<HinhAnhXe> HinhAnhXes { get; set; }

    public virtual DbSet<Hinhthucthanhtoan> Hinhthucthanhtoans { get; set; }

    public virtual DbSet<HoadonThuexe> HoadonThuexes { get; set; }

    public virtual DbSet<HopDongThueXe> HopDongThueXes { get; set; }

    public virtual DbSet<Khuvuc> Khuvucs { get; set; }

    public virtual DbSet<LichChoThue> LichChoThues { get; set; }

    public virtual DbSet<LoaiXe> LoaiXes { get; set; }

    public virtual DbSet<MauXe> MauXes { get; set; }

    public virtual DbSet<Quyen> Quyens { get; set; }

    public virtual DbSet<Taikhoan> Taikhoans { get; set; }

    public virtual DbSet<Trangthaithanhtoan> Trangthaithanhtoans { get; set; }

    public virtual DbSet<Website> Websites { get; set; }

    public virtual DbSet<Xe> Xes { get; set; }

    public virtual DbSet<Yeucauthuexe> Yeucauthuexes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<HangXe>(entity =>
        {
            entity.HasKey(e => e.MaHx).HasName("PK__HangXe__2725A6D47E890028");

            entity.ToTable("HangXe");

            entity.Property(e => e.MaHx).HasColumnName("MaHX");
            entity.Property(e => e.TenHx)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("TenHX");
        });

        modelBuilder.Entity<HinhAnhXe>(entity =>
        {
            entity.HasKey(e => e.MaAnh).HasName("PK__HinhAnhX__356240DF40C94D59");

            entity.ToTable("HinhAnhXe");

            entity.Property(e => e.Biensoxe)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Hinh).HasMaxLength(100);

            entity.HasOne(d => d.BiensoxeNavigation).WithMany(p => p.HinhAnhXes)
                .HasForeignKey(d => d.Biensoxe)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HinhAnhXe__Biens__6A30C649");
        });

        modelBuilder.Entity<Hinhthucthanhtoan>(entity =>
        {
            entity.HasKey(e => e.MaHt).HasName("PK__Hinhthuc__2725A6D050ECABB5");

            entity.ToTable("Hinhthucthanhtoan");

            entity.Property(e => e.MaHt)
                .ValueGeneratedNever()
                .HasColumnName("MaHT");
            entity.Property(e => e.TenHt)
                .HasMaxLength(24)
                .HasColumnName("TenHT");
        });

        modelBuilder.Entity<HoadonThuexe>(entity =>
        {
            entity.HasKey(e => e.MaHd).HasName("PK_Hoadon");

            entity.ToTable("HoadonThuexe");

            entity.Property(e => e.MaHd).HasColumnName("MaHD");
            entity.Property(e => e.Biensoxe)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.MaHdong).HasColumnName("MaHDong");
            entity.Property(e => e.Matrangthai).HasColumnName("matrangthai");
            entity.Property(e => e.NgaylapHd)
                .HasColumnType("datetime")
                .HasColumnName("NgaylapHD");

            entity.HasOne(d => d.BiensoxeNavigation).WithMany(p => p.HoadonThuexes)
                .HasForeignKey(d => d.Biensoxe)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HoadonThuexe_Xe");

            entity.HasOne(d => d.EmailNavigation).WithMany(p => p.HoadonThuexes)
                .HasForeignKey(d => d.Email)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HoadonThuexe_Taikhoan");

            entity.HasOne(d => d.MaHdongNavigation).WithMany(p => p.HoadonThuexes)
                .HasForeignKey(d => d.MaHdong)
                .HasConstraintName("FK_HoadonThuexe_HopDongThueXe");

            entity.HasOne(d => d.MatrangthaiNavigation).WithMany(p => p.HoadonThuexes)
                .HasForeignKey(d => d.Matrangthai)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HoadonThuexe_Trangthaithanhtoan");
        });

        modelBuilder.Entity<HopDongThueXe>(entity =>
        {
            entity.HasKey(e => e.MaHdong);

            entity.ToTable("HopDongThueXe");

            entity.Property(e => e.MaHdong).HasColumnName("MaHDong");
            entity.Property(e => e.Chuki).HasMaxLength(100);
            entity.Property(e => e.DieukhoantuChuXe).HasColumnType("ntext");
            entity.Property(e => e.Email)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.MaYc).HasColumnName("MaYC");

            entity.HasOne(d => d.MaHtNavigation).WithMany(p => p.HopDongThueXes)
                .HasForeignKey(d => d.MaHt)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HopDongThueXe_Hinhthucthanhtoan");

            entity.HasOne(d => d.MaYcNavigation).WithMany(p => p.HopDongThueXes)
                .HasForeignKey(d => d.MaYc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HopDongThueXe_Yeucauthuexe");
        });

        modelBuilder.Entity<Khuvuc>(entity =>
        {
            entity.HasKey(e => e.MaKv);

            entity.ToTable("Khuvuc");

            entity.Property(e => e.MaKv).HasColumnName("MaKV");
            entity.Property(e => e.TenKv)
                .HasMaxLength(50)
                .HasColumnName("TenKV");
        });

        modelBuilder.Entity<LichChoThue>(entity =>
        {
            entity.HasKey(e => e.MaLich);

            entity.ToTable("LichChoThue");

            entity.Property(e => e.Biensx)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Idyc).HasColumnName("IDyc");
            entity.Property(e => e.Ngaynhanxe).HasColumnType("datetime");
            entity.Property(e => e.Ngaytraxe).HasColumnType("datetime");

            entity.HasOne(d => d.BiensxNavigation).WithMany(p => p.LichChoThues)
                .HasForeignKey(d => d.Biensx)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LichChoThue_Xe");

            entity.HasOne(d => d.IdycNavigation).WithMany(p => p.LichChoThues)
                .HasForeignKey(d => d.Idyc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LichChoThue_Yeucauthuexe");
        });

        modelBuilder.Entity<LoaiXe>(entity =>
        {
            entity.HasKey(e => e.MaLoai).HasName("PK__LoaiXe__730A5759EE31DDAA");

            entity.ToTable("LoaiXe");

            entity.Property(e => e.TenLoai).HasMaxLength(24);
        });

        modelBuilder.Entity<MauXe>(entity =>
        {
            entity.HasKey(e => e.MaMx);

            entity.ToTable("MauXe");

            entity.Property(e => e.MaMx).HasColumnName("MaMX");
            entity.Property(e => e.MaHx).HasColumnName("MaHX");
            entity.Property(e => e.TenMx)
                .HasMaxLength(50)
                .HasColumnName("TenMX");

            entity.HasOne(d => d.MaHxNavigation).WithMany(p => p.MauXes)
                .HasForeignKey(d => d.MaHx)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MauXe_HangXe");
        });

        modelBuilder.Entity<Quyen>(entity =>
        {
            entity.HasKey(e => e.MaQuyen).HasName("PK__Quyen__1D4B7ED4CD60832B");

            entity.ToTable("Quyen");

            entity.Property(e => e.TenQuyen).HasMaxLength(50);
        });

        modelBuilder.Entity<Taikhoan>(entity =>
        {
            entity.HasKey(e => e.Email).HasName("PK__Khachhan__A9D10535ACE187CE");

            entity.ToTable("Taikhoan");

            entity.Property(e => e.Email)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.HinhCccd)
                .HasMaxLength(100)
                .HasColumnName("HinhCCCD");
            entity.Property(e => e.HinhDaiDien).HasMaxLength(100);
            entity.Property(e => e.HinhGplxb2)
                .HasMaxLength(100)
                .HasColumnName("HinhGPLXB2");
            entity.Property(e => e.HoTen).HasMaxLength(30);
            entity.Property(e => e.IdQuyen).HasColumnName("ID_Quyen");
            entity.Property(e => e.Matkhau).HasMaxLength(100);
            entity.Property(e => e.Ngsinh).HasColumnType("datetime");
            entity.Property(e => e.Online).HasDefaultValue(false);
            entity.Property(e => e.Sdt)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.SoCccd)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("SoCCCD");
            entity.Property(e => e.SoGplxB2)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.Username).HasMaxLength(30);

            entity.HasOne(d => d.IdQuyenNavigation).WithMany(p => p.Taikhoans)
                .HasForeignKey(d => d.IdQuyen)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Taikhoan_Quyen");
        });

        modelBuilder.Entity<Trangthaithanhtoan>(entity =>
        {
            entity.HasKey(e => e.MaTrangthaiHd);

            entity.ToTable("Trangthaithanhtoan");

            entity.Property(e => e.MaTrangthaiHd)
                .ValueGeneratedNever()
                .HasColumnName("MaTrangthaiHD");
            entity.Property(e => e.NoiDung).HasColumnType("ntext");
        });

        modelBuilder.Entity<Website>(entity =>
        {
            entity.HasKey(e => e.MaWebsite);

            entity.ToTable("Website");

            entity.Property(e => e.MaWebsite).HasColumnName("maWebsite");
            entity.Property(e => e.MaQuyen).HasColumnName("maQuyen");
            entity.Property(e => e.TenWebsite).HasMaxLength(50);

            entity.HasOne(d => d.MaQuyenNavigation).WithMany(p => p.Websites)
                .HasForeignKey(d => d.MaQuyen)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Website_Quyen");
        });

        modelBuilder.Entity<Xe>(entity =>
        {
            entity.HasKey(e => e.Biensoxe).HasName("PK__Xe__C58311A237A97495");

            entity.ToTable("Xe");

            entity.Property(e => e.Biensoxe)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Diachixe).HasColumnType("ntext");
            entity.Property(e => e.Dieukhoanthuexe).HasColumnType("ntext");
            entity.Property(e => e.Email)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Hinhanh).HasMaxLength(100);
            entity.Property(e => e.Loainhienlieu).HasMaxLength(20);
            entity.Property(e => e.MaMx).HasColumnName("MaMX");
            entity.Property(e => e.MotaDacDiemChucNang).HasColumnType("ntext");
            entity.Property(e => e.NamSx)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("NamSX");

            entity.HasOne(d => d.EmailNavigation).WithMany(p => p.Xes)
                .HasForeignKey(d => d.Email)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Xe__Email__6754599E");

            entity.HasOne(d => d.MaLoaiNavigation).WithMany(p => p.Xes)
                .HasForeignKey(d => d.MaLoai)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Xe__MaLoai__6B24EA82");

            entity.HasOne(d => d.MaMxNavigation).WithMany(p => p.Xes)
                .HasForeignKey(d => d.MaMx)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Xe_MauXe");

            entity.HasOne(d => d.MakvNavigation).WithMany(p => p.Xes)
                .HasForeignKey(d => d.Makv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Xe_Khuvuc");
        });

        modelBuilder.Entity<Yeucauthuexe>(entity =>
        {
            entity.HasKey(e => e.MaYc).HasName("PK_Yeucauthuexe_1");

            entity.ToTable("Yeucauthuexe");

            entity.Property(e => e.MaYc).HasColumnName("MaYC");
            entity.Property(e => e.Biensoxe)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Diadiemnhanxe).HasMaxLength(100);
            entity.Property(e => e.Email)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Ngaynhanxe).HasColumnType("datetime");
            entity.Property(e => e.Ngaytraxe).HasColumnType("datetime");

            entity.HasOne(d => d.BiensoxeNavigation).WithMany(p => p.Yeucauthuexes)
                .HasForeignKey(d => d.Biensoxe)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Yeucauthuexe_Xe");

            entity.HasOne(d => d.EmailNavigation).WithMany(p => p.Yeucauthuexes)
                .HasForeignKey(d => d.Email)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Yeucauthuexe_Taikhoan");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
