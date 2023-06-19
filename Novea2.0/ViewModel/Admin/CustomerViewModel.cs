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
    public class CustomerViewModel : BaseViewModel
    {
        private ObservableCollection<KHACH> _listCustomer;
        public ObservableCollection<KHACH> listCustomer { get => _listCustomer; set { _listCustomer = value; OnPropertyChanged(); } }
        public ICommand SearchCommand { get; set; }
        public ICommand DetailPdCommand { get; set; }
        public ICommand LoadCsCommand { get; set; }
        public ICommand SortCommand { get; set; }

        public CustomerViewModel()
        {
            SearchCommand = new RelayCommand<View.Admin.Customer>((p) => { return p == null ? false : true; }, (p) => _SearchCommand(p));
            LoadCsCommand = new RelayCommand<View.Admin.Customer>((p) => true, (p) => _LoadCsCommand(p));
            SortCommand = new RelayCommand<View.Admin.Customer>((p) => { return p == null ? false : true; }, (p) => _SortCommand(p));
            DetailPdCommand = new RelayCommand<View.Admin.Customer>((p) => { return p.ListViewCustomer.SelectedItem == null ? false : true; }, (p) => _DetailPd(p));
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                return;
        }
        void _LoadCsCommand(View.Admin.Customer parameter)
        {
            listCustomer = new ObservableCollection<KHACH>(DataProvider.Ins.DB.KHACHes.GroupBy(p => p.MAND).Select(grp => grp.FirstOrDefault()));
            parameter.cbbSort.SelectedIndex = 0;
            _SearchCommand(parameter);
            _SortCommand(parameter);

            Const.KH = null;
            Const.CH = null;
            Const.SHP = null;
        }

        void _SortCommand(View.Admin.Customer parameter)
        {
            switch (parameter.cbbSort.SelectedIndex.ToString())
            {
                case "0":
                    {
                        listCustomer = new ObservableCollection<KHACH>(DataProvider.Ins.DB.KHACHes.GroupBy(p => p.MAND).Select(grp => grp.FirstOrDefault()).OrderBy(m => m.HOTEN));
                        parameter.ListViewCustomer.ItemsSource = listCustomer;
                        break;
                    }
                case "1":
                    {
                        listCustomer = new ObservableCollection<KHACH>(DataProvider.Ins.DB.KHACHes.GroupBy(p => p.MAND).Select(grp => grp.FirstOrDefault()).OrderByDescending(m => m.HOTEN));
                        parameter.ListViewCustomer.ItemsSource = listCustomer;
                        break;
                    }
            }
        }

        void _SearchCommand(View.Admin.Customer paramater)
        {
            ObservableCollection<KHACH> temp = new ObservableCollection<KHACH>();
            if (paramater.tbSearch.Text == "")
            {
                paramater.ListViewCustomer.ItemsSource = listCustomer;
            }
            else
            {
                foreach (KHACH s in listCustomer)
                {
                    if (s.HOTEN.ToLower().Contains(paramater.tbSearch.Text.ToLower()))
                    {
                        temp.Add(s);
                    }
                }
                if (temp != null)
                {
                    paramater.ListViewCustomer.ItemsSource = temp;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy tên");
                }
            }
        }

        void _DetailPd(View.Admin.Customer paramater)
        {
            CustomerInfo customerInfo = new CustomerInfo();
            KHACH temp = (KHACH)paramater.ListViewCustomer.SelectedItem;
            Const.KH = temp;
            customerInfo.TenND.Text = temp.HOTEN;
            customerInfo.GT.Text = temp.GIOITINH;
            customerInfo.NS.Text = temp.NGSINH.ToString();
            customerInfo.SDT.Text = temp.SDT;
            customerInfo.Mail.Text = temp.EMAIL;
            customerInfo.DC.Text = temp.DIACHI;
            customerInfo.ShowDialog();
            paramater.ListViewCustomer.SelectedItem = null;
            listCustomer = new ObservableCollection<KHACH>(DataProvider.Ins.DB.KHACHes.GroupBy(p => p.MAND).Select(grp => grp.FirstOrDefault()));
            paramater.ListViewCustomer.ItemsSource = listCustomer;
        }
    }
}
