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
        public ICommand Login { get; set; }
        public ICommand RegisterCommand { get; set; }
        public ICommand ForgetPassCommand { get; set; }
        public ICommand _Loadwd { get; set; }
        public ICommand UncheckedCommand { get; set; }
        public StoreLoginViewModel()
        {
            Const.IsLogin = false;
            _Loadwd = new RelayCommand<StoreLogin>((p) => true, (p) => loadwd(p));
            Login = new RelayCommand<StoreLogin>((p) => true, (p) => login(p));
            RegisterCommand = new RelayCommand<StoreLogin>((p) => true, (p) => Register());
            ForgetPassCommand = new RelayCommand<StoreLogin>((p) => true, (p) => ForgetPass());
            UncheckedCommand = new RelayCommand<CheckBox>((p) => true, (p) => Unchecked());
        }
        void Unchecked()
        {
            Properties.Settings.Default.isRememberCheck = false;
            Properties.Settings.Default.Save();
        }
        void loadwd(StoreLogin p)
        {
            Const.IsLogin = false;
            if (Properties.Settings.Default.isRememberCheck == true)
            {
                p.tbUsername.Text = Properties.Settings.Default.username;
                p.password.Password = Properties.Settings.Default.password;
                p.Remember.IsChecked = true;
            }
        }
        public void login(StoreLogin p)
        {
            try
            {
                if (p == null) return;
                string username = p.tbUsername.Text;
                string PassEncode = MainLoginViewModel.MD5Hash(MainLoginViewModel.Base64Encode(p.password.Password));               
                foreach (CUAHANG store in DataProvider.Ins.DB.CUAHANGs)
                {
                    if (username == store.TAIKHOAN && PassEncode == store.MATKHAU)
                    {
                        if (store.STATU == true)
                        {
                            if (p.Remember.IsChecked == true)
                            {
                                Properties.Settings.Default.isRememberCheck = true;
                                Properties.Settings.Default.username = username;
                                Properties.Settings.Default.password = p.password.Password;
                                Properties.Settings.Default.Save();
                            }
                            if (p.Remember.IsChecked == false)
                            {
                                Properties.Settings.Default.isRememberCheck = false;
                                Properties.Settings.Default.Save();
                            }
                            Const.IsLogin = true;
                            Const.CH = store;
                            MainWindow mainWindow = new MainWindow();
                            mainWindow.Show();
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
