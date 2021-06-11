using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace saduApp.ViewModel
{
    public class urunlerModel
    {
        public string urunId { get; set; }
        public string Adi { get; set; }
        public string Aciklama { get; set; }
        public bool Aktiflik { get; set; }
        public decimal Fiyat { get; set; }
        public string UyeId { get; set; }
        public int UrunUyeSayisi { get; set; }
        public string UrunFoto { get; set; }
    }
}