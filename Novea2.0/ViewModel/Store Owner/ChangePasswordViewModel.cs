using Novea2._0.Model;
using Novea2._0.View.Store_Owner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Novea2._0.ViewModel.Store_Owner
{
    public class ChangePasswordViewModel : BaseViewModel
    {
        public ICommand ChangePasswordCommand { get; set; }
        public ChangePasswordViewModel() 
        {
            ChangePasswordCommand = new RelayCommand<ChangePassword>((p) => true, (p) => ChangePass(p));
        }
        private void ChangePass(ChangePassword p)
        {
            if (p.pbOLDPASS.Password == "" || p.pbNEWPASS.Password == "" || p.pbNEWPASSAGAIN.Password == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (Const.CH.MATKHAU != MD5Hash(Base64Encode(p.pbOLDPASS.Password)))
            {
                MessageBox.Show("Mật khẩu cũ không đúng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (p.pbNEWPASS.Password == p.pbOLDPASS.Password)
            {
                MessageBox.Show("Mật khẩu mới không được giống mật khẩu cũ!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (p.pbNEWPASS.Password != p.pbNEWPASSAGAIN.Password)
            {
                MessageBox.Show("Mật khẩu nhập lại không đúng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                foreach (CUAHANG store in DataProvider.Ins.DB.CUAHANGs)
                {
                    if (store.MACH == Const.CH.MACH)
                    {
                        store.MATKHAU = MD5Hash(Base64Encode(p.pbNEWPASS.Password));
                        Const.CH.MATKHAU = store.MATKHAU;
                        break;
                    }
                }
                DataProvider.Ins.DB.SaveChanges();
                MessageBox.Show("Đổi mật khẩu thành công!", "Thông báo");
                p.pbOLDPASS.Clear();
                p.pbNEWPASS.Clear();
                p.pbNEWPASSAGAIN.Clear();
            }
        }
        private string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
        private string MD5Hash(string input)
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
    }
}
