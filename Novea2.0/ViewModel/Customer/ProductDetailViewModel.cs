using Novea2._0.Model;
using Novea2._0.View.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.IO;

namespace Novea2._0.ViewModel.Customer
{
    public class ProductDetailViewModel : BaseViewModel
    {
        private HOADON _HoaDon;
        public HOADON HoaDon { get => _HoaDon; set { _HoaDon = value; OnPropertyChanged(); } }
        private CTHD _Cthd;
        public CTHD Cthd { get => _Cthd; set { _Cthd = value; OnPropertyChanged(); } }
        private string _SL;
        public string SL { get => _SL; set { _SL = value; OnPropertyChanged(); } }
        private int _Trigia;
        public int Trigia { get => _Trigia; set { _Trigia = value; OnPropertyChanged(); } }
        private BitmapImage image;
        public BitmapImage Image { get => image; set { image = value; OnPropertyChanged(); } }
        public ICommand CloseProductDetailwdCommand { get; set; }
        public ICommand Loadwd { get; set; }
        public ICommand UpdateSLCommand { get; set; }
        public ICommand AddToCartCommand { get; set; }
        public ProductDetailViewModel()
        {
            Loadwd = new RelayCommand<ProductDetail>((p) => true, (p) => _Loadwd(p));
            UpdateSLCommand = new RelayCommand<ProductDetail>((p) => true, (p) => _UpdateSLCommand(p));
            AddToCartCommand = new RelayCommand<ProductDetail>((p) => true, (p) => _AddToCartCommand(p));
            CloseProductDetailwdCommand = new RelayCommand<ProductDetail>((p) => true, (p) => CloseProductDetailwd(p));
        }
        void _UpdateSLCommand(ProductDetail parameter)
        {
            SL = parameter.txbSL.Text;

            if (SL == "")
            {
                Trigia = 0;
            }
            else
            {
                if (Int32.TryParse(SL, out int temp) == false)
                {
                    MessageBox.Show("Số lượng chỉ có thể nhập số");
                }
                else if (Int32.Parse(SL) > 1000 || Int32.Parse(SL) <= 0)
                {
                    MessageBox.Show("Số lượng chỉ có thể đặt từ 1 đến 1000");
                }
                else
                {
                    Trigia = (Int32.Parse(SL) * Decimal.ToInt32(Const.SP_temp.DONGIA));
                }
            }
        }
        void _Loadwd(ProductDetail parameter)
        {
            byte[] imageData = Const.SP_temp.HINHSP;
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(imageData);
            bitmapImage.EndInit();
            Image = bitmapImage;
            //HoaDon = DataProvider.Ins.DB.HOADONs.Where(p => p.SOHD == parameter.txbSOHD.Text).FirstOrDefault();
            //Const.HD = HoaDon;

            //CTHD CT = new CTHD();
            //CT.SOHD = parameter.txbSOHD.Text;
            //CT.MASP = Const.SP_temp.MASP;
            //CT.SOLUONG = 0;
            //CT.TRIGIA = 0;

            //Cthd = CT;

            //Disable button add to cart if product from diffrent store or product has exist in cart
            if (CheckProduct())
                parameter.buttonAdd.IsEnabled = false;

            parameter.txbSL.Text = "1";
            _UpdateSLCommand(parameter);
        }
        bool CheckProduct()
        {
            var hoadon = DataProvider.Ins.DB.HOADONs.Where(hd => hd.MAND_KHACH == Const.KH.MAND && hd.STATU == "Khởi tạo").FirstOrDefault();
            if (hoadon != null)
            {
                var dem = DataProvider.Ins.DB.CTHDs.Count(ct => ct.SOHD == hoadon.SOHD && ct.MASP == Const.SP_temp.MASP);
                if (dem > 0 || Const.SP_temp.MACH != hoadon.MACH)
                {
                    return true;
                }
            }
            return false;
        }
        void _AddToCartCommand(ProductDetail parameter)
        {
            //CTHD Cthd_temp = new CTHD();
            //Cthd_temp = Cthd;
            //Cthd_temp.SOLUONG = Int32.Parse(parameter.txbSL.Text);

            //DataProvider.Ins.DB.CTHDs.Add(Cthd_temp);
            //DataProvider.Ins.DB.SaveChanges();

            //Create hd
            var hoadon = DataProvider.Ins.DB.HOADONs.Where(h => h.MAND_KHACH == Const.KH.MAND && h.STATU == "Khởi tạo").FirstOrDefault();
            if (hoadon == null)
            {
                HOADON hd = new HOADON();
                hd.SOHD = rdSOHD();
                hd.NGMH = DateTime.Now;
                hd.TONGTIEN = 0;
                hd.STATU = "Khởi tạo";
                hd.MAND_KHACH = Const.KH.MAND;
                hd.MACH = Const.CH.MACH;
                DataProvider.Ins.DB.HOADONs.Add(hd);
                Const.HD = hd;
            }
            
            //Create cthd
            CTHD cthd = new CTHD();
            cthd.SOCTHD = rdSOCTHD();
            if (Const.HD == null)
            {
                cthd.SOHD = hoadon.SOHD;
            }
            else
            {
                cthd.SOHD = Const.HD.SOHD;
            }
            cthd.MASP = Const.SP_temp.MASP;
            cthd.SOLUONG = Int32.Parse(parameter.txbSL.Text);
            DataProvider.Ins.DB.CTHDs.Add(cthd);
            DataProvider.Ins.DB.SaveChanges();

            parameter.Close();
        }
        void CloseProductDetailwd(ProductDetail p)
        {
            p.Close();
        }
        bool checkSOHD(string m)
        {
            foreach (HOADON temp in DataProvider.Ins.DB.HOADONs)
            {
                if (temp.SOHD == m)
                    return true;
            }
            return false;
        }
        string rdSOHD()
        {
            string SoHD;
            do
            {
                Random rand = new Random();
                SoHD = "HD" + rand.Next(0, 10000).ToString();
            } while (checkSOHD(SoHD));
            return SoHD;
        }
        bool checkSOCTHD(string m)
        {
            foreach (CTHD temp in DataProvider.Ins.DB.CTHDs)
            {
                if (temp.SOCTHD == m)
                    return true;
            }
            return false;
        }
        string rdSOCTHD()
        {
            string SoCTHD;
            do
            {
                Random rand = new Random();
                SoCTHD = "CTHD" + rand.Next(0, 10000).ToString();
            } while (checkSOCTHD(SoCTHD));
            return SoCTHD;
        }
    }
}
