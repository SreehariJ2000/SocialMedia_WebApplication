using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using SocialMediaWeb.Models;
using SocialMediaWeb.Repository;
using BCrypt.Net;

namespace SocialMediaWeb.Controllers
{
    public class AuthenticationController : Controller
    {
         AuthenticationRepository authenticationRepository = new AuthenticationRepository();

        
        /// <summary>
        /// For rendering the sign up page . 
        /// </summary>
        /// <returns>  it will return sigh up html page </returns>
        public ActionResult Signup()
        {
            try
            {
                ViewBag.States = authenticationRepository.GetStates();
                return View();
            }
            catch (Exception exception)
            {
                
                Console.WriteLine(exception.Message);
                return RedirectToAction("Signup", "Authentication");
            }
        }


        /// <summary>
        /// when the user submit the sign up form . 
        /// </summary>
        /// <param name="model">  contails the data submitted from form </param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signup(Signup signup)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string profilePicture = null;
                   

                    if (Request.Files["ProfilePicture"] != null && Request.Files["ProfilePicture"].ContentLength > 0)
                    {
                        var profilePictureFile = Request.Files["ProfilePicture"];
                        using (var memoryStream = new MemoryStream())
                        {
                            profilePictureFile.InputStream.CopyTo(memoryStream);
                            byte[] imageBytes = memoryStream.ToArray();
                            profilePicture = Convert.ToBase64String(imageBytes);               
                        }
                    }

                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(signup.UserPassword);

                    Users user = new Users
                    {
                        Email = signup.Email,
                        UserPassword = hashedPassword
                    };

                    Profile profile = new Profile
                    {
                        FirstName = signup.FirstName,
                        LastName = signup.LastName,
                        DateOfBirth = signup.DateOfBirth,
                        Gender = signup.Gender,
                        ProfilePicture = profilePicture,
                        PhoneNumber = signup.PhoneNumber,
                        Address = signup.Address,
                        StateID = signup.StateID,
                        DistrictID = signup.DistrictID
                    };

                    authenticationRepository.AddUser(user, profile);
                    return RedirectToAction("Login", "Authentication");
                }
                catch (Exception exception)
                {
                   
                    Console.WriteLine(exception.Message);
                    return RedirectToAction("Signup", "Authentication");
                }
            }

            try
            {
                ViewBag.States = authenticationRepository.GetStates();
            }
            catch (Exception exception)
            {
               
                Console.WriteLine(exception.Message);
                return RedirectToAction("Signup", "Authentication");
            }

            return View(signup);
        }


        /// <summary>
        ///  For getting the district base on state id . It will pass to the signup form as dropdowm. 
        /// </summary>
        /// <param name="stateId">  get from the signup form .
        /// used to fetch corresponding district based on state </param>
        /// <returns>  return a json list consist of districts </returns>
        public JsonResult GetCities(int stateId)
        {
            try
            {
                var cities = authenticationRepository.GetDistricts(stateId)
                    .Select(d => new SelectListItem
                    {
                        Value = d.Value,
                        Text = d.Text
                    }).ToList();

                return Json(cities, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                
                Console.WriteLine("json error",exception.Message);
                return Json(new List<SelectListItem>(), JsonRequestBehavior.AllowGet); 
            }
        }



        /// <summary>
        /// Load the login page
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }


        /// <summary>
        /// Post data to login page.
        /// </summary>
        /// <param name="model">  consist of data submitted by user from login form</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = authenticationRepository.AuthenticateUser(loginViewModel.Email, loginViewModel.Password);

                    if (user != null)
                    {
                        
                        Session["UserID"] = user.UserID;
                        Session["UserEmail"] = user.Email;
                        Session["UserRole"] = user.Role;

                        if (user.Role == "Admin")
                        {
                            return RedirectToAction("AdminDashboard", "Admin");
                        }
                        else if (user.Role == "User")
                        {
                            return RedirectToAction("UserDashboard", "User");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid email or password.");
                    }
                }
                catch (Exception exception)
                {
                    
                    Console.WriteLine(exception.Message);
                    return RedirectToAction("Authentication", "Login");
                }
            }
            return View(loginViewModel);
        }


        /// <summary>
        /// For logout
        /// </summary>
        /// <returns>  redirect to  the login page </returns>
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login");
        }


    }
}
