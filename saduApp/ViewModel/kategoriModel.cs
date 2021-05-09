using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace saduApp.ViewModel
{
    public class kategoriModel
    {
        public string kategoriId { get; set; }
        public string KatAdi { get; set; }
        public string ustKategoriId { get; set; }
        public bool Aktiflik { get; set; }
        public kategoriModel UstKategoriBilgi { get; set; }
    }
}