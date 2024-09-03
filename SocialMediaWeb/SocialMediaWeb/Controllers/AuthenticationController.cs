using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using SocialMediaWeb.Models;
using SocialMediaWeb.Repository;

namespace SocialMediaWeb.Controllers
{
    public class AuthenticationController : Controller
    {
         AuthenticationRepository authenticationRepository = new AuthenticationRepository();

        // GET: Authentication/Signup
        public ActionResult Signup()
        {
            try
            {
                ViewBag.States = authenticationRepository.GetStates();
                return View();
            }
            catch (Exception ex)
            {
                
                Console.WriteLine("NOTHING");
                return Content("error3");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signup(SignupViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string profilePicturePath = null;
                      
                    if (model.ProfilePicture != null && model.ProfilePicture.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(model.ProfilePicture.FileName);
                        var path = Path.Combine(Server.MapPath("~/Content/Images/ProfilePicture"), fileName);
                        model.ProfilePicture.SaveAs(path);
                        profilePicturePath = "/Content/Images/ProfilePicture/" + fileName;
                    }

                    Users user = new Users
                    {
                        Email = model.Email,
                        UserPassword = model.UserPassword
                    };

                    Profile profile = new Profile
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        DateOfBirth = model.DateOfBirth,
                        Gender = model.Gender,
                        ProfilePicture = profilePicturePath,
                        PhoneNumber = model.PhoneNumber,
                        Address = model.Address,
                        StateID = model.StateID,
                        DistrictID = model.DistrictID
                    };

                    authenticationRepository.AddUser(user, profile);
                    return Content("data added");
                }
                catch (Exception ex)
                {
                    // Log the exception or handle it according to your needs
                    Console.WriteLine(ex.Message);
                    return Content("error4");
                }
            }

            try
            {
                ViewBag.States = authenticationRepository.GetStates();
            }
            catch (Exception ex)
            {
               
                Console.WriteLine(ex.Message);
                return Content("error5");
            }

            return View(model);
        }

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
            catch (Exception ex)
            {
                
                Console.WriteLine("json error");
                return Json(new List<SelectListItem>(), JsonRequestBehavior.AllowGet); 
            }
        }
    }
}
