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

namespace Novea2._0.ViewModel.Customer
{
    public class StoreDetailViewModel : BaseViewModel
    {
        private ObservableCollection<SANPHAM> listProduct;
        public ObservableCollection<SANPHAM> ListProduct { get => listProduct; set { listProduct = value; OnPropertyChanged(); } }
        private ObservableCollection<SANPHAM> listProductTemp;
        public ObservableCollection<SANPHAM> ListProductTemp { get => listProductTemp; set { listProductTemp = value; OnPropertyChanged(); } }
        private string storeImage;
        public string StoreImage { get => storeImage; set { storeImage = value; OnPropertyChanged(); } }
        public ICommand DetailPdCommand { get; set; }
        public ICommand LoadDetailStoreCommand { get; set; }
        public ICommand BackToHomeCommand { get; set; }
        public StoreDetailViewModel()
        {
            ListProductTemp = new ObservableCollection<SANPHAM>(DataProvider.Ins.DB.SANPHAMs.Where(p => p.MACH == Const.CH.MACH && p.AVAILABLE == true));
            ListProduct = new ObservableCollection<SANPHAM>(ListProductTemp.GroupBy(p => p.TENSP).Select(grp => grp.FirstOrDefault()));
            DetailPdCommand = new RelayCommand<StoreDetail>((p) => { return p.ListViewProduct.SelectedItem != null; }, (p) => DisplayDetailProduct(p));
            LoadDetailStoreCommand = new RelayCommand<StoreDetail>((p) => true, (p) => LoadDetailStore(p));
            BackToHomeCommand = new RelayCommand<StoreDetail>((p) => true, (p) => BackToHome());
        }

        void BackToHome()
        {
            MainWindow.Instance.MainFrame.NavigationService.GoBack();
            Const.CH = null;
            Const.SP_temp = null;
        }
        public void LoadDetailStore(StoreDetail parameter)
        {
            ListProductTemp = new ObservableCollection<SANPHAM>(DataProvider.Ins.DB.SANPHAMs.Where(p => p.MACH == Const.CH.MACH && p.AVAILABLE == true));
            ListProduct = new ObservableCollection<SANPHAM>(ListProductTemp.GroupBy(p => p.TENSP).Select(grp => grp.FirstOrDefault()));
            parameter.tbTENCH.Text = Const.CH.TENCH;
            parameter.tbDIADIEM.Text = Const.CH.DIADIEM;
            Random random = new Random();
            StoreImage = "/Resources/Images/storewallpaper" + random.Next(1, 4) + ".jpg";
        }

        bool checkSOHD(string m)
        {
            foreach (HOADON temp in DataProvider.Ins.DB.HOADONs)
            {
                if (temp.SOHD == m)
                    return true;
            }
            return false;
        }
        string rdSOHD()
        {
            string SoHD;
            do
            {
                Random rand = new Random();
                SoHD = "HD" + rand.Next(0, 10000).ToString();
            } while (checkSOHD(SoHD));
            return SoHD;
        }

        public void DisplayDetailProduct(StoreDetail paramater)
        {
            ProductDetail productDetail = new ProductDetail();
            SANPHAM temp = (SANPHAM)paramater.ListViewProduct.SelectedItem;
            productDetail.txbTENSP.Text = temp.TENSP;
            productDetail.txbDONGIA.Text = string.Format("{0:0,0}", temp.DONGIA) + " VNĐ";
            productDetail.txbMOTA.Text = temp.MOTA;
            productDetail.txbSIZE.Text = "Size: " + temp.SIZE;
            Const.SP_temp = temp;

            if (Const.HD == null)
            {
                HOADON hd = new HOADON();
                hd.SOHD = rdSOHD();
                hd.NGMH = DateTime.Now;
                hd.TONGTIEN = 0;
                //Set hd.STATU
                hd.STATU = "";
                hd.MAND_KHACH = Const.KH.MAND;
                hd.MACH = Const.CH.MACH;
                Const.HD = hd;
                productDetail.txbSOHD.Text = Const.HD.SOHD;
                DataProvider.Ins.DB.HOADONs.Add(hd);
                DataProvider.Ins.DB.SaveChanges();

                productDetail.ShowDialog();
                paramater.ListViewProduct.SelectedItem = null;
            }
            else
            {
                if (Const.HD.MACH != Const.CH.MACH)
                {
                    MessageBoxResult h = System.Windows.MessageBox.Show("Bạn có muốn hủy giỏ hàng hiện tại ?", "THÔNG BÁO", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (h == MessageBoxResult.Yes)
                    {
                        var itemToRemove = DataProvider.Ins.DB.HOADONs.SingleOrDefault(pa => (pa.SOHD == Const.HD.SOHD));

                        ObservableCollection<CTHD> ListCTHD = new ObservableCollection<CTHD>(DataProvider.Ins.DB.CTHDs.Where(p => p.SOHD == itemToRemove.SOHD));

                        if (itemToRemove != null)
                        {
                            if (ListCTHD != null)
                            {
                                for (int i = 0; i < ListCTHD.Count; i++)
                                {
                                    DataProvider.Ins.DB.CTHDs.Remove(ListCTHD[i]);
                                }
                            }
                            DataProvider.Ins.DB.HOADONs.Remove(itemToRemove);
                            DataProvider.Ins.DB.SaveChanges();
                        }
                        HOADON hd = new HOADON();
                        hd.SOHD = rdSOHD();
                        hd.NGMH = DateTime.Now;
                        hd.TONGTIEN = 0;
                        //Set hd.STATU
                        hd.STATU = "";
                        hd.MAND_KHACH = Const.KH.MAND;
                        hd.MACH = Const.CH.MACH;
                        Const.HD = hd;
                        productDetail.txbSOHD.Text = Const.HD.SOHD;
                        DataProvider.Ins.DB.HOADONs.Add(hd);
                        DataProvider.Ins.DB.SaveChanges();

                        productDetail.ShowDialog();
                        paramater.ListViewProduct.SelectedItem = null;
                    }
                }
                else
                {
                    var CountSPExist = DataProvider.Ins.DB.CTHDs.Where(p => p.SOHD == Const.HD.SOHD && p.MASP == Const.SP_temp.MASP).Count();
                    if (CountSPExist > 0)
                    {
                        MessageBox.Show("Bạn đã thêm sản phẩm này vào giỏ hàng.\n" +
                                        "Nếu bạn muốn thay đổi chi tiết đơn hàng, mời bạn vào giao diện giỏ hàng.");
                    }
                    else
                    {
                        productDetail.txbSOHD.Text = Const.HD.SOHD;
                        productDetail.ShowDialog();
                        paramater.ListViewProduct.SelectedItem = null;
                    }
                }
            }
        }
    }
}
