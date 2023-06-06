using Novea2._0.View.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Novea2._0.ViewModel.Login
{
    public class AdminLoginViewModel : BaseViewModel
    {
        public ICommand RegisterCommand { get; set; }
        public ICommand ForgetPassCommand { get; set; }
        public AdminLoginViewModel()
        {
            RegisterCommand = new RelayCommand<AdminLogin>((p) => true, (p) => Register());
            ForgetPassCommand = new RelayCommand<AdminLogin>((p) => true, (p) => ForgetPass());
        }
        void ForgetPass()
        {
            ForgotPassword forgotPassword = new ForgotPassword();
            forgotPassword.ShowDialog();
        }
        void Register()
        {
            AdminSignUp adminSignUp = new AdminSignUp();
            adminSignUp.ShowDialog();
        }
    }
}
