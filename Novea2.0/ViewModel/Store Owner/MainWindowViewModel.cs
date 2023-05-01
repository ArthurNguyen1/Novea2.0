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
    public class MainWindowViewModel : BaseViewModel
    {
        public ICommand MinimizeWd { get; set; }
        public ICommand CloseWd { get; set; }
        public MainWindowViewModel()
        {
            MinimizeWd = new RelayCommand<MainWindow>((p) => true, (p) => minimizeWindow(p));
            CloseWd = new RelayCommand<MainWindow>((p) => true, (p) => closeWindow());
        }
        private void minimizeWindow(MainWindow p)
        {
            p.WindowState = WindowState.Minimized;
        }
        private void closeWindow()
        {
            Application.Current.Shutdown();
        }
        
    }
}
