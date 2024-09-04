using SocialMediaWeb.Models;
using SocialMediaWeb.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialMediaWeb.Controllers
{
    public class UserController : Controller
    {
        private UserRepository userRepository = new UserRepository();
        AuthenticationRepository authenticationRepository = new AuthenticationRepository();

        public ActionResult UserDashboard()
        {
            return View();
        }


        /// <summary>
        /// To see the profile of user who is currently login
        /// </summary>
        /// <returns>  return a view with details of user  </returns>
        public ActionResult ViewProfile()
        {
            try
            {
                int userId = Convert.ToInt32(Session["UserID"]);
                ProfileViewModel model = userRepository.GetUserDetails(userId);

                if (model == null)
                {
                    return HttpNotFound("User not found.");
                }

                return View(model);
            }
            catch (Exception ex)
            {

                return Content("An error occurred while fetching the user profile.");
            }
        }



        /// <summary>
        /// edit the profile of logged in user
        /// </summary>
        /// <param name="id">   id of logged in user </param>
        /// <returns>  the html page with user details to edit </returns>
        public ActionResult EditProfile(int id)
        {
            try
            {
                
                var user = userRepository.GetUserDetailsById(id);
               
                if (user == null)
                {
                    return HttpNotFound();
                }

                var viewModel = new SignupViewModel
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    DateOfBirth = user.DateOfBirth,
                    Gender = user.Gender,
                    ProfilePicture = null, 
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address,
                    StateID = user.StateID,
                    DistrictID = user.DistrictID
                };
                ViewBag.States = authenticationRepository.GetStates();

                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("Error loading profile.");
            }
        }

        /// <summary>
        /// submit the edit details form to update in database
        /// </summary>
        /// <param name="model">  hold the details in the form </param>
        /// <returns>  </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(SignupViewModel model)
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
                    int userId = Convert.ToInt32(Session["UserID"]);
                    userRepository.UpdateUserProfile(model, profilePicturePath, userId);
                    return Content("ProfileUpdatedSuccess"); 
                }
                catch (Exception ex)
                {
                    
                    Console.WriteLine(ex.Message);
                    return Content("Error updating profile.");
                }
            }
            return View(model);
        }


    }
}