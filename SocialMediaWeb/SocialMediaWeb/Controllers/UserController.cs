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
            try
            {
                var posts = userRepository.GetAllPost();
                return View(posts);
            }
            catch (Exception ex)
            {
                // Handle exception
                Console.WriteLine(ex.Message);
                return View(new List<PostDisplayViewModel>());
            }
            
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


        /// <summary>
        /// Load the change password html.
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        /// <summary>
        /// Submit the data to data base for change password
        /// </summary>
        /// <param name="model">   contails old and new password </param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string userEmail = Session["UserEmail"].ToString();
                    var user = authenticationRepository.AuthenticateUser(userEmail, model.OldPassword);
                    if (user != null)
                    {
                        string hashedNewPassword = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
                        userRepository.UpdatePassword(userEmail, hashedNewPassword);

                        return RedirectToAction("Login", "Authentication"); 
                    }
                    else
                    {
 
                        TempData["ErrorMessage"] = "Old password is incorrect";
                    }
                }
                catch (Exception exception)
                {
                    
                    Console.WriteLine(exception.Message);
                    ModelState.AddModelError("", "An error occurred while changing the password.");
                }
            }

            return View(model);
        }



        /// <summary>
        /// User can add a new post
        /// </summary>
        /// <returns></returns>
        public ActionResult AddPost()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPost(PostViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string imageBase64 = null;
                    string imageType = null;

                    if (model.ImageFile != null && model.ImageFile.ContentLength > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            model.ImageFile.InputStream.CopyTo(memoryStream);
                            byte[] imageBytes = memoryStream.ToArray();
                            imageBase64 = Convert.ToBase64String(imageBytes);
                            imageType= model.ImageFile.ContentType;
                        }
                    }

                    var userId = (int)Session["UserId"]; 
                    var createdAt = DateTime.Now;

                    var post = new Post
                    {
                        UserId = userId,
                        Content = model.Content,
                        ImageUrl = imageBase64,
                        CreatedAt = createdAt,
                        ImageType= imageType
                    };

                    userRepository.AddPost(post);
                    return RedirectToAction("UserDashboard"); 
                }
                catch (Exception exception)
                {
                   
                    Console.WriteLine(exception.Message);
                    ModelState.AddModelError("", "An error occurred while adding the post.");
                }
            }

            return View("AddPost", model);
        }


        /// <summary>
        /// managing the post particular user added. 
        /// </summary>
        /// <returns></returns>
        public ActionResult ManagePost()
        {
            try
            {
                var userId = (int)Session["UserId"]; 
                var posts = userRepository.GetPostsByUserId(userId);
                return View(posts);
            }
            catch (Exception exception)
            {
                
                Console.WriteLine(exception.Message);
                return RedirectToAction("UserDashboard");
            }
        }


    /// <summary>
    /// Delete a post which is added by the corresponding user
    /// </summary>
    /// <param name="id">id of logged in user </param>
    /// <returns></returns>
        public ActionResult DeletePost(int id)
        {
            try
            {

                userRepository.DeletePost(id); 
                return RedirectToAction("ManagePost");
            }
            catch (Exception exception)
            {
                
                Console.WriteLine(exception.Message);
                return RedirectToAction("ManagePost");
            }
        }


        /// <summary>
        /// User can report the post if some harmful content is there.
        /// </summary>
        /// <param name="id">id of the corresponting post </param>
        /// <returns></returns>
        public ActionResult ReportPost(int id)
        {
            var post = userRepository.GetPostById(id);
            if (post == null)
            {
                return HttpNotFound();
            }

            var viewModel = new ReportPostViewModel
            {
                PostId = post.PostId,
                Content = post.Content,
                ImageUrl = post.ImageUrl,
                ReportedBy = (int)Session["UserID"]  
            };

            return View(viewModel);
        }

        /// <summary>
        /// store the data that reported by user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ReportPost(ReportPostViewModel model)
        {
            if (ModelState.IsValid)
            {
                userRepository.SaveReport(model.PostId, model.Reason, model.ReportedBy);
                TempData["PostReported"] = "You reported the post";
                return RedirectToAction("UserDashboard");
            }

            return View(model);
        }

    }
}