using Novea2._0.Model;
using Novea2._0.View.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace Novea2._0.ViewModel.Login
{
    public class ForgotPasswordViewModel : BaseViewModel
    {
        public ICommand CloseWdCommand { get; set; }
        public ICommand SendPassCommand { get; set; }
        public ForgotPasswordViewModel()
        {
            CloseWdCommand = new RelayCommand<ForgotPassword>((p) => true, (p) => CloseWindow(p));
            SendPassCommand = new RelayCommand<ForgotPassword>((p) => true, (p) => SendPass(p));
        }
        void CloseWindow(ForgotPassword p)
        {
            p.Close();
        }
        void SendPass(ForgotPassword parameter)
        {
            if (parameter.tbMail.Text == "")
            {
                MessageBox.Show("Bạn chưa nhập email !", "THÔNG BÁO", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            string match = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            Regex reg = new Regex(match);
            if (!reg.IsMatch(parameter.tbMail.Text))
            {
                MessageBox.Show("Email không hợp lệ !", "THÔNG BÁO", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            int dem1 = DataProvider.Ins.DB.ADMINIS.Where(p => p.EMAIL == parameter.tbMail.Text).Count();
            int dem2 = DataProvider.Ins.DB.KHACHes.Where(p => p.EMAIL == parameter.tbMail.Text).Count();
            int dem3 = DataProvider.Ins.DB.CUAHANGs.Where(p => p.EMAIL == parameter.tbMail.Text).Count();
            int dem4 = DataProvider.Ins.DB.SHIPPERs.Where(p => p.EMAIL == parameter.tbMail.Text).Count();
            if (dem1 + dem2 + dem3 + dem4 == 0)
            {
                MessageBox.Show("Email này chưa được đăng ký !", "THÔNG BÁO", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Random rand = new Random();
            string newpass = rand.Next(100000, 999999).ToString();
            if (dem1 == 1)
            {
                foreach (ADMINI temp in DataProvider.Ins.DB.ADMINIS)
                {
                    if (temp.EMAIL == parameter.tbMail.Text)
                    {
                        temp.MATKHAU = MainLoginViewModel.MD5Hash(MainLoginViewModel.Base64Encode(newpass));
                        break;
                    }
                }
            }
            if (dem2 == 1)
            {
                foreach (KHACH temp in DataProvider.Ins.DB.KHACHes)
                {
                    if (temp.EMAIL == parameter.tbMail.Text)
                    {
                        temp.MATKHAU = MainLoginViewModel.MD5Hash(MainLoginViewModel.Base64Encode(newpass));
                        break;
                    }
                }
            }
            if (dem3 == 1)
            {
                foreach (CUAHANG temp in DataProvider.Ins.DB.CUAHANGs)
                {
                    if (temp.EMAIL == parameter.tbMail.Text)
                    {
                        temp.MATKHAU = MainLoginViewModel.MD5Hash(MainLoginViewModel.Base64Encode(newpass));
                        break;
                    }
                }
            }
            if (dem4 == 1)
            {
                foreach (SHIPPER temp in DataProvider.Ins.DB.SHIPPERs)
                {
                    if (temp.EMAIL == parameter.tbMail.Text)
                    {
                        temp.MATKHAU = MainLoginViewModel.MD5Hash(MainLoginViewModel.Base64Encode(newpass));
                        break;
                    }
                }
            }
            string body = "Mật khẩu mới của bạn là: " + newpass + ". Vui lòng đổi mật khẩu mới ngay khi đăng nhập!";

            try
            {
                MailMessage message = new MailMessage("21522329@gm.uit.edu.vn", parameter.tbMail.Text, "Lấy lại mật khẩu đã quên", body);
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("21522329@gm.uit.edu.vn", "yytitpdpuoxgyklc");
                smtp.Send(message); 
            }
            catch
            {
                MessageBox.Show("Vui lòng cho phép 'quyền truy cập ứng dụng kém an toàn' của gmail");
                return;
            }
            DataProvider.Ins.DB.SaveChanges();
            MessageBox.Show("Đã gửi mật khẩu vào Email đăng ký !", "Thông báo");
            parameter.Close();
        }
    }
}
