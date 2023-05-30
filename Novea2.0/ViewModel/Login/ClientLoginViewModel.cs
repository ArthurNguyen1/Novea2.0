using Microsoft.Win32;
using Novea2._0.View.Customer;
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
    public class ClientLoginViewModel : BaseViewModel
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

        public ClientLoginViewModel()
        {
            Const.IsLogin = false;
            Password = "";
            Username = "";
            _Loadwd = new RelayCommand<ClientLogin>((p) => true, (p) => loadwd());
            Login = new RelayCommand<ClientLogin>((p) => true, (p) => login(p));
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => true, (p) => { Password = p.Password; });
            RegisterCommand = new RelayCommand<ClientLogin>((p) => true, (p) => Register());
            ForgetPassCommand = new RelayCommand<ClientLogin>((p) => true, (p) => ForgetPass());
        }
        void loadwd()
        {
            Const.IsLogin = false;
        }

        public void login(ClientLogin p)
        {
            try
            {
                if (p == null) return;
                string PassEncode = MainLoginViewModel.MD5Hash(MainLoginViewModel.Base64Encode(Password));
                var accCountKH = DataProvider.Ins.DB.KHACHes.Where(x => x.TAIKHOAN == Username && x.MATKHAU == PassEncode && x.STATU == true).Count();
                if (accCountKH > 0)
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
        void Register()
        {
            ClientSignUp clientSignUp = new ClientSignUp();
            clientSignUp.ShowDialog();
        }
        void ForgetPass()
        {
            ForgotPassword forgetPassView = new ForgotPassword();
            forgetPassView.ShowDialog();
        }
    }

}
