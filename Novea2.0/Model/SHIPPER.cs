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
    
    public partial class SHIPPER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SHIPPER()
        {
            this.HOADONs = new HashSet<HOADON>();
        }
    
        public string MAND { get; set; }
        public string TAIKHOAN { get; set; }
        public string MATKHAU { get; set; }
        public string HOTEN { get; set; }
        public Nullable<System.DateTime> NGSINH { get; set; }
        public string GIOITINH { get; set; }
        public string SDT { get; set; }
        public string EMAIL { get; set; }
        public Nullable<System.DateTime> NGDK { get; set; }
        public Nullable<decimal> LUONG { get; set; }
        public Nullable<bool> SHIPSTATUS { get; set; }
        public byte[] AVATAR { get; set; }
        public string REASONBANNING { get; set; }
        public Nullable<bool> STATU { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HOADON> HOADONs { get; set; }
    }
}
