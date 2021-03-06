﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eCommerceSite.Models;
using eCommerceSite.Services;
using eCommerceSite.Data;

namespace eCommerceSite.Controllers
{
    public class HomeController : Controller
    {
        private IMailService _mail;
        private eCommerceRepository _rep;
        public HomeController(IMailService mail)
        {
            
            _mail = mail;
            _rep = new eCommerceRepository();
        }

        public ActionResult Index()
        {

            var items = _rep.GetItems();
            return View(items);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult Contact(ContactModel model)
        {
            var msg = string.Format("Comment from: {1}{0}Email:{2}{0}subject:{3}{0} Comment:{4}{0}",
                Environment.NewLine, model.name, model.email, model.subject, model.message);

            if (_mail.SendMail("companyemail@company.com", "customer@customer.com", "website contact", msg))
            {
                ViewBag.MailSent = true;
            }
            return View();
        }
        public ActionResult MyCart()
        {
            
            return View();
        }
        public ActionResult Item(int Id) 
        {
            var x = _rep.GetItemDetails().Where(i => i.ItemId == Id).ToList();
            ViewData["Reviews"] = _rep.GetReviewsByItem(Id).ToList();
            ViewData["ItemDetails"] = x;
            ViewData["ReviewsCount"] = _rep.GetReviewsByItem(Id).ToList().Count;
            var result = _rep.GetObjects().Find(Id);
            
            return View(result);
        }
        public FileContentResult ImageRender(int Id)
        {
            byte[] ImgArray = _rep.GetObjects().Find(Id).Thumbnail;
            return new FileContentResult(ImgArray, "image/jpeg");
        }
    }
}
