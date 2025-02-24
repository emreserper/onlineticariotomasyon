﻿using MvcOnlineTicariOtomasyon.Models.Siniflar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MvcOnlineTicariOtomasyon.Controllers
{
    public class CariPanelController : Controller
    {
        // GET: CariPanel
        Context c = new Context();
        [Authorize]
        public ActionResult Index()
        {
            var mail = (string)Session["CariMail"];
            var degerler = c.Mesajs.Where(x => x.Alici == mail).ToList();
            ViewBag.m = mail;

            var mailid = c.Caris.Where(x => x.CariMail == mail).Select(y => y.CariID).FirstOrDefault();
            ViewBag.mailId = mailid;
            var toplamsatis = c.SatisHarekets.Where(x => x.CariID == mailid).Count();
            ViewBag.toplamSatis = toplamsatis;
            var sehir = c.Caris.Where(x => x.CariMail == mail ).Select(y => y.CariSehir).FirstOrDefault();
            ViewBag.sehir = sehir;
            //var toplamtutar = c.SatisHarekets.Where(x => x.CariID == mailid).Sum(y => y.ToplamTutar);
            //ViewBag.toplamTutar = toplamtutar;
            //var toplamurunsayisi = c.SatisHarekets.Where(x => x.CariID == mailid).Sum(y => y.Adet);
            //ViewBag.toplamUrun = toplamurunsayisi;
            var adsoyad = c.Caris.Where(x => x.CariMail == mail).Select(y => y.CariAd + " " + y.CariSoyad).FirstOrDefault();
            ViewBag.adsoyad = adsoyad;
            return View(degerler);

        }
        [Authorize]
        public ActionResult Siparislerim()
        {
            var mail = (string)Session["CariMail"];
            var id = c.Caris.Where(x => x.CariMail == mail.ToString()).Select(y => y.CariID).FirstOrDefault();
            var degerler = c.SatisHarekets.Where(x => x.CariID == id).ToList();
            return View(degerler);
        }
        [Authorize]
        public ActionResult GelenMesajlar()
        {
            var mail = (string)Session["CariMail"];
            var mesajlar = c.Mesajs.Where(x=>x.Alici==mail).OrderByDescending(x=>x.MesajID).ToList();
            var gelensayisi = c.Mesajs.Count(x => x.Alici == mail).ToString();
            ViewBag.d1 = gelensayisi;
            var gidensayisi = c.Mesajs.Count(x => x.Gonderici == mail).ToString();
            ViewBag.d2 = gidensayisi;
            return View(mesajlar);
        }
        [Authorize]
        public ActionResult GidenMesajlar()
        {
            var mail = (string)Session["CariMail"];
            var mesajlar = c.Mesajs.Where(x => x.Gonderici == mail).OrderByDescending(z => z.MesajID).ToList();
            var gidensayisi = c.Mesajs.Count(x => x.Gonderici == mail).ToString();
            ViewBag.d2 = gidensayisi;
            var gelensayisi = c.Mesajs.Count(x => x.Alici == mail).ToString();
            ViewBag.d1 = gelensayisi;
            return View(mesajlar);
        }
        [Authorize]
        public ActionResult MesajDetay(int id)
        {
            var degerler = c.Mesajs.Where(x => x.MesajID == id).ToList();
            var mail = (string)Session["CariMail"];
            var gidensayisi = c.Mesajs.Count(x => x.Gonderici == mail).ToString();
            ViewBag.d2 = gidensayisi;
            var gelensayisi = c.Mesajs.Count(x => x.Alici == mail).ToString();
            ViewBag.d1 = gelensayisi;
            return View(degerler);
        }
        [Authorize]
        [HttpGet]
        public ActionResult YeniMesaj()
        {
            var mail = (string)Session["CariMail"];
            var gidensayisi = c.Mesajs.Count(x => x.Gonderici == mail).ToString();
            ViewBag.d2 = gidensayisi;
            var gelensayisi = c.Mesajs.Count(x => x.Alici == mail).ToString();
            ViewBag.d1 = gelensayisi;
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult YeniMesaj(Mesaj m)
        {
            var mail = (string)Session["CariMail"];
            m.Tarih = DateTime.Parse(DateTime.Now.ToShortDateString());
            m.Gonderici = mail;
            c.Mesajs.Add(m);
            c.SaveChanges();
            return View();
        }
        [Authorize]
        public ActionResult KargoTakip(string p)
        {
            var k = from x in c.KargoDetays select x;
            k = k.Where(y => y.TakipKodu.Contains(p));
            return View(k.ToList());
        }
        [Authorize]
        public ActionResult CariKargoTakip(string id)
        {
            var degerler = c.KargoTakips.Where(x => x.TakipKodu == id).ToList();
            return View(degerler);
        }
        [Authorize]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }
        public PartialViewResult Partial1()
        {
            var mail = (string)Session["CariMail"];
            var id = c.Caris.Where(x => x.CariMail == mail).Select(y => y.CariID).FirstOrDefault();
            var caribul = c.Caris.Find(id);
            return PartialView("Partial1", caribul);
        }

        public PartialViewResult Partial2()
        {

            var veriler = c.Mesajs.Where(x => x.Gonderici == "admin").ToList();
            return PartialView(veriler);
        }
        public ActionResult CariBilgiGuncelle(Cari cr)
        {
            var cari = c.Caris.Find(cr.CariID);
            cari.CariAd = cr.CariAd;
            cari.CariSoyad = cr.CariSoyad;
            cari.CariMail = cr.CariMail;
            cari.CariSehir = cr.CariSehir;
            cari.CariSifre = cr.CariSifre;
            c.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}