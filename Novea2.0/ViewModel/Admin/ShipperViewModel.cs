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
    public class ShipperViewModel : BaseViewModel
    {
        private ObservableCollection<SHIPPER> _listShipper;
        public ObservableCollection<SHIPPER> listShipper { get => _listShipper; set { _listShipper = value; OnPropertyChanged(); } }
        public ICommand SearchCommand { get; set; }
        public ICommand DetailPdCommand { get; set; }
        public ICommand LoadCsCommand { get; set; }
        public ICommand SortCommand { get; set; }

        public ShipperViewModel()
        {
            SearchCommand = new RelayCommand<View.Admin.Shipper>((p) => { return p == null ? false : true; }, (p) => _SearchCommand(p));
            LoadCsCommand = new RelayCommand<View.Admin.Shipper>((p) => true, (p) => _LoadCsCommand(p));
            SortCommand = new RelayCommand<View.Admin.Shipper>((p) => { return p == null ? false : true; }, (p) => _SortCommand(p));
            DetailPdCommand = new RelayCommand<View.Admin.Shipper>((p) => { return p.ListViewShipper.SelectedItem == null ? false : true; }, (p) => _DetailPd(p));
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                return;
        }
        void _LoadCsCommand(View.Admin.Shipper parameter)
        {
            listShipper = new ObservableCollection<SHIPPER>(DataProvider.Ins.DB.SHIPPERs.GroupBy(p => p.MAND).Select(grp => grp.FirstOrDefault()));
            parameter.cbbSort.SelectedIndex = 0;
            _SearchCommand(parameter);
            _SortCommand(parameter);

            Const.KH = null;
            Const.CH = null;
            Const.SHP = null;
        }

        void _SortCommand(View.Admin.Shipper parameter)
        {
            switch (parameter.cbbSort.SelectedIndex.ToString())
            {
                case "0":
                    {
                        listShipper = new ObservableCollection<SHIPPER>(DataProvider.Ins.DB.SHIPPERs.GroupBy(p => p.MAND).Select(grp => grp.FirstOrDefault()).OrderBy(m => m.HOTEN));
                        parameter.ListViewShipper.ItemsSource = listShipper;
                        break;
                    }
                case "1":
                    {
                        listShipper = new ObservableCollection<SHIPPER>(DataProvider.Ins.DB.SHIPPERs.GroupBy(p => p.MAND).Select(grp => grp.FirstOrDefault()).OrderByDescending(m => m.HOTEN));
                        parameter.ListViewShipper.ItemsSource = listShipper;
                        break;
                    }
            }
        }

        void _SearchCommand(View.Admin.Shipper paramater)
        {
            ObservableCollection<SHIPPER> temp = new ObservableCollection<SHIPPER>();
            if (paramater.tbSearch.Text == "")
            {
                paramater.ListViewShipper.ItemsSource = listShipper;
            }
            else
            {
                foreach (SHIPPER s in listShipper)
                {
                    if (s.HOTEN.ToLower().Contains(paramater.tbSearch.Text.ToLower()))
                    {
                        temp.Add(s);
                    }
                }
                if (temp != null)
                {
                    paramater.ListViewShipper.ItemsSource = temp;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy tên");
                }
            }
        }

        void _DetailPd(View.Admin.Shipper paramater)
        {
            ShipperInfo shipperInfo = new ShipperInfo();
            SHIPPER temp = (SHIPPER)paramater.ListViewShipper.SelectedItem;
            Const.SHP = temp;
            shipperInfo.TenND.Text = temp.HOTEN;
            shipperInfo.GT.Text = temp.GIOITINH;
            shipperInfo.NS.Text = temp.NGSINH.ToString();
            shipperInfo.SDT.Text = temp.SDT;
            shipperInfo.Mail.Text = temp.EMAIL;
            shipperInfo.ShowDialog();
            paramater.ListViewShipper.SelectedItem = null;
            listShipper = new ObservableCollection<SHIPPER>(DataProvider.Ins.DB.SHIPPERs.GroupBy(p => p.MAND).Select(grp => grp.FirstOrDefault()));
            paramater.ListViewShipper.ItemsSource = listShipper;
        }
    }
}
