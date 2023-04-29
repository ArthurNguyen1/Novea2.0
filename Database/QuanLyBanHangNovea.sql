set dateformat dmy

CREATE DATABASE QuanLyBanHangNovea

USE QuanLyBanHangNovea
---------------------
CREATE TABLE ADMINIS
(
	MAND varchar(128) primary key,
	TAIKHOAN varchar(max),
	MATKHAU varchar(max),
	HOTEN nvarchar(max),
	NGSINH datetime,
	GIOITINH nvarchar(3),
	SDT varchar(15),
	EMAIL varchar(max),
	NGDK datetime,
	AVATAR varbinary(max),
	REASONBANNING bit,
	STATU bit
) 

CREATE TABLE SHIPPER
(
	MAND varchar(128) primary key,
	TAIKHOAN varchar(max),
	MATKHAU varchar(max),
	HOTEN nvarchar(max),
	NGSINH datetime,
	GIOITINH nvarchar(3),
	SDT varchar(15),
	EMAIL varchar(max),
	NGDK datetime,
	LUONG money,
	SHIPSTATUS bit,
	AVATAR varbinary(max),
	REASONBANNING bit,
	STATU bit
)

CREATE TABLE KHACH
(
	MAND varchar(128) primary key,
	TAIKHOAN varchar(max),
	MATKHAU varchar(max),
	HOTEN nvarchar(max),
	NGSINH datetime,
	GIOITINH nvarchar(3),
	SDT varchar(15),
	EMAIL varchar(max),
	DIACHI nvarchar(max),
	NGDK datetime,
	DOANHSO money,
	AVATAR varbinary(max),
	REASONBANNING bit,
	STATU bit
)

CREATE TABLE CUAHANG
(
	MACH varchar(128) primary key,
	TAIKHOAN varchar(max),
	MATKHAU varchar(max),
	TENCH nvarchar(max),
	DIADIEM nvarchar(max),
	SDT varchar(15),
	EMAIL varchar(max),
	NGDK datetime,	
	DOANHTHU money,
	AVATAR varbinary(max),
	REASONBANNING bit,
	STATU bit
)

CREATE TABLE SANPHAM
(
	MASP varchar(128) primary key,
	TENSP nvarchar(max),
	LOAISP nvarchar(max),
	DONVI nvarchar(max),
	DONGIA money,
	SIZE varchar(10),
	MOTA nvarchar(max),
	AVAILABLE bit,
	HINHSP varbinary(max),
	AVERAGERATE float,
	NGHIEULUC datetime,
	MACH varchar(128) FOREIGN KEY REFERENCES CUAHANG(MACH)
)

CREATE TABLE HOADON
(
	SOHD varchar(128) primary key,
	NGMH datetime,
	TONGTIEN money,
	STATU int,
	MAND_KHACH varchar(128) FOREIGN KEY REFERENCES KHACH(MAND),
	MAND_SHIPPER varchar(128) FOREIGN KEY REFERENCES SHIPPER(MAND),
	MACH varchar(128) FOREIGN KEY REFERENCES CUAHANG(MACH),
)

CREATE TABLE CTHD
(
	SOCTHD varchar(128) primary key,
	SOLUONG int,
	TRIGIA money,
	CHUTHICH varchar(64),
	SOHD varchar(128) FOREIGN KEY REFERENCES HOADON(SOHD),
	MASP varchar(128) FOREIGN KEY REFERENCES SANPHAM(MASP),
)

CREATE TABLE DANHGIA
(
	MADG varchar(128) primary key,
	NGAYDG datetime,
	COMMENT nvarchar(max),
	RATE int,
	MAND_KHACH varchar(128) FOREIGN KEY REFERENCES KHACH(MAND),
	MASP varchar(128) FOREIGN KEY REFERENCES SANPHAM(MASP),
)
