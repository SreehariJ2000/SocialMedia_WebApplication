using SocialMediaWeb.Models;
using SocialMediaWeb.Repository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialMediaWeb.Controllers
{
    public class AdminController : Controller
    {
        private AdminRepository adminRepository = new AdminRepository();
        private UserRepository userRepository = new UserRepository();

        public ActionResult AdminDashboard()
        {
            try
            {
                var posts = userRepository.GetAllPost();
                return View(posts);
            }
            catch (Exception exception)
            {
               
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

                Console.WriteLine(exception.Message);
                return RedirectToAction("ReportedPost");
            }
        }


        public ActionResult AdminAddPost()
        {
            return View();
        }


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

                    Console.WriteLine(exception.Message);
                    ModelState.AddModelError("", "An error occurred while adding the post.");
                }
            }

            return View("AdminAddPost", postViewModel);
        }

    }
}