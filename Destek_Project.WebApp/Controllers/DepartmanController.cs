using Destek.BLL;
using Destek.BLL.Results;
using Destek.Entities;
using Destek_Project.WebApp.Filters;
using Destek_Project.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Destek_Project.WebApp.Controllers
{
    public class DepartmanController : Controller
    {
        private BransManager bransManager = new BransManager();
        // GET: Departman
        [Auth]
        public ActionResult Index()
        {
            return View(bransManager.List());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Brans brans = bransManager.Find(x => x.Id == id.Value);

            if (brans == null)
            {
                return HttpNotFound();
            }

            return View(brans);
        }
        [Auth]
        public ActionResult Create()
        {
            return View();
        }
        [Auth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Brans brans, HttpPostedFileBase PostImage)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedName");

            if (ModelState.IsValid)
            {
               
                bransManager.Insert(brans);
                CacheHelper.RemoveCategoriesFromCache();

                return RedirectToAction("Index");
            }

            return View(brans);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Brans brans = bransManager.Find(x => x.Id == id.Value);

            if (brans == null)
            {
                return HttpNotFound();
            }
            return View(brans);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Brans brns, HttpPostedFileBase PostImage)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedName");

            if (ModelState.IsValid)
            {
               
                Brans brans = bransManager.Find(x => x.Id == brns.Id);
                brans.Title = brns.Title;
                brans.Description = brns.Description;

                bransManager.Update(brans);
                CacheHelper.RemoveCategoriesFromCache();

                return RedirectToAction("Index");
            }
            return View(brns);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Brans brans = bransManager.Find(x => x.Id == id.Value);

            if (brans == null)
            {
                return HttpNotFound();
            }

            return View(brans);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Brans brans = bransManager.Find(x => x.Id == id);
            bransManager.Delete(brans);

            CacheHelper.RemoveCategoriesFromCache();


            return RedirectToAction("Index");
        }
    }
}