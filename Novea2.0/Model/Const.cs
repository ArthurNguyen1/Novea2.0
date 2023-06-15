using Novea2._0.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Novea2._0.Model
{
    public class Const : BaseViewModel
    {
        public static bool IsLogin { get; set; }

        public static string TenDangNhap { get; set; }
        public static string MACH { get; set; }
        public static CUAHANG CH { get; set; }
        public static KHACH KH { get; set; }
        public static ADMINI ADM { get; set; }
        public static SHIPPER SHP { get; set; }
        public static HOADON HD { get; set; }
        public static SANPHAM SP_temp { get; set; }

        public static string _localLink = System.Reflection.Assembly.GetExecutingAssembly().Location.Remove(System.Reflection.Assembly.GetExecutingAssembly().Location.IndexOf(@"bin\Debug"));
    }
}
