//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace saduApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class KategoriUrun
    {
        public string kategoriUrunId { get; set; }
        public string UrunId { get; set; }
        public string KategoriId { get; set; }
        public bool Aktiflik { get; set; }
    
        public virtual Kategoriler Kategoriler { get; set; }
        public virtual Urunler Urunler { get; set; }
    }
}
