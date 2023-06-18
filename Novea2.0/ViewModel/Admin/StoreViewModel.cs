using Novea2._0.Model;
using Novea2._0.View.Admin;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Novea2._0.ViewModel.Admin
{
    public class StoreViewModel : BaseViewModel
    {
        private ObservableCollection<CUAHANG> _listStore;
        public ObservableCollection<CUAHANG> listStore { get => _listStore; set { _listStore = value; OnPropertyChanged(); } }
        public ICommand SearchCommand { get; set; }
        public ICommand DetailPdCommand { get; set; }
        public ICommand LoadCsCommand { get; set; }
        public ICommand SortCommand { get; set; }

        public StoreViewModel()
        {
            SearchCommand = new RelayCommand<View.Admin.Store>((p) => { return p == null ? false : true; }, (p) => _SearchCommand(p));
            LoadCsCommand = new RelayCommand<View.Admin.Store>((p) => true, (p) => _LoadCsCommand(p));
            SortCommand = new RelayCommand<View.Admin.Store>((p) => { return p == null ? false : true; }, (p) => _SortCommand(p));
            DetailPdCommand = new RelayCommand<View.Admin.Store>((p) => { return p.ListViewStore.SelectedItem == null ? false : true; }, (p) => _DetailPd(p));
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                return;
        }
        void _LoadCsCommand(View.Admin.Store parameter)
        {
            listStore = new ObservableCollection<CUAHANG>(DataProvider.Ins.DB.CUAHANGs.GroupBy(p => p.MACH).Select(grp => grp.FirstOrDefault()));
            parameter.cbbSort.SelectedIndex = 0;
            _SearchCommand(parameter);
            _SortCommand(parameter);

            Const.KH = null;
            Const.CH = null;
            Const.SHP = null;
        }

        void _SortCommand(View.Admin.Store parameter)
        {
            switch (parameter.cbbSort.SelectedIndex.ToString())
            {
                case "0":
                    {
                        listStore = new ObservableCollection<CUAHANG>(DataProvider.Ins.DB.CUAHANGs.GroupBy(p => p.MACH).Select(grp => grp.FirstOrDefault()));
                        parameter.ListViewStore.ItemsSource = listStore;
                        break;
                    }
                case "1":
                    {
                        listStore = new ObservableCollection<CUAHANG>(DataProvider.Ins.DB.CUAHANGs.GroupBy(p => p.MACH).Select(grp => grp.FirstOrDefault()).OrderBy(m => m.TENCH));
                        parameter.ListViewStore.ItemsSource = listStore;
                        break;
                    }
                case "2":
                    {
                        listStore = new ObservableCollection<CUAHANG>(DataProvider.Ins.DB.CUAHANGs.GroupBy(p => p.MACH).Select(grp => grp.FirstOrDefault()).OrderByDescending(m => m.TENCH));
                        parameter.ListViewStore.ItemsSource = listStore;
                        break;
                    }
            }
        }

        void _SearchCommand(View.Admin.Store paramater)
        {
            ObservableCollection<CUAHANG> temp = new ObservableCollection<CUAHANG>();
            if (paramater.tbSearch.Text == "")
            {
                paramater.ListViewStore.ItemsSource = listStore;
            }
            else
            {
                foreach (CUAHANG s in listStore)
                {
                    if (s.TENCH.ToLower().Contains(paramater.tbSearch.Text.ToLower()))
                    {
                        temp.Add(s);
                    }
                }
                if (temp != null)
                {
                    paramater.ListViewStore.ItemsSource = temp;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy tên");
                }
            }
        }

        void _DetailPd(View.Admin.Store paramater)
        {
            StoreInfo storeInfo = new StoreInfo();
            CUAHANG temp = (CUAHANG)paramater.ListViewStore.SelectedItem;
            Const.CH = temp;
            storeInfo.TenCH.Text = temp.TENCH;
            storeInfo.SDT.Text = temp.SDT;
            storeInfo.Mail.Text = temp.EMAIL;
            storeInfo.DCCH.Text = temp.DIADIEM;
            storeInfo.ShowDialog();
            paramater.ListViewStore.SelectedItem = null;
            listStore = new ObservableCollection<CUAHANG>(DataProvider.Ins.DB.CUAHANGs.GroupBy(p => p.MACH).Select(grp => grp.FirstOrDefault()));
            paramater.ListViewStore.ItemsSource = listStore;
        }
    }
}
