using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using saduApp.Models;
using saduApp.ViewModel;

namespace saduApp.Controllers
{
    public class ServisController : ApiController
    {
        SaduDbEntities db = new SaduDbEntities();
        sonucModel sonuc = new sonucModel();


        #region Uye

        //UyeListele

        [HttpGet]
        [Route("api/uyeliste")]
        public List<uyeModel> UyeListe()
        {
            List<uyeModel> liste = db.Uye.Select(x => new uyeModel()
            {
                uyeId = x.uyeId,
                KullaniciAdi = x.KullaniciAdi,
                Sifre = x.Sifre,
                Email = x.Email,
                admin = x.admin,
                UyeFoto = x.UyeFoto,
       

            }).ToList();
            return liste;
        }

        //UyeById

        [HttpGet]
        [Route("api/uyebyid/{uyeId}")]

        public uyeModel UyeById(string uyeId)
        {
            uyeModel kayit = db.Uye.Where(s => s.uyeId == uyeId).Select(x => new uyeModel()
            {

                uyeId = x.uyeId,
                KullaniciAdi = x.KullaniciAdi,
                Sifre = x.Sifre,
                Email = x.Email,
                admin = x.admin,
                UyeFoto = x.UyeFoto,

            }).SingleOrDefault();

            return kayit;
        }

        //Uye EKle

        [HttpPost]
        [Route("api/uyeekle")]
        public sonucModel UyeEkle(uyeModel model)
        {

            if (db.Uye.Count(s =>s.Email == model.Email) >= 1)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Bu Üye Zaten Kayıtlıdır!";
                return sonuc;
            }

            Uye yeni = new Uye();
            yeni.uyeId = Guid.NewGuid().ToString();
            yeni.KullaniciAdi = model.KullaniciAdi;
            yeni.Sifre = model.Sifre;
            yeni.Email = model.Email;
            yeni.admin = model.admin;
            yeni.UyeFoto = model.UyeFoto;
            db.Uye.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Üye Eklendi";

            return sonuc;
        }

        //Uye Duzenle

        [HttpPut]
        [Route("api/uyeduzenle")]

        public sonucModel UyeDuzenle(uyeModel model)
        {
            Uye kayit = db.Uye.Where(s => s.uyeId == model.uyeId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Üye Bulunamadı";
                return sonuc;
            }

            kayit.KullaniciAdi = model.KullaniciAdi;
            kayit.Sifre = model.Sifre;
            kayit.admin = model.admin;
            kayit.UyeFoto = model.UyeFoto;
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Üye Düzenlendi";

            return sonuc;
        }

        //Uye Sil
        [HttpDelete]
        [Route("api/uyesil/{uyeId}")]
        public sonucModel UyeSil(string uyeId)
        {

            Uye kayit = db.Uye.Where(s => s.uyeId == uyeId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Üye Bulunamadı";
                return sonuc;
            }

            if (db.Iletisim.Count(s => s.UyeId == uyeId) >= 1)
            {
                sonuc.islem = false;
                sonuc.mesaj = "İletişim Bilgisi Olan Üye Silinemez!";
                return sonuc;
            }

            if (db.Urunler.Count(s => s.UyeId == uyeId) >= 1)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Ürünü Olan Üye Silinemez!";
                return sonuc;
            }

            db.Uye.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Üye Silindi";

