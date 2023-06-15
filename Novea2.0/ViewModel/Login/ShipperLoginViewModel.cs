using Microsoft.Win32;
//using Novea2._0.View.Admin;
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
using Novea2._0.View.Shipper;

namespace Novea2._0.ViewModel.Login
{
    public class ShipperLoginViewModel : BaseViewModel
    {
        public ICommand Login { get; set; }
        public ICommand UncheckedCommand { get; set; }
        public ICommand RegisterCommand { get; set; }
        public ICommand ForgetPassCommand { get; set; }
        public ICommand _Loadwd { get; set; }

        public ShipperLoginViewModel()
        {
            Const.IsLogin = false;
            _Loadwd = new RelayCommand<ShipperLogin>((p) => true, (p) => loadwd(p));
            Login = new RelayCommand<ShipperLogin>((p) => true, (p) => login(p));
            RegisterCommand = new RelayCommand<ShipperLogin>((p) => true, (p) => Register());
            ForgetPassCommand = new RelayCommand<ShipperLogin>((p) => true, (p) => ForgetPass());
            UncheckedCommand = new RelayCommand<CheckBox>((p) => true, (p) => Unchecked());
        }
        void Unchecked()
        {
            Properties.Settings.Default.Shipper_isChecked = false;
            Properties.Settings.Default.Save();
        }
        void loadwd(ShipperLogin p)
        {
            Const.IsLogin = false;
            if (Properties.Settings.Default.Shipper_isChecked == true)
            {
                p.dangnhap.Text = Properties.Settings.Default.Shipper_username;
                p.password.Password = Properties.Settings.Default.Shipper_password;
                p.Remember.IsChecked = true;
            }
        }
        public void login(ShipperLogin p)
        {
            try
            {
                if (p == null) return;
                string username = p.dangnhap.Text;
                string PassEncode = MainLoginViewModel.MD5Hash(MainLoginViewModel.Base64Encode(p.password.Password));
                foreach (SHIPPER k in DataProvider.Ins.DB.SHIPPERs)
                {
                    if (username == k.TAIKHOAN && PassEncode == k.MATKHAU)
                    {
                        if (k.STATU == true)
                        {
                            if (p.Remember.IsChecked == true)
                            {
                                Properties.Settings.Default.Shipper_isChecked = true;
                                Properties.Settings.Default.Shipper_username = username;
                                Properties.Settings.Default.Shipper_password = p.password.Password;
                                Properties.Settings.Default.Save();
                            }
                            if (p.Remember.IsChecked == false)
                            {
                                Properties.Settings.Default.Shipper_isChecked = false;
                                Properties.Settings.Default.Save();
                            }
                            Const.IsLogin = true;
                            //Const.KH = k;
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
        void ForgetPass()
        {
            ForgotPassword forgotPassword = new ForgotPassword();
            forgotPassword.ShowDialog();
        }
        void Register()
        {
            ShipperSignUp shipperSignUp = new ShipperSignUp();
            shipperSignUp.ShowDialog();
        }
    }
}
