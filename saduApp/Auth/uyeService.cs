using saduApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using saduApp.ViewModel;

namespace saduApp.Auth
{
    public class uyeService
    {
        SaduDbEntities db = new SaduDbEntities();
        public uyeModel UyeOturumAc(string kadi, string parola)
        {
            uyeModel uye = db.Uye.Where(s => s.Email == kadi && s.Sifre == parola).Select(x => new uyeModel()

            {
                uyeId = x.uyeId,
                KullaniciAdi = x.KullaniciAdi,
                Email = x.Email,
                admin = x.admin,
                Sifre = x.Sifre,
               


            }).SingleOrDefault();
            return uye;
        }
    }
}