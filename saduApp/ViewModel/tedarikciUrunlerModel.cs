using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace saduApp.ViewModel
{
    public class tedarikciUrunlerModel
    {
        public string tedarikId { get; set; }
        public string tedarikciIUyed { get; set; }
        public string tedarikUrunId { get; set; }
        public bool Aktiflik { get; set; }
        public uyeModel uyeBilgi { get; set; }
        public urunlerModel urunBilgi { get; set; }
    }
}