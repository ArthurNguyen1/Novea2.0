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
using Novea2._0.View.Shipper;

namespace Novea2._0.ViewModel.Shipper
{
    public class PersonInfoViewModel : BaseViewModel
    {
        private byte[] imageData;
        private BitmapImage avatar;
        public BitmapImage Avatar
        {
            get { return avatar; }
            set { avatar = value; OnPropertyChanged(); }
        }
        public ICommand UpdateAvatarCommand { get; set; }
        public ICommand LoadPersonInfowdCommand { get; set; }
        public ICommand UpdateInfoCommand { get; set; }
        public PersonInfoViewModel()
        {
            imageData = Const.SHP.AVATAR;
            UpdateAvatarCommand = new RelayCommand<ImageBrush>((p) => true, (p) => UpdateAvatar());
            LoadPersonInfowdCommand = new RelayCommand<PersonInfo>((p) => true, (p) => LoadPersonInfowd(p));
            UpdateInfoCommand = new RelayCommand<PersonInfo>((p) => true, (p) => UpdateInfo(p));
        }
        void UpdateInfo(PersonInfo p)
        {
            if (p.tbTENDANGNHAP.Text == "" || p.cbbGIOITINH.Text == "" || p.NS.SelectedDate == null || p.tbSDT.Text == "" || p.tbHOTEN.Text == "" || p.tbMAIL.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập đầy đủ thông tin !", "THÔNG BÁO", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            int dem1 = DataProvider.Ins.DB.ADMINIS.Where(k => k.TAIKHOAN == p.tbTENDANGNHAP.Text).Count();
            int dem2 = DataProvider.Ins.DB.KHACHes.Where(k => k.TAIKHOAN == p.tbTENDANGNHAP.Text).Count();
            int dem3 = DataProvider.Ins.DB.CUAHANGs.Where(k => k.TAIKHOAN == p.tbTENDANGNHAP.Text).Count();
            int dem4 = DataProvider.Ins.DB.SHIPPERs.Where(k => k.TAIKHOAN == p.tbTENDANGNHAP.Text).Count();
            if ((dem1 > 0 || dem2 > 0 || dem3 > 0) || (dem4 > 0 && p.tbTENDANGNHAP.Text != Const.SHP.TAIKHOAN))
            {
                MessageBox.Show("Tên đăng nhập đã tồn tại !", "THÔNG BÁO", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            int dem5 = DataProvider.Ins.DB.ADMINIS.Where(k => k.EMAIL == p.tbMAIL.Text).Count();
            int dem6 = DataProvider.Ins.DB.KHACHes.Where(k => k.EMAIL == p.tbMAIL.Text).Count();
            int dem7 = DataProvider.Ins.DB.CUAHANGs.Where(k => k.EMAIL == p.tbMAIL.Text).Count();
            int dem8 = DataProvider.Ins.DB.SHIPPERs.Where(k => k.EMAIL == p.tbMAIL.Text).Count();
            if ((dem5 > 0 || dem6 > 0 || dem7 > 0) || (dem8 > 0 && p.tbMAIL.Text != Const.SHP.EMAIL))
            {
                MessageBox.Show("Email này đã được sử dụng !", "THÔNG BÁO", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string match = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            Regex reg = new Regex(match);
            if (!reg.IsMatch(p.tbMAIL.Text))
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
            var temp = DataProvider.Ins.DB.SHIPPERs.Where(k => k.MAND == Const.SHP.MAND).FirstOrDefault();
            temp.TAIKHOAN = p.tbTENDANGNHAP.Text;
            temp.GIOITINH = p.cbbGIOITINH.Text;
            temp.HOTEN = p.tbHOTEN.Text;
            temp.NGSINH = p.NS.SelectedDate;
            temp.SDT = p.tbSDT.Text;
            temp.EMAIL = p.tbMAIL.Text;
            temp.AVATAR = imageData;
            DataProvider.Ins.DB.SaveChanges();
            Const.SHP = temp;
            MessageBox.Show("Cập nhật thành công !", "THÔNG BÁO", MessageBoxButton.OK);
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(imageData);
            bitmapImage.EndInit();
            MainWindow.Instance.image.ImageSource = bitmapImage;
            MainWindow.Instance.tbHoten.Text = string.Join(" ", Const.SHP.HOTEN.Split().Reverse().Take(2).Reverse());
        }
        void UpdateAvatar()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png";
            if (openFileDialog.ShowDialog() == true)
            {
                Avatar = new BitmapImage(new Uri(openFileDialog.FileName));
                MemoryStream memoryStream = new MemoryStream();
                using (FileStream fileStream = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                {
                    fileStream.CopyTo(memoryStream);
                }
                imageData = memoryStream.ToArray();
            }
        }
        void LoadPersonInfowd(PersonInfo p)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(Const.SHP.AVATAR);
            bitmapImage.EndInit();
            Avatar = bitmapImage;
            p.tbTENDANGNHAP.Text = Const.SHP.TAIKHOAN;
            p.cbbGIOITINH.Text = Const.SHP.GIOITINH;
            p.tbHOTEN.Text = Const.SHP.HOTEN;
            p.NS.SelectedDate = Const.SHP.NGSINH;
            p.tbSDT.Text = Const.SHP.SDT;
            p.tbMAIL.Text = Const.SHP.EMAIL;
        }
    }
}
