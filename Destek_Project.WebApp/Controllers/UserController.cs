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
    public class UserController : Controller
    {
        private DestekUserManager destekUserManager = new DestekUserManager();
        // GET: User
        [Auth]
        public ActionResult Index()
        {
            var destekUser = destekUserManager.ListQueryable().OrderByDescending(
                x => x.ModifiedOn);

            return View(destekUser.ToList());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DestekUser destekUser = destekUserManager.Find(x => x.Id == id.Value);

            if (destekUser == null)
            {
                return HttpNotFound();
            }

            return View(destekUser);
        }
        public ActionResult Create()
        {
            ViewBag.BransId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DestekUser destekUser)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedName");

            if (ModelState.IsValid)
            {
                BusinessLayerResult<DestekUser> res = destekUserManager.Insert(destekUser);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(destekUser);
                }

                return RedirectToAction("Index");
            }

            ViewBag.BransId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", destekUser.BransId);
            return View(destekUser);
        }
         public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DestekUser destekUser = destekUserManager.Find(x => x.Id == id.Value);

            if (destekUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.BransId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", destekUser.BransId);

            return View(destekUser);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DestekUser destekUser)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedName");

            if (ModelState.IsValid)
            {
                BusinessLayerResult<DestekUser> res = destekUserManager.Update(destekUser);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(destekUser);
                }

                return RedirectToAction("Index");
            }
            ViewBag.BransId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", destekUser.BransId);
            return View(destekUser);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DestekUser destekUser = destekUserManager.Find(x => x.Id == id.Value);

            if (destekUser == null)
            {
                return HttpNotFound();
            }

            return View(destekUser);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DestekUser destekUser = destekUserManager.Find(x => x.Id == id);
            destekUserManager.DeleteUser(destekUser);


            return RedirectToAction("Index");
        }
    }
}