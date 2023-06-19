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
using Novea2._0.View.Store_Owner;

namespace Novea2._0.ViewModel.Customer
{
    public class CartDetailViewModel : BaseViewModel
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
        private double averageRating;
        public double AverageRating { get => averageRating; set { averageRating = value; OnPropertyChanged(); } }
        private int sumRating;
        public int SumRating { get => sumRating; set { sumRating = value; OnPropertyChanged(); } }
        private int totalRating;
        public int TotalRating { get => totalRating; set { totalRating = value; OnPropertyChanged(); } }
        public ICommand CloseProductDetailwdCommand { get; set; }
        public ICommand Loadwd { get; set; }
        public ICommand UpdateSLCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand UserRateCommand { get; set; }
        public CartDetailViewModel()
        {
            Loadwd = new RelayCommand<CartDetail>((p) => true, (p) => _Loadwd(p));
            UpdateSLCommand = new RelayCommand<CartDetail>((p) => true, (p) => _UpdateSLCommand(p));
            UpdateCommand = new RelayCommand<CartDetail>((p) => true, (p) => _UpdateCommand(p));
            DeleteCommand = new RelayCommand<CartDetail>((p) => true, (p) => _DeleteCommand(p));
            CloseProductDetailwdCommand = new RelayCommand<CartDetail>((p) => true, (p) => CloseProductDetailwd(p));
            UserRateCommand = new RelayCommand<CartDetail>((p) => true, (p) => Rate(p));
        }
        void _UpdateSLCommand(CartDetail parameter)
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
                    Trigia = (Int32.Parse(SL) * Decimal.ToInt32(Const.CTHD_temp.SANPHAM.DONGIA));
                }
            }
        }
        void _Loadwd(CartDetail parameter)
        {
            DataProvider.Ins.Refresh();

            byte[] imageData = Const.CTHD_temp.SANPHAM.HINHSP;
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

            parameter.txbSL.Text = "1";
            _UpdateSLCommand(parameter);

            //Load AverageRating, SumRating, Rating
            SumRating = 0;
            TotalRating = 0;
            foreach (DANHGIA dg in DataProvider.Ins.DB.DANHGIAs)
            {
                if (dg.MASP == Const.CTHD_temp.SANPHAM.MASP)
                {
                    SumRating += (int)dg.RATE;
                    TotalRating++;
                }
            }
            if (TotalRating == 0)
            {
                AverageRating = 0;
            }
            else
            {
                AverageRating = Math.Round(sumRating * 1.0 / TotalRating, 1);
            }
            var result = from cthd in DataProvider.Ins.DB.CTHDs
                         join hoadon in DataProvider.Ins.DB.HOADONs on cthd.SOHD equals hoadon.SOHD
                         where hoadon.MAND_KHACH == Const.KH.MAND && hoadon.STATU == "Đã nhận" && cthd.MASP == Const.CTHD_temp.SANPHAM.MASP
                         select cthd;
            if (result.Any())
            {
                parameter.RatingStackPanel.Visibility = Visibility.Visible;
                var danhgia = (from d in DataProvider.Ins.DB.DANHGIAs
                               where d.MAND_KHACH == Const.KH.MAND && d.MASP == Const.CTHD_temp.SANPHAM.MASP
                               select d.RATE).FirstOrDefault();
                if (danhgia != null)
                {
                    parameter.Rating.Value = danhgia.Value;
                    parameter.Rating.IsEnabled = false;
                }
            }
        }

        void _UpdateCommand(CartDetail parameter)
        {
            MessageBoxResult h = MessageBox.Show("Bạn muốn cập nhật đơn hàng ?", "THÔNG BÁO", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (h == MessageBoxResult.Yes)
            {
                foreach (CTHD a in DataProvider.Ins.DB.CTHDs.Where(pa => (pa.SOCTHD == Const.CTHD_temp.SOCTHD)))
                {
                    a.SOLUONG = Int32.Parse(parameter.txbSL.Text);
                }
                DataProvider.Ins.DB.SaveChanges();
                MessageBox.Show("Cập nhật đơn hàng thành công !", "THÔNG BÁO");
            }
            parameter.Close();
        }

        void _DeleteCommand(CartDetail parameter)
        {
            MessageBoxResult h = MessageBox.Show("Bạn muốn xóa sản phẩm khỏi giỏ hàng ?", "THÔNG BÁO", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (h == MessageBoxResult.Yes)
            {
                var itemToRemove = DataProvider.Ins.DB.CTHDs.SingleOrDefault(pa => (pa.SOCTHD == Const.CTHD_temp.SOCTHD));
                if (itemToRemove != null)
                {
                    DataProvider.Ins.DB.CTHDs.Remove(itemToRemove);
                    DataProvider.Ins.DB.SaveChanges();
                    MessageBox.Show("Xóa sản phẩm khỏi giỏ hàng thành công !", "THÔNG BÁO");
                }
            }
            parameter.Close();
        }

        void CloseProductDetailwd(CartDetail p)
        {
            p.Close();
        }
        
        bool checkDANHGIA(string m)
        {
            foreach (DANHGIA d in DataProvider.Ins.DB.DANHGIAs)
            {
                if (d.MADG == m)
                    return true;
            }
            return false;
        }
        string rdDANHGIA()
        {
            string madg;
            do
            {
                Random rand = new Random();
                madg = "DG" + rand.Next(0, 10000).ToString();
            } while (checkDANHGIA(madg));
            return madg;
        }
        void Rate(CartDetail p)
        {
            p.Rating.IsEnabled = false;
            DANHGIA dg = new DANHGIA();
            dg.MADG = rdDANHGIA();
            dg.RATE = (int)p.Rating.Value;
            dg.MAND_KHACH = Const.KH.MAND;
            dg.MASP = Const.CTHD_temp.SANPHAM.MASP;
            DataProvider.Ins.DB.DANHGIAs.Add(dg);
            DataProvider.Ins.DB.SaveChanges();
        }
    }
}
