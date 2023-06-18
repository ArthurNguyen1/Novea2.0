using Novea2._0.Model;
using Novea2._0.View.Admin;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace Novea2._0.ViewModel.Admin
{
    public class BanWindowViewModel : BaseViewModel
    {
        public ICommand Closewd { get; set; }
        public ICommand MoveWindow { get; set; }
        public ICommand BanCommand { get; set; }

        public BanWindowViewModel()
        {
            Closewd = new RelayCommand<BanWindow>((p) => true, (p) => Close(p));
            MoveWindow = new RelayCommand<BanWindow>((p) => true, (p) => moveWindow(p));
            BanCommand = new RelayCommand<BanWindow>((p) => true, (p) => _BanAccount(p));
        }

        void _BanAccount(BanWindow p)
        {
            MessageBoxResult h = MessageBox.Show("Bạn chắc chắn muốn khóa tài khoản này ?", "THÔNG BÁO", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (h == MessageBoxResult.Yes)
            {
                if (string.IsNullOrEmpty(p.Mota.Text))
                {
                    MessageBox.Show("Chưa có lý do khóa tài khoản !", "THÔNG BÁO");
                }
                else
                {
                    if (Const.KH != null)
                    {
                        foreach (KHACH a in DataProvider.Ins.DB.KHACHes.Where(pa => (pa.MAND == Const.KH.MAND)))
                        {
                            a.STATU = false;
                            a.REASONBANNING = p.Mota.Text;
                        }
                    }
                    else if (Const.CH != null)
                    {
                        foreach (CUAHANG a in DataProvider.Ins.DB.CUAHANGs.Where(pa => (pa.MACH == Const.CH.MACH)))
                        {
                            a.STATU = false;
                            a.REASONBANNING = p.Mota.Text;
                        }
                    }
                    else if (Const.SHP != null)
                    {
                        foreach (SHIPPER a in DataProvider.Ins.DB.SHIPPERs.Where(pa => (pa.MAND == Const.SHP.MAND)))
                        {
                            a.STATU = false;
                            a.REASONBANNING = p.Mota.Text;
                        }
                    }

                    DataProvider.Ins.DB.SaveChanges();
                    MessageBox.Show("Khóa tài khoản thành công !", "THÔNG BÁO");
                }
            }
            p.Close();
        }

        void moveWindow(BanWindow p)
        {
            p.DragMove();
        }
        void Close(BanWindow p)
        {
            p.Close();
        }
    }
}
