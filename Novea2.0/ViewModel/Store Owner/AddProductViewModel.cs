using Novea2._0.View.Store_Owner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Novea2._0.ViewModel.Store_Owner
{
    public class AddProductViewModel : BaseViewModel
    {
        public ICommand MinimizeWdCommand { get; set; }
        public ICommand CloseWdCommand { get; set; }
        public AddProductViewModel() 
        {
            MinimizeWdCommand = new RelayCommand<AddProduct>((p) => true, (p) => MinimizeWd(p));
            CloseWdCommand = new RelayCommand<AddProduct>((p) => true, (p) => CloseWd(p));
        }
        private void MinimizeWd(AddProduct p)
        {
            p.WindowState = WindowState.Minimized;
        }
        private void CloseWd(AddProduct p)
        {
            p.Close();
        }
    }
}
