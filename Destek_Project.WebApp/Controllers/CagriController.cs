using Destek.BLL;
using Destek.BLL.Results;
using Destek.Entities;
using Destek_Project.WebApp.Filters;
using Destek_Project.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Destek_Project.WebApp.Controllers
{
  
    public class CagriController : Controller
    {
        private MessageManager messageManager = new MessageManager();

        // GET: Cagri
        [Auth]
        public ActionResult Index()
        {
            var messages = messageManager.ListQueryable().Include("Brans").Include("Owner").Where(
                x => x.Brans.Id == CurrentSession.User.BransId).OrderByDescending(
                x => x.ModifiedOn);

            return View(messages.ToList());
        }
        [Auth]
        public ActionResult CagriEkle()
        {
            ViewBag.BransId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title");
            return View();
        }
        [Auth]
        [HttpPost]
        public ActionResult CagriEkle(Message message, HttpPostedFileBase PostImage)
        {
            message.MesajDurum = "Aktif";
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedName");

            if (ModelState.IsValid)
            {
                if (PostImage != null &&
                (PostImage.ContentType == "image/jpeg" ||
                PostImage.ContentType == "image/jpg" ||
                PostImage.ContentType == "image/png"))
                {
                    string filename = $"post_{message.Id}.{PostImage.ContentType.Split('/')[1]}";

                    PostImage.SaveAs(Server.MapPath($"~/images/{filename}"));
                    message.PostImageFilename = filename;
                }
                message.Owner = CurrentSession.User;
                message.MesajDurum = "Aktif";
                messageManager.Insert(message);
                return RedirectToAction("Index");
            }
            BusinessLayerResult<Message> res = messageManager.InsertPostFoto(message);

            ViewBag.BransId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", message.BransId);
            return View(message);
        }
        [Auth]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = messageManager.Find(x => x.Id == id);
            if (message == null)
            {
                return HttpNotFound();
            }
            ViewBag.BransId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", message.BransId);
            return View(message);
        }

        [Auth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Message message, HttpPostedFileBase PostImage)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedName");

            if (ModelState.IsValid)
            {
                if (PostImage != null &&
                   (PostImage.ContentType == "image/jpeg" ||
                   PostImage.ContentType == "image/jpg" ||
                   PostImage.ContentType == "image/png"))
                {
                    string filename = $"post_{message.Id}.{PostImage.ContentType.Split('/')[1]}";

                    PostImage.SaveAs(Server.MapPath($"~/images/{filename}"));
                    message.PostImageFilename = filename;
                }
                BusinessLayerResult<Message> res = messageManager.UpdatePostFoto(message);

                return RedirectToAction("Index");
            }
            ViewBag.BransId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", message.BransId);
            return View(message);
        }
        [Auth]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = messageManager.Find(x => x.Id == id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }
        public ActionResult Aktif()
        {
            var messages = messageManager.ListQueryable().Include("Brans").Include("Owner").Where(
                x => x.Brans.Id == CurrentSession.User.BransId).OrderByDescending(
                x => x.ModifiedOn);

            return View(messages.ToList());
        }
        public ActionResult Pasif()
        {
            var messages = messageManager.ListQueryable().Include("Brans").Include("Owner").Where(
                x => x.Brans.Id == CurrentSession.User.BransId).OrderByDescending(
                x => x.ModifiedOn);

            return View(messages.ToList());
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Message message = messageManager.Find(x => x.Id == id.Value);

            if (message == null)
            {
                return HttpNotFound();
            }

            return View(message);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Message message = messageManager.Find(x => x.Id == id);
            messageManager.Delete(message);
            return RedirectToAction("Index");
        }

    }
}