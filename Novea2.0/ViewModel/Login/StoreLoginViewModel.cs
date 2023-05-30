using Microsoft.Win32;
using Novea2._0.View.Store_Owner;
using Novea2._0.View.Login;
using Novea2._0.ViewModel;
using Novea2._0.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Novea2._0.ViewModel.Login
{
    public class StoreLoginViewModel : BaseViewModel
    {
        private string _Username;
        public string Username { get => _Username; set { _Username = value; OnPropertyChanged(); } }
        private string _Password;
        public string Password { get => _Password; set { _Password = value; OnPropertyChanged(); } }
        public ICommand Login { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        public ICommand RegisterCommand { get; set; }
        public ICommand ForgetPassCommand { get; set; }
        public ICommand _Loadwd { get; set; }

        public StoreLoginViewModel()
        {
            Const.IsLogin = false;
            Password = "";
            Username = "";
            _Loadwd = new RelayCommand<StoreLogin>((p) => true, (p) => loadwd());
            Login = new RelayCommand<StoreLogin>((p) => true, (p) => login(p));
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => true, (p) => { Password = p.Password; });
            RegisterCommand = new RelayCommand<StoreLogin>((p) => true, (p) => Register());
            ForgetPassCommand = new RelayCommand<StoreLogin>((p) => true, (p) => ForgetPass());
        }
        void loadwd()
        {
            Const.IsLogin = false;
        }

        public void login(StoreLogin p)
        {
            try
            {
                if (p == null) return;
                string PassEncode = MD5Hash(Base64Encode(Password));
                var accCountCH = DataProvider.Ins.DB.CUAHANGs.Where(x => x.TAIKHOAN == Username && x.MATKHAU == PassEncode && x.STATU == true).Count();
                if (accCountCH > 0)
                {
                    if (p.Remember.IsChecked == true)
                    {
                        Const.IsLogin = true;
                        Const.TenDangNhap = Username;

                        Properties.Settings.Default.Save();

                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                    }
                    else
                    {
                        Const.IsLogin = true;
                        Const.TenDangNhap = Username;
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                        Username = "";
                    }
                }
                else
                {
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!", "Thông báo", MessageBoxButton.OK);
                }
            }
            catch
            {
                MessageBox.Show("Mất kết nối đến cơ sở dữ liệu!", "Thông báo", MessageBoxButton.OK);
            }
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }
        void Register()
        {
            StoreSignUp storeSignUp = new StoreSignUp();
            storeSignUp.ShowDialog();
        }
        void ForgetPass()
        {
            ForgotPassword forgetPassView = new ForgotPassword();
            forgetPassView.ShowDialog();
        }
    }

}
