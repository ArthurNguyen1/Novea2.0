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
        public ICommand Login { get; set; }
        public ICommand UncheckedCommand { get; set; }
        public ICommand RegisterCommand { get; set; }
        public ICommand ForgetPassCommand { get; set; }
        public ICommand _Loadwd { get; set; }

        public ClientLoginViewModel()
        {
            Const.IsLogin = false;
            _Loadwd = new RelayCommand<ClientLogin>((p) => true, (p) => loadwd(p));
            Login = new RelayCommand<ClientLogin>((p) => true, (p) => login(p));
            RegisterCommand = new RelayCommand<ClientLogin>((p) => true, (p) => Register());
            ForgetPassCommand = new RelayCommand<ClientLogin>((p) => true, (p) => ForgetPass());
            UncheckedCommand = new RelayCommand<CheckBox>((p) => true, (p) => Unchecked());
        }
        void Unchecked()
        {
            Properties.Settings.Default.Client_isChecked = false;
            Properties.Settings.Default.Save();
        }
        void loadwd(ClientLogin p)
        {
            Const.IsLogin = false;
            if (Properties.Settings.Default.Client_isChecked == true)
            {
                p.tbUsername.Text = Properties.Settings.Default.Client_username;
                p.password.Password = Properties.Settings.Default.Client_password;
                p.Remember.IsChecked = true;
            }
        }
        public void login(ClientLogin p)
        {
            try
            {
                if (p == null) return;
                string username = p.tbUsername.Text;
                string PassEncode = MainLoginViewModel.MD5Hash(MainLoginViewModel.Base64Encode(p.password.Password));
                foreach (KHACH k in DataProvider.Ins.DB.KHACHes)
                {
                    if (username == k.TAIKHOAN && PassEncode == k.MATKHAU)
                    {
                        if (k.STATU == true)
                        {
                            if (p.Remember.IsChecked == true)
                            {
                                Properties.Settings.Default.Client_isChecked = true;
                                Properties.Settings.Default.Client_username = username;
                                Properties.Settings.Default.Client_password = p.password.Password;
                                Properties.Settings.Default.Save();
                            }
                            if (p.Remember.IsChecked == false)
                            {
                                Properties.Settings.Default.Client_isChecked = false;
                                Properties.Settings.Default.Save();
                            }
                            Const.IsLogin = true;
                            Const.KH = k;
                            MainWindow mainWindow = new MainWindow();
                            mainWindow.Show();
                            Window mainLogin = Window.GetWindow(p);
                            mainLogin.Close();
                            return;
                        }
                        else
                        {
                            //Process blocked account
                        }
                    }
                }             
                if (Const.IsLogin == false)
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
