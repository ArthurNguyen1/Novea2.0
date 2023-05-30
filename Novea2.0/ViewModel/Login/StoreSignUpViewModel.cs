using Microsoft.Win32;
using Novea2._0.Model;
using Novea2._0.View.Login;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Novea2._0.ViewModel.Login
{
    public class StoreSignUpViewModel : BaseViewModel
    {
        private byte[] imageData;
        private BitmapImage selectedImage;
        public BitmapImage SelectedImage
        {
            get { return selectedImage; }
            set { selectedImage = value; OnPropertyChanged(); }
        }
        public ICommand MoveWindowCommand { get; set; }
        public ICommand MinimizeWdCommand { get; set; }
        public ICommand CloseWdCommand { get; set; }
        public ICommand AddImageCommand { get; set; }
        public ICommand SignUpCommand { get; set; }
        public StoreSignUpViewModel()
        {
            MoveWindowCommand = new RelayCommand<StoreSignUp>((p) => true, (p) => MoveWindow(p));
            MinimizeWdCommand = new RelayCommand<StoreSignUp>((p) => true, (p) => MinimizeWindow(p));
            CloseWdCommand = new RelayCommand<StoreSignUp>((p) => true, (p) => CloseWindow(p));
            AddImageCommand = new RelayCommand<ImageBrush>((p) => true, (p) => AddImage());
            SignUpCommand = new RelayCommand<StoreSignUp>((p) => true, (p) => SignUp(p));
        }
        void SignUp(StoreSignUp p)
        {
            if (p.TenCH.Text == "" || p.SDT.Text == "" || p.User.Text == "" || p.password.Password == "" || p.Mail.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập đầy đủ thông tin !", "THÔNG BÁO", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            int dem1 = DataProvider.Ins.DB.KHACHes.Where(k => k.TAIKHOAN == p.User.Text).Count();
            int dem2 = DataProvider.Ins.DB.CUAHANGs.Where(c => c.TAIKHOAN == p.User.Text).Count();
            if (dem1 > 0 || dem2 > 0)
            {
                MessageBox.Show("Tên đăng nhập đã tồn tại !", "THÔNG BÁO", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            int dem3 = DataProvider.Ins.DB.KHACHes.Where(k => k.EMAIL == p.Mail.Text).Count();
            int dem4 = DataProvider.Ins.DB.CUAHANGs.Where(c => c.EMAIL == p.Mail.Text).Count();
            if (dem3 > 0 || dem4 > 0)
            {
                MessageBox.Show("Email này đã được sử dụng !", "THÔNG BÁO", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string match = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            Regex reg = new Regex(match);
            if (!reg.IsMatch(p.Mail.Text))
            {
                MessageBox.Show("Email không hợp lệ !", "THÔNG BÁO", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string match1 = @"^((09(\d){8})|(086(\d){7})|(088(\d){7})|(089(\d){7})|(01(\d){9}))$";
            Regex reg1 = new Regex(match1);
            if (!reg1.IsMatch(p.SDT.Text))
            {
                MessageBox.Show("Số điện thoại không hợp lệ !", "THÔNG BÁO", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBoxResult h = MessageBox.Show("Bạn muốn đăng ký tài khoản ?", "THÔNG BÁO", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (h == MessageBoxResult.Yes)
            {
                CUAHANG temp = new CUAHANG();
                temp.MACH = rdMACH();
                temp.TENCH = p.TenCH.Text;
                temp.DIADIEM = p.DCCH.Text;
                temp.EMAIL = p.Mail.Text;
                temp.SDT = p.SDT.Text;
                temp.DOANHTHU = 0;
                temp.NGDK = DateTime.Now;
                temp.TAIKHOAN = p.User.Text;
                temp.MATKHAU = MainLoginViewModel.MD5Hash(MainLoginViewModel.Base64Encode(p.password.Password));
                temp.AVATAR = imageData;
                DataProvider.Ins.DB.CUAHANGs.Add(temp);
                DataProvider.Ins.DB.SaveChanges();
                MessageBox.Show("Chúc mừng bạn đã đăng ký thành công !", "THÔNG BÁO", MessageBoxButton.OK);
                p.User.Clear();
                p.password.Clear();
                p.TenCH.Clear();
                p.SDT.Clear();
                p.DCCH.Clear();
                p.Mail.Clear();
                p.Close();
            }
        }
        bool checkMACH(string m)
        {
            foreach (CUAHANG temp in DataProvider.Ins.DB.CUAHANGs)
            {
                if (temp.MACH == m)
                    return true;
            }
            return false;
        }
        string rdMACH()
        {
            string MaCH;
            do
            {
                Random rand = new Random();
                MaCH = "CH" + rand.Next(0, 10000).ToString();
            } while (checkMACH(MaCH));
            return MaCH;
        }
        void AddImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png";
            if (openFileDialog.ShowDialog() == true)
            {
                SelectedImage = new BitmapImage(new Uri(openFileDialog.FileName));
                MemoryStream memoryStream = new MemoryStream();
                using (FileStream fileStream = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                {
                    fileStream.CopyTo(memoryStream);
                }
                imageData = memoryStream.ToArray();
            }
        }
        void CloseWindow(StoreSignUp p)
        {
            p.Close();
        }
        void MinimizeWindow(StoreSignUp p)
        {
            p.WindowState = WindowState.Minimized;
        }
        void MoveWindow(StoreSignUp p)
        {
            p.DragMove();
        }
    }
}
