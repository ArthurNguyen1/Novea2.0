using Novea2._0.Model;
using Novea2._0.View.Customer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;

namespace Novea2._0.ViewModel.Customer
{
    public class HomeViewModel : BaseViewModel
    {
        private ObservableCollection<CUAHANG> listStore;
        public ObservableCollection<CUAHANG> ListStore
        {
            get => listStore;
            set { listStore = value; OnPropertyChanged(); }
        }
        private ObservableCollection<CUAHANG> listStore1;
        public ObservableCollection<CUAHANG> ListStore1
        {
            get => listStore1;
            set { listStore1 = value; OnPropertyChanged(); }
        }
        public ICommand LoadStoreCommand { get; set; }
        public ICommand StoreDetailCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public HomeViewModel()
        {
            Const.CH = null;
            ListStore1 = new ObservableCollection<CUAHANG>(DataProvider.Ins.DB.CUAHANGs);
            ListStore = new ObservableCollection<CUAHANG>(ListStore1.GroupBy(p => p.TENCH).Select(grp => grp.FirstOrDefault()));
            LoadStoreCommand = new RelayCommand<Home>((p) => true, (p) => loadStore(p));
            StoreDetailCommand = new RelayCommand<Home>((p) => { return p.ListViewStore.SelectedItem != null; }, (p) => DisplayStoreDetail(p));
            SearchCommand = new RelayCommand<Home>((p) => { return p != null; }, (p) => Search(p));
        }
        void Search(Home parameter)
        {
            ObservableCollection<CUAHANG> temp = new ObservableCollection<CUAHANG>();
            if (parameter.txbSearch.Text == "")
            {
                parameter.ListViewStore.ItemsSource = ListStore;
            }
            else
            {
                foreach (CUAHANG c in ListStore)
                {
                    if (c.TENCH.ToLower().Contains(parameter.txbSearch.Text.ToLower()))
                    {
                        temp.Add(c);
                    }
                }
                if (temp != null)
                {
                    parameter.ListViewStore.ItemsSource = temp;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy cửa hàng");
                }
            }
        }
        void loadStore(Home parameter)
        {
            //DataProvider.Ins.Refresh();
            //ListStore1 = new ObservableCollection<CUAHANG>(DataProvider.Ins.DB.CUAHANGs);           
            //ListStore = new ObservableCollection<CUAHANG>(ListStore1.GroupBy(p => p.TENCH).Select(grp => grp.FirstOrDefault()));           
            Const.CH = null;
        }
        void DisplayStoreDetail(Home parameter)
        {
            CUAHANG temp = (CUAHANG)parameter.ListViewStore.SelectedItem;
            Const.CH = temp;
            Page detailStore = new StoreDetail();
            MainWindow.Instance.MainFrame.NavigationService.Navigate(detailStore);
        }
    }
}
