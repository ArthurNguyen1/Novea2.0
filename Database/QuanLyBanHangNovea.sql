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
	REASONBANNING nvarchar(max),
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
	REASONBANNING nvarchar(max),
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
	REASONBANNING nvarchar(max),
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
	REASONBANNING nvarchar(max),
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
	CHUTHICH nvarchar(max),
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
---------------------


/*--I.Rang buoc toan ven cho KHACH--*/
--1.Doanh so ban ra cho 1 khach hang bang tong tat ca tong tien trong hoa don ma khach hang do mua
	--a.Trigger cho bang KHACH--
		---INSERT---
CREATE TRIGGER TRG_KHACH_DOANHSO_INSERT
ON KHACH
AFTER INSERT
AS
BEGIN
	DECLARE @MAND varchar(128)

	SELECT @MAND = MAND
	FROM  inserted

	UPDATE KHACH
	SET  DOANHSO = 0
	WHERE MAND = @MAND
END

	--b.Trigger cho bang HOADON--
		---INSERT, DELETE---
CREATE TRIGGER TRG_HOADON_DOANHSO_INSERT_DELETE
ON HOADON
AFTER INSERT, DELETE
AS 
BEGIN
	DECLARE @MAND varchar(128)
	DECLARE @TONGDSTHEM money
	DECLARE @TONGDSTRU money

	SET @TONGDSTHEM = 0
	SET @TONGDSTRU = 0

	SELECT @TONGDSTHEM = TONGTIEN, @MAND = MAND_KHACH 
	FROM inserted

	SELECT @TONGDSTRU = TONGTIEN, @MAND = MAND_KHACH 
	FROM deleted

	UPDATE KHACH
	SET DOANHSO = DOANHSO + @TONGDSTHEM - @TONGDSTRU
	WHERE MAND = @MAND
END

/*--II.Rang buoc toan ven cho HOADON--*/
--1.Tong tien bang tong tri gia cac chi tiet hoa don
	--a.Trigger cho bang HOADON--
		---INSERT---
CREATE TRIGGER TRG_HOADON_TONGTIEN_INSERT
ON HOADON
AFTER INSERT
AS
BEGIN
	DECLARE @SOHD varchar(128)

	SELECT @SOHD = SOHD
	FROM  inserted

	UPDATE HOADON
	SET  TONGTIEN = 0
	WHERE SOHD = @SOHD
END
		
	--b.Trigger cho bang CTHD--
		---INSERT, DELETE, UPDATE---
CREATE TRIGGER TRG_CTHD_TONGTIEN_INSERT_DELETE_UPDATE
ON CTHD
AFTER INSERT, DELETE, UPDATE
AS 
BEGIN
	DECLARE @SOHD varchar(128)
	DECLARE @TONGTRIGIATHEM money
	DECLARE @TONGTRIGIATRU money

	SET @TONGTRIGIATHEM = 0
	SET @TONGTRIGIATRU = 0

	SELECT @TONGTRIGIATHEM = TRIGIA, @SOHD = SOHD 
	FROM inserted

	SELECT @TONGTRIGIATRU = TRIGIA, @SOHD = SOHD 
	FROM deleted

	UPDATE HOADON
	SET TONGTIEN = TONGTIEN + @TONGTRIGIATHEM - @TONGTRIGIATRU
	WHERE SOHD = @SOHD
END

/*--III.Rang buoc toan ven cho CTHD--*/
--1.Tri gia bang so luong nhan don gia
	--Trigger cho bang CTHD--
		---INSERT---
CREATE TRIGGER TRG_CTHD_TRIGIA_INSERT
ON CTHD
AFTER INSERT
AS
BEGIN
	DECLARE @SOCTHD varchar(128)
	DECLARE @SOLUONG int
	DECLARE @DONGIA money

	SELECT @SOCTHD = inserted.SOCTHD, @SOLUONG = inserted.SOLUONG, @DONGIA = SANPHAM.DONGIA
	FROM inserted, SANPHAM
	WHERE inserted.MASP = SANPHAM.MASP
	
	UPDATE CTHD
	SET TRIGIA = @SOLUONG * @DONGIA
	WHERE SOCTHD = @SOCTHD
END
		---UPDATE_SOLUONG---
CREATE TRIGGER TRG_CTHD_TRIGIA_UPDATE_SOLUONG
ON CTHD
AFTER UPDATE
AS IF (UPDATE(SOLUONG))
BEGIN
	DECLARE @SOCTHD varchar(128)
	DECLARE @SOLUONG int
	DECLARE @DONGIA money

	SELECT @SOCTHD = inserted.SOCTHD, @SOLUONG = inserted.SOLUONG, @DONGIA = SANPHAM.DONGIA
	FROM inserted, SANPHAM
	WHERE inserted.MASP = SANPHAM.MASP

	UPDATE CTHD
	SET TRIGIA = @SOLUONG * @DONGIA
	WHERE SOCTHD = @SOCTHD
END		

/*--IV.Rang buoc toan ven cho CUAHANG--*/
--1.Doanh thu 1 cua hang bang tong tat ca tong tien xuat tu cua hang do
	--Trigger cho bang CUAHANG--
		---INSERT---
CREATE TRIGGER TRG_CUAHANG_DOANHTHU_INSERT
ON CUAHANG
AFTER INSERT
AS 
BEGIN
	DECLARE @MACH varchar(128)

	SELECT @MACH = inserted.MACH
	FROM inserted

	UPDATE CUAHANG
	SET DOANHTHU = 0
	WHERE MACH = @MACH
END
		
	--Trigger cho bang HOADON--
		---INSERT, DELETE---
CREATE TRIGGER TRG_HOADON_DOANHTHU_INSERT_DELETE
ON HOADON
AFTER INSERT, DELETE
AS 
BEGIN
	DECLARE @MACH varchar(128)
	DECLARE @TONGDOANHTHUTHEM money
	DECLARE @TONGDOANHTHUTRU money

	SET @TONGDOANHTHUTHEM = 0
	SET @TONGDOANHTHUTRU = 0

	SELECT @MACH = inserted.MACH, @TONGDOANHTHUTHEM = inserted.TONGTIEN
	FROM inserted

	SELECT @MACH = deleted.MACH, @TONGDOANHTHUTRU = deleted.TONGTIEN
	FROM deleted

	UPDATE CUAHANG
	SET DOANHTHU = DOANHTHU + @TONGDOANHTHUTHEM - @TONGDOANHTHUTRU
	WHERE MACH = @MACH
END
