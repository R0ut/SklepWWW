﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SklepWWW.DAL;
using SklepWWW.Models;
using SklepWWW.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static SklepWWW.App_Start.IdentityConfig;

namespace SklepWWW.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        KursyContext db = new KursyContext();

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            Error
        }
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            if (TempData["ViewData"] != null)
            {
                ViewData = (ViewDataDictionary)TempData["ViewData"];
            }
            if (User.IsInRole("Admin"))
            {
                ViewBag.UserIsAdmin = true;
            }
            else
                ViewBag.UserIsAdmin = false;

            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null)
            {
                return View("Error");
            }
            var model = new ManageCredentialsViewModel
            {
                Message = message,
                DaneUzytkownika = user.DaneUzytkownika
            };
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeProfile([Bind(Prefix = "DaneUzytkownika")]DaneUzytkownika daneUzytownika)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                user.DaneUzytkownika = daneUzytownika;
                var result = await UserManager.UpdateAsync(user);

                AddErrors(result);
            }
            if (!ModelState.IsValid)
            {
                TempData["ViewData"] = ViewData;
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword([Bind(Prefix = "ChangePasswordViewModel")]ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ViewData"] = ViewData;
                return RedirectToAction("Index");
            }

            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);

            if (!ModelState.IsValid)
            {
                TempData["ViewData"] = ViewData;
                return RedirectToAction("Index");
            }

            var message = ManageMessageId.ChangePasswordSuccess;
            return RedirectToAction("Index", new { Message = message });
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("password-error", error);
            }
        }
        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }

        public ActionResult ListaZamowien()
        {

            bool isAdmin = User.IsInRole("Admin");
            ViewBag.UserIsAdmin = isAdmin;

            IEnumerable<Zamowienie> zamowieniaUzytkownika;

            // Dla administratora zwracamy wszystkie zamowienia
            if (isAdmin)
            {
                zamowieniaUzytkownika = db.Zamowienia.Include("PozycjeZamowienia").OrderByDescending(o => o.DataDodania).ToArray();
            }
            else
            {
                var userId = User.Identity.GetUserId();
                zamowieniaUzytkownika = db.Zamowienia.Where(o => o.UserId == userId).Include("PozycjeZamowienia").OrderByDescending(o => o.DataDodania).ToArray();
            }

            return View(zamowieniaUzytkownika);
        }
        [HttpPost]
        [Authorize(Roles ="Admin")]
        public StanZamowienia ZmianaStanuZamowienia(Zamowienie zamowienie)
        {
            Zamowienie zamowienieDOModyfikacji = db.Zamowienia.Find(zamowienie.ZamowienieId);
            zamowienieDOModyfikacji.StanZamowienia = zamowienie.StanZamowienia;
            db.SaveChanges();

            return zamowienie.StanZamowienia;
        }


    }
}