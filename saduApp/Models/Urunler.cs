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
    
    public partial class Urunler
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Urunler()
        {
            this.KategoriUrun = new HashSet<KategoriUrun>();
            this.Siparisler = new HashSet<Siparisler>();
            this.TedarikciUrunler = new HashSet<TedarikciUrunler>();
        }
    
        public string urunId { get; set; }
        public string Adi { get; set; }
        public string Aciklama { get; set; }
        public bool Aktiflik { get; set; }
        public decimal Fiyat { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KategoriUrun> KategoriUrun { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Siparisler> Siparisler { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TedarikciUrunler> TedarikciUrunler { get; set; }
    }
}
