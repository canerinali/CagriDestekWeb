using Destek.BLL;
using Destek.BLL.Results;
using Destek.Entities;
using Destek.Entities.ValueObject;
using Destek_Project.WebApp.Filters;
using Destek_Project.WebApp.Models;
using Destek_Project.WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Destek_Project.WebApp.Controllers
{
    public class HomeController : Controller
    {
        Destek.BLL.Baglan bgln = new Destek.BLL.Baglan();
        private DestekUserManager destekUserManager = new DestekUserManager();
        // GET: Home
        [Auth]
        public ActionResult Index()
        {
            
            return View();
        }
        public ActionResult ShowProfile(DestekUser user)
        {
            BusinessLayerResult<DestekUser> res =
            destekUserManager.GetUserById(CurrentSession.User.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };

                return View("Error", errorNotifyObj);
            }

            return View(res.Result);
        }
        [Auth]
        public ActionResult EditProfile()
        {
            BusinessLayerResult<DestekUser> res = destekUserManager.GetUserById(CurrentSession.User.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };

                return View("Error", errorNotifyObj);
            }

            return View(res.Result);
        }
        [Auth]
        [HttpPost]
        public ActionResult EditProfile(DestekUser model, HttpPostedFileBase ProfileImage)
        {
            if (ModelState.IsValid)
            {


                if (ProfileImage != null &&
                (ProfileImage.ContentType == "image/jpeg" ||
                ProfileImage.ContentType == "image/jpg" ||
                ProfileImage.ContentType == "image/png"))
                {
                    string filename = $"user_{model.Id}.{ProfileImage.ContentType.Split('/')[1]}";

                    ProfileImage.SaveAs(Server.MapPath($"~/images/{filename}"));
                    model.ProfileImageFilename = filename;
                }

                BusinessLayerResult<DestekUser> res = destekUserManager.UpdateProfile(model);

                if (res.Errors.Count > 0)
                {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Items = res.Errors,
                        Title = "Profil Güncellenemedi.",
                        RedirectingUrl = "/Home/EditProfile"
                    };

                    return View("Error", errorNotifyObj);
                }

                // Profil güncellendiği için session güncellendi.
                CurrentSession.Set<DestekUser>("login", res.Result);

                return RedirectToAction("ShowProfile");
            }
            return View(model);
        }
        [Auth]
        public ActionResult DeleteProfile()
        {
            BusinessLayerResult<DestekUser> res =
                 destekUserManager.RemoveUserById(CurrentSession.User.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Items = res.Errors,
                    Title = "Profil Silinemedi.",
                    RedirectingUrl = "/Home/ShowProfile"
                };

                return View("Error", errorNotifyObj);
            }

            Session.Clear();

            return RedirectToAction("Index");
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                BusinessLayerResult<DestekUser> res = destekUserManager.LoginUser(model);

                if (res.Errors.Count > 0)
                {
                    // Hata koduna göre özel işlem yapmamız gerekirse..
                    // Hatta hata mesajına burada müdahale edilebilir.
                    // (Login.cshtml'deki kısmında açıklama satırı şeklinden kurtarınız)
                    //
                    //if (res.Errors.Find(x => x.Code == ErrorMessageCode.UserIsNotActive) != null)
                    //{
                    //    ViewBag.SetLink = "http://Home/Activate/1234-4567-78980";
                    //}

                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));

                    return View(model);
                }

                CurrentSession.Set<DestekUser>("login", res.Result); // Session'a kullanıcı bilgi saklama..
                return RedirectToAction("Index");   // yönlendirme..
            }
            return View(model);
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
    }
}