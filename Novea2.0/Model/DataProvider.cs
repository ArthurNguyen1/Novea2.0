using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Novea2._0.Model
{
    public class DataProvider
    {
        private static DataProvider _ins;
        public static DataProvider Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new DataProvider();
                return _ins;
            }
            set
            {
                _ins = value;
            }
        }

        public QuanLyBanHangNoveaEntities DB { get; set; }

        private DataProvider()
        {
            DB = new QuanLyBanHangNoveaEntities();
        }
        public void Refresh()
        {
            DB = new QuanLyBanHangNoveaEntities();
        }
    }
}
