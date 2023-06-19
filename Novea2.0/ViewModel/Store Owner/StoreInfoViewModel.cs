using Microsoft.Win32;
using Novea2._0.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using Novea2._0.View.Store_Owner;

namespace Novea2._0.ViewModel.Store_Owner
{
    public class StoreInfoViewModel : BaseViewModel
    {
        private byte[] imageData;
        private BitmapImage _Ava;
        public BitmapImage Ava { get => _Ava; set { _Ava = value; OnPropertyChanged(); } }
        private string _Name;
        public string Name { get => _Name; set { _Name = value; OnPropertyChanged(); } }
        private string _DoB;
        public string DoB { get => _DoB; set { _DoB = value; OnPropertyChanged(); } }
        private string _DiaChi;
        public string DiaChi { get => _DiaChi; set { _DiaChi = value; OnPropertyChanged(); } }
        private string _Mail;
        public string Mail { get => _Mail; set { _Mail = value; OnPropertyChanged(); } }

        private string _SDT;
        public string SDT { get => _SDT; set { _SDT = value; OnPropertyChanged(); } }
        private string _TenTK;
        public string TenTK { get => _TenTK; set { _TenTK = value; OnPropertyChanged(); } }
        private CUAHANG _User;
        public CUAHANG User { get => _User; set { _User = value; OnPropertyChanged(); } }
        public ICommand Loadwd { get; set; }
        public ICommand UpdateInfo { get; set; }
        public ICommand AddImage { get; set; }
        public ICommand ChangePass { get; set; }
        public StoreInfoViewModel()
        {
            imageData = Const.CH.AVATAR;
            Loadwd = new RelayCommand<StoreInfo>((p) => true, (p) => _Loadwd(p));
            AddImage = new RelayCommand<ImageBrush>((p) => true, (p) => _AddImage());
            UpdateInfo = new RelayCommand<StoreInfo>((p) => true, (p) => _UdpateInfo(p));
        }
        void _AddImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png";
            if (openFileDialog.ShowDialog() == true)
            {
                Ava = new BitmapImage(new Uri(openFileDialog.FileName));
                MemoryStream memoryStream = new MemoryStream();
                using (FileStream fileStream = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                {
                    fileStream.CopyTo(memoryStream);
                }
                imageData = memoryStream.ToArray();
            }
        }
        void _Loadwd(StoreInfo p)
        {
            if (Const.IsLogin)
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = new MemoryStream(Const.CH.AVATAR);
                bitmapImage.EndInit();
                Ava = bitmapImage;
                Name = Const.CH.TENCH;
                DoB = Const.CH.NGDK.ToString();
                DiaChi = Const.CH.DIADIEM;
                SDT = Const.CH.SDT;
                TenTK = Const.CH.TAIKHOAN;
                Mail = Const.CH.EMAIL;
            }
        }
        void _UdpateInfo(StoreInfo p)
        {
            if (p.tbName.Text == "" || p.tbSDT.Text == "" || p.tbAddress.Text == "" || p.tbMail.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập đầy đủ thông tin !", "THÔNG BÁO", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            int dem5 = DataProvider.Ins.DB.SHIPPERs.Where(k => k.EMAIL == p.tbMail.Text).Count();
            int dem6 = DataProvider.Ins.DB.ADMINIS.Where(k => k.EMAIL == p.tbMail.Text).Count();
            int dem7 = DataProvider.Ins.DB.KHACHes.Where(k => k.EMAIL == p.tbMail.Text).Count();
            int dem8 = DataProvider.Ins.DB.CUAHANGs.Where(k => k.EMAIL == p.tbMail.Text).Count();
            if ((dem5 > 0 || dem6 > 0 || dem7 > 0) || (dem8 > 0 && p.tbMail.Text != Const.CH.EMAIL))
            {
                MessageBox.Show("Email này đã được sử dụng !", "THÔNG BÁO", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string match = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            Regex reg = new Regex(match);
            if (!reg.IsMatch(p.tbMail.Text))
            {
                MessageBox.Show("Email không hợp lệ !", "THÔNG BÁO", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string match1 = @"^((09(\d){8})|(086(\d){7})|(088(\d){7})|(089(\d){7})|(01(\d){9}))$";
            Regex reg1 = new Regex(match1);
            if (!reg1.IsMatch(p.tbSDT.Text))
            {
                MessageBox.Show("Số điện thoại không hợp lệ !", "THÔNG BÁO", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var temp = DataProvider.Ins.DB.CUAHANGs.Where(pa => pa.TAIKHOAN == TenTK).FirstOrDefault();
            temp.TENCH = p.tbName.Text;
            temp.SDT = p.tbSDT.Text;
            temp.DIADIEM = p.tbAddress.Text;
            temp.EMAIL = p.tbMail.Text;
            temp.AVATAR = imageData;
            DataProvider.Ins.DB.SaveChanges();
            Const.CH = temp;
            MessageBox.Show("Cập nhật thành công!", "Thông báo");
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(imageData);
            bitmapImage.EndInit();
            MainWindow.Instance.image.ImageSource = bitmapImage;
            MainWindow.Instance.TenDangNhap.Text = string.Join(" ", temp.TENCH.Split().Reverse().Take(2).Reverse());
        }
    }
}
