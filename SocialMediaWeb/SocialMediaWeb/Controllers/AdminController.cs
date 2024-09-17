using SocialMediaWeb.Models;
using SocialMediaWeb.Repository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebGrease.Activities;

namespace SocialMediaWeb.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private AdminRepository adminRepository = new AdminRepository();
        private UserRepository userRepository = new UserRepository();
        AuthenticationRepository authenticationRepository = new AuthenticationRepository();
        ErrorLog errorLog = new ErrorLog();


        public ActionResult AdminDashboard()
        {
            try
            {
                var posts = userRepository.GetAllPost();
                return View(posts);
            }
            catch (Exception exception)
            {
                errorLog.LogError(exception);
                Console.WriteLine(exception.Message);
                return View(new List<PostDisplayViewModel>());
            }
        }


        /// <summary>
        /// Show the post reported by users
        /// </summary>
        /// <returns></returns>
        public ActionResult ReportedPost()
        {
            var reportedPosts = adminRepository.GetReportedPosts();
            return View(reportedPosts);
        }

        /// <summary>
        /// delete the post reported by users
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteReportedPost(int id)
        {
            try
            {
                adminRepository.DeleteReportedPost(id);
                TempData["PostDelete"] = "Post was removed";

                return RedirectToAction("ReportedPost");
            }
            catch (Exception exception)
            {
                errorLog.LogError(exception);
                Console.WriteLine(exception.Message);
                return RedirectToAction("ReportedPost");
            }
        }

        /// <summary>
        /// load add post page
        /// </summary>
        /// <returns></returns>
        public ActionResult AdminAddPost()
        {
            return View();
        }


        /// <summary>
        /// Admin can add post
        /// </summary>
        /// <param name="postViewModel"> data (image, content ,id)</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminAddPost(PostViewModel postViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string imageBase64 = null;
                    string imageType = null;

                    if (postViewModel.ImageFile != null && postViewModel.ImageFile.ContentLength > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            postViewModel.ImageFile.InputStream.CopyTo(memoryStream);
                            byte[] imageBytes = memoryStream.ToArray();
                            imageBase64 = Convert.ToBase64String(imageBytes);
                            imageType = postViewModel.ImageFile.ContentType;
                        }
                    }

                    var userId = (int)Session["UserId"];
                    var createdAt = DateTime.Now;

                    var post = new Post
                    {
                        UserId = userId,
                        Content = postViewModel.Content,
                        ImageUrl = imageBase64,
                        CreatedAt = createdAt,
                        ImageType = imageType
                    };

                    userRepository.AddPost(post);
                    return RedirectToAction("AdminDashboard");
                }
                catch (Exception exception)
                {
                    errorLog.LogError(exception);
                    Console.WriteLine(exception.Message);
                    ModelState.AddModelError("", "An error occurred while adding the post.");
                }
            }

            return View("AdminAddPost", postViewModel);
        }


        public ActionResult AdminChangePassword()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminChangePassword(ChangePassword changePassword)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string userEmail = Session["UserEmail"].ToString();
                    var user = authenticationRepository.AuthenticateUser(userEmail, changePassword.OldPassword);
                    if (user != null)
                    {
                        string hashedNewPassword = BCrypt.Net.BCrypt.HashPassword(changePassword.NewPassword);
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
                    errorLog.LogError(exception);
                    Console.WriteLine(exception.Message);
                    ModelState.AddModelError("", "An error occurred while changing the password.");
                }
            }

            return View(changePassword);
        }


        public ActionResult UserList()
        {
            var users = adminRepository.GetUsersStatus();
            return View(users);
        }


        public ActionResult ViewUserDetails(int id)
        {
            var user = userRepository.GetUserDetails(id);
            return View(user);
        }

        public ActionResult ChangeUserStatus(int id, string status)
        {
            try
            {
                adminRepository.UpdateUserStatus(id, status);
                return RedirectToAction("ViewUserDetails", new { id = id });


            }
            catch (Exception exception)
            {
                errorLog.LogError(exception);
                return RedirectToAction("ViewUserDetails", new { id = id });
            }

           ;
        }


    }
}