            return sonuc;
        }

        [HttpPost]
        [Route("api/uyefotoguncelle")]
        public sonucModel UyeFotoGuncelle(UyeFotoModel model)
        {
            Uye uye = db.Uye.Where(s => s.uyeId == model.uyeId).SingleOrDefault();


            if (uye ==null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }

            if (uye.UyeFoto != "profil.jpg")
            {
                string yol = System.Web.Hosting.HostingEnvironment.MapPath("~/Dosyalar/" + uye.UyeFoto);
                if (File.Exists(yol))
                {
                    File.Delete(yol);
                }
            }

            string data = model.fotoData;
            string base64 = data.Substring(data.IndexOf(',') + 1);
            base64 = base64.Trim('\0');
            byte[] imgbytes = Convert.FromBase64String(base64);
            string dosyaAdi = uye.uyeId + model.fotoUzanti.Replace("image/",".");
            using (var ms = new MemoryStream(imgbytes,0,imgbytes.Length))
            {
                Image img = Image.FromStream(ms,true);
                img.Save(System.Web.Hosting.HostingEnvironment.MapPath("~/Dosyalar/" + dosyaAdi));

            }
            uye.UyeFoto = dosyaAdi;
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Fotoğraf Güncellendi";
          

                return sonuc;
        }

        #endregion

        #region İletisim

        //İletisimListele

        [HttpGet]
        [Route("api/iletisimliste")]
        public List<iletisimModel> IletisimListe()
        {
            List<iletisimModel> liste = db.Iletisim.Select(x => new iletisimModel()
            {
                iletisimId=x.iletisimId,
                Ad = x.Ad,
                Soyad = x.Soyad,
                Adres = x.Adres,
                Telefon = x.Telefon,
                UyeId = x.UyeId
            }).ToList();
            return liste;
        }

        //İletisimById

        [HttpGet]
        [Route("api/iletisimbyid/{uyeId}")]

        public iletisimModel IletisimById(string uyeId)
        {
            iletisimModel kayit = db.Iletisim.Where(s => s.UyeId == uyeId).Select(x => new iletisimModel()
            {
                
               
                Ad= x.Ad,
                Soyad = x.Soyad,
                Adres = x.Adres,
                Telefon = x.Telefon,
                UyeId = x.UyeId,
                iletisimId = x.iletisimId

            }).SingleOrDefault();

            return kayit;
        }

        //İletisim Ekle
        [HttpPost]
        [Route("api/iletisimekle")]
        public sonucModel IletisimEkle(iletisimModel model)
        {

            if (db.Iletisim.Count(s =>s.UyeId == model.UyeId) >= 1)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Bu İletişim Bilgisi Zaten Kayıtlıdır!";
                return sonuc;
            }

            Iletisim yeni = new Iletisim();
            yeni.iletisimId = Guid.NewGuid().ToString();
            yeni.Ad = model.Ad;
            yeni.Soyad = model.Soyad;
            yeni.Telefon = model.Telefon;
            yeni.Adres = model.Adres;
            yeni.UyeId = model.UyeId;
            db.Iletisim.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Üye Eklendi";

            return sonuc;
        }
        //İletisim Duzenle

        [HttpPut]
        [Route("api/iletisimduzenle")]

        public sonucModel IletisimDuzenle(iletisimModel model)
        {
            Iletisim kayit = db.Iletisim.Where(s => s.iletisimId == model.iletisimId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "İletisim Bulunamadı";
                return sonuc;
            }

            kayit.Ad = model.Ad;
            kayit.Soyad = model.Soyad;
            kayit.Adres = model.Adres;
            kayit.Telefon = model.Telefon;      

            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "İletişim Bilgileri Düzenlendi";

            return sonuc;
        }

        //İletisim Sil

        [HttpDelete]
        [Route("api/iletisimsil/{iletisimId}")]
        public sonucModel IletisimSil(string iletisimId)
        {

            Iletisim kayit = db.Iletisim.Where(s => s.iletisimId == iletisimId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Üye Bulunamadı";
                return sonuc;
            }

            db.Iletisim.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Iletisim Bilgileri Silindi";

            return sonuc;
        }

        #endregion

        #region TedarikciUrun
        //TedarikciuyeListele

        [HttpGet]
        [Route("api/tedarikuyeliste/{uyeTedId}")]
        public List<urunlerModel> TedarikUrunListe(string uyeTedId)
        {
            List<urunlerModel> liste = db.Urunler.Where(s => s.UyeId == uyeTedId).Select(x => new urunlerModel()
            {
                urunId = x.urunId,
                Adi = x.Adi,
                Aciklama = x.Aciklama,
                Aktiflik = x.Aktiflik,
                UyeId = x.UyeId,
                Fiyat = x.Fiyat,
                UrunFoto =x.UrunFoto
                

            }).ToList();

            return liste;
        }

        ////TedarikciUrunListele

        //[HttpGet]
        //[Route("api/tedarikurunliste/{urunTedId}")]
        //public List<tedarikciUrunlerModel> UrunTedarikListe(string urunTedId)
        //{
        //    List<tedarikciUrunlerModel> liste = db.TedarikciUrunler.Where(s => s.tedarikUrunId == urunTedId).Select(x => new tedarikciUrunlerModel()
        //    {
        //        tedarikId = x.tedarikId,
        //        tedarikciIUyed = x.tedarikciIUyed,
        //        tedarikUrunId = x.tedarikUrunId,
        //        Aktiflik = x.Aktiflik
        //    }).ToList();

        //    foreach (var kayit in liste)
        //    {
        //        kayit.uyeBilgi = UyeById(kayit.tedarikciIUyed);
        //        kayit.urunBilgi = UrunById(kayit.tedarikUrunId);

        //    }

        //    return liste;
        //}

        ////Tedarikci EKle
        //[HttpPost]
        //[Route("api/tedarikciekle")]
        //public sonucModel TedarikciEkle(tedarikciUrunlerModel model)
        //{

        //    if (db.TedarikciUrunler.Count(s => s.tedarikciIUyed == model.tedarikciIUyed && s.tedarikUrunId == model.tedarikUrunId) > 0)
        //    {
        //        sonuc.islem = false;
        //        sonuc.mesaj = "Bu Tedarikci Ürünü Zaten Kayıtlıdır!";
        //        return sonuc;
        //    }

        //    TedarikciUrunler yeni = new TedarikciUrunler();
        //    yeni.tedarikId = Guid.NewGuid().ToString();
        //    yeni.tedarikciIUyed = model.tedarikciIUyed;
        //    yeni.tedarikUrunId = model.tedarikUrunId;
        //    yeni.Aktiflik = true;

        //    db.TedarikciUrunler.Add(yeni);
        //    db.SaveChanges();
        //    sonuc.islem = true;
        //    sonuc.mesaj = "Tedarikci Ürünü Eklendi";

        //    return sonuc;
        //}
        ////Tedarikci Duzenle

        //[HttpPut]
        //[Route("api/tedarikurunduzenle")]

        //public sonucModel TedarikUrunDuzenle(tedarikciUrunlerModel model)
        //{
        //    TedarikciUrunler kayit = db.TedarikciUrunler.Where(s => s.tedarikId == model.tedarikId).SingleOrDefault();

        //    if (kayit == null)
        //    {
        //        sonuc.islem = false;
        //        sonuc.mesaj = "Tedarikci Ürün Bulunamadı";
        //        return sonuc;
        //    }

        //    kayit.Aktiflik = model.Aktiflik;

        //    db.SaveChanges();
        //    sonuc.islem = true;
        //    sonuc.mesaj = "Tedarik Ürun Bilgileri Düzenlendi";

        //    return sonuc;
        //}
        ////Tedarikci Sil

        //[HttpDelete]
        //[Route("api/tedarikurunsil/{tuId}")]
        //public sonucModel TedarikUrunSil(string tuId)
        //{

        //    TedarikciUrunler kayit = db.TedarikciUrunler.Where(s => s.tedarikId == tuId).SingleOrDefault();

        //    if (kayit == null)
        //    {
        //        sonuc.islem = false;
        //        sonuc.mesaj = "Kayıt Bulunamadı";
        //        return sonuc;
        //    }

        //    db.TedarikciUrunler.Remove(kayit);
        //    db.SaveChanges();
        //    sonuc.mesaj = "Tedarik Ürün Silindi";
        //    sonuc.islem = true;
        //    return sonuc;
        //}

        #endregion

        #region Urunler

        //Urun Listele

        [HttpGet]
        [Route("api/urunliste")]
        public List<urunlerModel> UrunListe()
        {
            List<urunlerModel> liste = db.Urunler.Select(x => new urunlerModel()
            {
                Adi = x.Adi,
                Aciklama = x.Aciklama,
                Aktiflik = x.Aktiflik,
                Fiyat = x.Fiyat,
                urunId = x.urunId,
                UyeId=x.UyeId,
                UrunFoto=x.UrunFoto,
                
            }).ToList();
            return liste;
        }

        //UrunById

        [HttpGet]
        [Route("api/urunbyid/{urunId}")]
        public urunlerModel UrunById(string urunId)
        {
            urunlerModel kayit = db.Urunler.Where(s => s.urunId == urunId).Select(x => new urunlerModel()
            {
                urunId = x.urunId,
                Adi = x.Adi,
                Aciklama = x.Aciklama,
                Fiyat = x.Fiyat,
                Aktiflik = x.Aktiflik,
                UyeId = x.UyeId,
                UrunFoto = x.UrunFoto,
            }).SingleOrDefault();

            return kayit;
        }


        //Urun Ekle

        [HttpPost]
        [Route("api/urunekle")]
        public sonucModel UrunEkle(urunlerModel model)
        {
  
            Urunler yeni = new Urunler();
            yeni.urunId = Guid.NewGuid().ToString();
            yeni.Adi = model.Adi;
            yeni.Aciklama = model.Aciklama;
            yeni.Fiyat = model.Fiyat;
            yeni.Aktiflik = true;
            yeni.UyeId = model.UyeId;
            yeni.UrunFoto = model.UrunFoto;


            db.Urunler.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Ürun Eklendi";
            return sonuc;
        }

        //Urun Duzenle

        [HttpPut]
        [Route("api/urunduzenle")]
        public sonucModel UrunDuzenle(urunlerModel model)
        {
            Urunler kayit = db.Urunler.Where(s => s.urunId == model.urunId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Ürün Bulunamadı!";
                return sonuc;
            }

            kayit.Adi = model.Adi;
            kayit.Aciklama = model.Aciklama;
            kayit.Fiyat = model.Fiyat;
            kayit.Aktiflik = model.Aktiflik;
            kayit.UrunFoto = model.UrunFoto; 

            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Ürün Düzenlendi";

            return sonuc;
        }

        //Urun Sil

        [HttpDelete]
        [Route("api/urunsil/{urunId}")]
        public sonucModel UrunSil(string urunId)
        {
            Urunler kayit = db.Urunler.Where(s => s.urunId == urunId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Ürün Bulunamadı";
                return sonuc;
            }
            if (db.TedarikciUrunler.Count(s => s.tedarikUrunId == urunId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Ürünü Tedarikten Ve Kategorideki Ürünlerden Sil";
                return sonuc;

            }

            db.Urunler.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Ürün Silindi";
            return sonuc;
        }

        //urunFoto
        [HttpPost]
        [Route("api/urunfotoguncelle")]
        public sonucModel UrunFotoGuncelle(UrunFotoModel model)
        {
            Urunler urun = db.Urunler.Where(s => s.urunId == model.urunId).SingleOrDefault();


            if (urun == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }

            if (urun.UrunFoto != "urun.jpg")
            {
                string yol = System.Web.Hosting.HostingEnvironment.MapPath("~/Dosyalar/" + urun.UrunFoto);
                if (File.Exists(yol))
                {
                    File.Delete(yol);
                }
            }

            string data = model.fotoData;
            string base64 = data.Substring(data.IndexOf(',') + 1);
            base64 = base64.Trim('\0');
            byte[] imgbytes = Convert.FromBase64String(base64);
            string dosyaAdi = urun.urunId + model.fotoUzanti.Replace("image/", ".");
            using (var ms = new MemoryStream(imgbytes, 0, imgbytes.Length))
            {
                Image img = Image.FromStream(ms, true);
                img.Save(System.Web.Hosting.HostingEnvironment.MapPath("~/Dosyalar/" + dosyaAdi));

            }
            urun.UrunFoto = dosyaAdi;
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Fotoğraf Güncellendi";


            return sonuc;
        }

        #endregion

        #region Kategoriler 
        //Kategori Listele

        [HttpGet]
        [Route("api/kategoriliste")]
        public List<kategoriModel> KategoriListe()
        {
            List<kategoriModel> liste = db.Kategoriler.Select(x => new kategoriModel()
            {
                kategoriId = x.kategoriId,
                KatAdi = x.KatAdi,
                Aktiflik = x.Aktiflik,
                ustKategoriId = x.ustKategoriId,
               
            }).ToList();

            foreach (var kayit in liste)
            {
                kayit.UstKategoriBilgi = KategoriById(kayit.ustKategoriId);
            }

            return liste;
        }

        //KategoriById

        [HttpGet]
        [Route("api/kategoribyid/{katId}")]

        public kategoriModel KategoriById(string katId)
        {
            kategoriModel kayit = db.Kategoriler.Where(s => s.kategoriId == katId).Select(x => new kategoriModel()
            {
                kategoriId = x.kategoriId,
                KatAdi = x.KatAdi,
                Aktiflik = x.Aktiflik,
                ustKategoriId = x.ustKategoriId,
                
        }).SingleOrDefault();      
            return kayit;
        }

        //Kategori Ekle

        [HttpPost]
        [Route("api/kategoriekle")]
        public sonucModel KategoriEkle(kategoriModel model)
        {

            if (db.Kategoriler.Count(s => s.KatAdi == model.KatAdi && s.ustKategoriId == model.ustKategoriId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Bu Kategori Bilgisi Zaten Kayıtlıdır!";
                return sonuc;
            }

            Kategoriler yeni = new Kategoriler();
            yeni.kategoriId = Guid.NewGuid().ToString();
            yeni.KatAdi = model.KatAdi;
            yeni.ustKategoriId = model.ustKategoriId;
            yeni.Aktiflik = true;
            
            db.Kategoriler.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kategori Eklendi";

            return sonuc;
        }

        //Kategori Duzenle

        [HttpPut]
        [Route("api/kategoriduzenle")]

        public sonucModel KategoriDuzenle(kategoriModel model)
        {
            Kategoriler kayit = db.Kategoriler.Where(s => s.kategoriId == model.kategoriId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kategori Bulunamadı";
                return sonuc;
            }

            kayit.KatAdi = model.KatAdi;
            kayit.ustKategoriId = model.ustKategoriId;
            kayit.Aktiflik = model.Aktiflik;

            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kategori Bilgileri Düzenlendi";

            return sonuc;
        }

        //Kategori Sil

        [HttpDelete]
        [Route("api/kategorisil/{katId}")]
        public sonucModel KategoriSil(string katId)
        {

            Kategoriler kayit = db.Kategoriler.Where(s => s.kategoriId == katId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kategori Bulunamadı";
                return sonuc;
            }

            db.Kategoriler.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kategori Silindi";

            return sonuc;
        }

        #endregion

        #region KategoriUrun

        //UrunKategoriListele

        [HttpGet]
        [Route("api/urunkategoriliste/{urunKatId}")]
        public List<kategoriUrunModel> UrunKategoriListe(string urunKatId)
        {
            List<kategoriUrunModel> liste = db.KategoriUrun.Where(s => s.UrunId == urunKatId).Select(x => new kategoriUrunModel()
            {
                 kategoriUrunId= x.kategoriUrunId,
                UrunId = x.UrunId,
                KategoriId = x.KategoriId,
                Aktiflik = x.Aktiflik

            }).ToList();

            foreach (var kayit in liste)
            {
                kayit.kategoriBilgi = KategoriById(kayit.KategoriId);
                kayit.urunBilgi = UrunById(kayit.UrunId);

            }

            return liste;
        }

        //KategoriUrunListele

        [HttpGet]
        [Route("api/kategoriurunliste/{katUrunId}")]
        public List<kategoriUrunModel> KategoriUrunListe(string katUrunId)
        {
            List<kategoriUrunModel> liste = db.KategoriUrun.Where(s => s.KategoriId == katUrunId).Select(x => new kategoriUrunModel()
            {
                kategoriUrunId = x.kategoriUrunId,
                UrunId = x.UrunId,
                KategoriId = x.KategoriId,
                Aktiflik = x.Aktiflik

            }).ToList();

            foreach (var kayit in liste)
            {
                kayit.kategoriBilgi = KategoriById(kayit.KategoriId);
                kayit.urunBilgi = UrunById(kayit.UrunId);

            }

            return liste;
        }

        //KategoriUrun Ekle

        [HttpPost]
        [Route("api/kategoriurunekle")]
        public sonucModel KategoriUrunEkle(kategoriUrunModel model)
        {

            if (db.KategoriUrun.Count(s => s.KategoriId == model.KategoriId && s.UrunId == model.UrunId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Bu Ürün Bu Kategoriye Zaten Kayıtlıdır!";
                return sonuc;
            }

            KategoriUrun yeni = new KategoriUrun();
            yeni.kategoriUrunId = Guid.NewGuid().ToString();
            yeni.UrunId = model.UrunId;
            yeni.KategoriId = model.KategoriId;
            yeni.Aktiflik = true;

            db.KategoriUrun.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Ürün kategoriye Eklendi";

            return sonuc;
        }

        //KategoriUrun Duzenle

        [HttpPut]
        [Route("api/kategoriurunduzenle")]

        public sonucModel KategoriUrunDuzenle(kategoriUrunModel model)
        {
            KategoriUrun kayit = db.KategoriUrun.Where(s => s.kategoriUrunId == model.kategoriUrunId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kategoride Ürün Bulunamadı";
                return sonuc;
            }

            kayit.Aktiflik = model.Aktiflik;

            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kategori Ürun Bilgileri Düzenlendi";

            return sonuc;
        }

        //KategoriUrurn Sil

        [HttpDelete]
        [Route("api/kategoriurunsil/{katUrunId}")]
        public sonucModel KategoriUrunSil(string katUrunId)
        {

            KategoriUrun kayit = db.KategoriUrun.Where(s => s.kategoriUrunId == katUrunId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı";
                return sonuc;
            }

            db.KategoriUrun.Remove(kayit);
            db.SaveChanges();
            sonuc.mesaj = "Kategori Ürünü Silindi";
            sonuc.islem = true;
            return sonuc;
        }


        #endregion

        #region Siparisler
        //Siparisler Listele

        [HttpGet]
        [Route("api/siparisliste")]
        public List<siparislerModel> SiparisListe()
        {
            List<siparislerModel> liste = db.Siparisler.Select(x => new siparislerModel()
            {
                siparisId = x.siparisId,
                UyeId = x.UyeId,
                UrunId = x.UrunId,
                Fiyat = x.Fiyat,
                Adres = x.Adres,
                SiparisTarihi = x.SiparisTarihi,
                SiparisDurumuId = x.SiparisDurumuId,
                KargoId = x.KargoId,
                KargoUcreti = x.KargoUcreti,
                OdemeId = x.OdemeId,
                
            }).ToList();

            return liste;
        }

        //SiparisById

        [HttpGet]
        [Route("api/siparisbyid/{siparisId}")]

        public siparislerModel SiparisById(string siparisId)
        {
            siparislerModel kayit = db.Siparisler.Where(s => s.siparisId == siparisId).Select(x => new siparislerModel()
            {
                siparisId = x.siparisId,
                UyeId = x.UyeId,
                UrunId = x.UrunId,
                Fiyat = x.Fiyat,
                Adres = x.Adres,
                SiparisTarihi = x.SiparisTarihi,
                SiparisDurumuId = x.SiparisDurumuId,
                KargoId = x.KargoId,
                KargoUcreti = x.KargoUcreti,
                OdemeId = x.OdemeId,

            }).SingleOrDefault();

            return kayit;
        }

        //Siparisler Ekle

        [HttpPost]
        [Route("api/siparisekle")]
        public sonucModel SiparisEkle(siparislerModel model)
        {

            Siparisler yeni = new Siparisler();
            yeni.siparisId = Guid.NewGuid().ToString();
            yeni.UyeId = model.UyeId;
            yeni.Adres = model.Adres;
            yeni.Fiyat = model.Fiyat;
            yeni.UrunId = model.UrunId;
            yeni.SiparisTarihi = model.SiparisTarihi;
            yeni.SiparisDurumuId = model.SiparisDurumuId;
            yeni.KargoId = model.KargoId;
            yeni.KargoUcreti = model.KargoUcreti;
            yeni.OdemeId = model.OdemeId;
            db.Siparisler.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Siparis Eklendi";

            return sonuc;
        }

        //Siparisler Duzenle

        [HttpPut]
        [Route("api/siparisduzenle")]

        public sonucModel SiparisDuzenle(siparislerModel model)
        {
            Siparisler kayit = db.Siparisler.Where(s => s.siparisId == model.SiparisDurumuId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Siparis Bulunamadı";
                return sonuc;
            }

            kayit.UrunId = model.UrunId;
            kayit.Fiyat = model.Fiyat;
            kayit.Adres = model.Adres;
            kayit.KargoId = model.KargoId;
            kayit.KargoUcreti = model.KargoUcreti;
            kayit.SiparisDurumuId = model.SiparisDurumuId;

            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Siparis Düzenle";

            return sonuc;
        }

        //Siparisler Sil

        [HttpDelete]
        [Route("api/siparissil/{siparisId}")]
        public sonucModel SiparisSil(string siparisId)
        {

            Siparisler kayit = db.Siparisler.Where(s => s.siparisId == siparisId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "siparis Bulunamadı";
                return sonuc;
            }

        
            db.Siparisler.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Siparis Silindi";

            return sonuc;
        }

        #endregion

        #region SiparisDurum

        //SiparisDurum Listele

        [HttpGet]
        [Route("api/siparisdurumliste")]
        public List<siparisDurumuModel> SiparisDurumListe()
        {
            List<siparisDurumuModel> liste = db.SiparisDurumu.Select(x => new siparisDurumuModel()
            {
                siparisDurumId = x.siparisDurumId,                
                SiparisDurumu1 = x.SiparisDurumu1
              
            }).ToList();
            return liste;
        }

        //SiparisDurum Ekle

        [HttpPost]
        [Route("api/siparisdurumekle")]
        public sonucModel SiparisDurumEkle(siparisDurumuModel model)
        {

            SiparisDurumu yeni = new SiparisDurumu();
            yeni.siparisDurumId = Guid.NewGuid().ToString();
            yeni.SiparisDurumu1 = model.SiparisDurumu1;
            db.SiparisDurumu.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Siparis Durumu Eklendi";

            return sonuc;
        }

        //SiparisDurum Duzenle

        [HttpPut]
        [Route("api/siparisdurumuduzenle")]

        public sonucModel SiparisDurumuDuzenle(siparisDurumuModel model)
        {
            SiparisDurumu kayit = db.SiparisDurumu.Where(s => s.siparisDurumId == model.siparisDurumId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Siparis Durumu Bulunamadı";
                return sonuc;
            }

            kayit.SiparisDurumu1 = model.SiparisDurumu1;

            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Siparis Durumu Düzenlendi";

            return sonuc;
        }

        //SiparisDurum Sil

        [HttpDelete]
        [Route("api/siparisdurumusil/{siparisDId}")]
        public sonucModel SiparisDurumuSil(string siparisDId)
        {

            SiparisDurumu kayit = db.SiparisDurumu.Where(s => s.siparisDurumId == siparisDId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "siparis Durumu Bulunamadı";
                return sonuc;
            }


            db.SiparisDurumu.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Siparis Durumu Silindi";

            return sonuc;
        }

        #endregion

        #region Kargo

        //Kargo Listele

        [HttpGet]
        [Route("api/kargoliste")]
        public List<kargoModel> KargoListe()
        {
            List<kargoModel> liste = db.Kargo.Select(x => new kargoModel()
            {
                kargoId = x.kargoId,
                FirmaAdi = x.FirmaAdi,
                Telefon = x.Telefon

            }).ToList();
            return liste;
        }

        //Kargo Ekle

        [HttpPost]
        [Route("api/kargoekle")]
        public sonucModel KargoEkle(kargoModel model)
        {

            Kargo yeni = new Kargo();
            yeni.kargoId = Guid.NewGuid().ToString();
            yeni.FirmaAdi = model.FirmaAdi;
            yeni.Telefon = model.Telefon;
            db.Kargo.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kargo Firması Eklendi";

            return sonuc;
        }

        //Kargo Duzenle

        [HttpPut]
        [Route("api/siparisdurumuduzenle")]

        public sonucModel KargoDuzenle(kargoModel model)
        {
            Kargo kayit = db.Kargo.Where(s => s.kargoId== model.kargoId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kargo Firması Bulunamadı";
                return sonuc;
            }

            kayit.FirmaAdi = model.FirmaAdi;
            kayit.Telefon = model.Telefon;

            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kargo Firması Düzenlendi";

            return sonuc;
        }

        //Kargo Sil

        [HttpDelete]
        [Route("api/kargosil/{kargoId}")]
        public sonucModel KargoSil(string kargoId)
        {

            Kargo kayit = db.Kargo.Where(s => s.kargoId == kargoId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kargo Firması Bulunamadı";
                return sonuc;
            }


            db.Kargo.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kargo Firması Silindi";

            return sonuc;
        }

        #endregion

        #region OdemeTuru

        //OdemeTuru Listele

        [HttpGet]
        [Route("api/odemeliste")]
        public List<odemeTuruModel> odemeListe()
        {
            List<odemeTuruModel> liste = db.OdemeTuru.Select(x => new odemeTuruModel()
            {
                odemeTuruId  = x.odemeTuruId,
                OdemeCesiti = x.OdemeCesiti

            }).ToList();
            return liste;
        }

        //OdemeTuru Ekle

        [HttpPost]
        [Route("api/odemeekle")]
        public sonucModel OdemeEkle(odemeTuruModel model)
        {

            OdemeTuru yeni = new OdemeTuru();
            yeni.odemeTuruId = Guid.NewGuid().ToString();
            yeni.OdemeCesiti = model.OdemeCesiti;
            db.OdemeTuru.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Ödeme Türü Eklendi";

            return sonuc;
        }

        //OdemeTuru Duzenle

        [HttpPut]
        [Route("api/odemeduzenle")]

        public sonucModel OdemeDuzenle(odemeTuruModel model)
        {
            OdemeTuru kayit = db.OdemeTuru.Where(s => s.odemeTuruId == model.odemeTuruId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Odeme Türü Bulunamadı";
                return sonuc;
            }

            kayit.OdemeCesiti = model.OdemeCesiti;

            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Odeme Türü Düzenlendi";

            return sonuc;
        }

        //OdemeTuru Sil

        [HttpDelete]
        [Route("api/odemesil/{odemeId}")]
        public sonucModel OdemeSil(string odemeId)
        {

            OdemeTuru kayit = db.OdemeTuru.Where(s => s.odemeTuruId == odemeId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Ödeme Türü Bulunamadı";
                return sonuc;
            }


            db.OdemeTuru.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Ödeme Türü Silindi";

            return sonuc;
        }

        #endregion
    }
}
