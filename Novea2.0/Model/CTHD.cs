//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Novea2._0.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class CTHD
    {
        public string SOCTHD { get; set; }
        public int SOLUONG { get; set; }
        public decimal TRIGIA { get; set; }
        public string CHUTHICH { get; set; }
        public string SOHD { get; set; }
        public string MASP { get; set; }
    
        public virtual SANPHAM SANPHAM { get; set; }
        public virtual HOADON HOADON { get; set; }
    }
}
