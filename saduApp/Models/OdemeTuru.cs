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
    
    public partial class OdemeTuru
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OdemeTuru()
        {
            this.Siparisler = new HashSet<Siparisler>();
        }
    
        public string odemeTuruId { get; set; }
        public string OdemeCesiti { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Siparisler> Siparisler { get; set; }
    }
}
