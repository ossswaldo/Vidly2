using Vidly2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace Vidly2.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Autherize(Vidly2.Models.User userModel)
        {
            using (LoginDataBaseEntities db = new LoginDataBaseEntities())
            {
                var userDetails = db.Users.Where(x => x.Email == userModel.Email && x.Password == userModel.Password).FirstOrDefault();
                var admins = userDetails.Role.Contains("Admin");
                var pid = userDetails.Name;


                if (userDetails == null)
                {
                    userModel.LoginErrorMessage = "Wrong username or password.";
                    return View("Index", userModel);
                }
                else {
                    if (admins)
                    {
                        Session["userID"] = userDetails.Email;
                        Session["PID"] = pid;
                        return RedirectToAction("Index", "Keys");
                        
                    }
                    else
                    {
                        Session["userID"] = userDetails.Email;
                        Session["PID"] = pid;
                        return RedirectToAction("UserIndex", "Keys");
                    }
                    
                }
            }
            return View();
        }

        public ActionResult LogOut() 
        {
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        
        
        }
    }
}