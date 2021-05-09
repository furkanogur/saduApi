using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace saduApp.ViewModel
{
    public class kategoriUrunModel
    {
        public string kategoriUrunId { get; set; }
        public string UrunId { get; set; }
        public string KategoriId { get; set; }
        public bool Aktiflik { get; set; }
        public urunlerModel urunBilgi { get; set; }
        public kategoriModel kategoriBilgi { get; set; }

    }
}