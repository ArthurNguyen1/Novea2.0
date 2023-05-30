using Novea2._0.View.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Novea2._0.ViewModel.Login
{
    public class ForgotPasswordViewModel : BaseViewModel
    {
        public ICommand CloseWdCommand { get; set; }
        public ForgotPasswordViewModel()
        {
            CloseWdCommand = new RelayCommand<ForgotPassword>((p) => true, (p) => CloseWindow(p));
        }
        void CloseWindow(ForgotPassword p)
        {
            p.Close();
        }
    }
}
