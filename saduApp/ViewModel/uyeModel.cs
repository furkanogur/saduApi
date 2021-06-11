using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace saduApp.ViewModel
{
    public class uyeModel
    {
        public string uyeId { get; set; }
        public string KullaniciAdi { get; set; }
        public string Sifre { get; set; }
        public bool admin { get; set; }
        public string Email { get; set; }
        public int UyeUrunSayisi { get; set; }
        public string UyeFoto { get; set; }
    }
}