using SocialMediaWeb.Models;
using SocialMediaWeb.Repository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
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
            catch (SqlException sqlException)
            {
               
                return Content("Database error occurred.");
            }
            catch (Exception exception)
            {
                return RedirectToAction("Login", "Authentication");
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
            catch (Exception exception)
            {

                return RedirectToAction("UserDashboard", "User");
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

                var viewModel = new Signup
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    DateOfBirth = user.DateOfBirth,
                    Gender = user.Gender,
                    ProfilePicture = user.ProfilePicture, 
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address,
                    StateID = user.StateID,
                    DistrictID = user.DistrictID
                };
                ViewBag.States = authenticationRepository.GetStates();

                return View(viewModel);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
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
        public ActionResult EditProfile(Signup signup)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string profilePicture = null;


                    if (Request.Files["ProfilePicture"] != null && Request.Files["ProfilePicture"].ContentLength > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            var profilePictureFile = Request.Files["ProfilePicture"];
                            profilePictureFile.InputStream.CopyTo(memoryStream);
                            byte[] imageBytes = memoryStream.ToArray();
                            profilePicture = Convert.ToBase64String(imageBytes);
                        }
                    }
                    else
                    {       
                        profilePicture = signup.ProfilePicture; 
                    }
                    int userId = Convert.ToInt32(Session["UserID"]);
                    userRepository.UpdateUserProfile(signup, profilePicture, userId);
                    return RedirectToAction("ViewProfile", "User"); 
                }
                catch (Exception exception)
                {
                    
                    Console.WriteLine(exception.Message);
                    int userId = Convert.ToInt32(Session["UserID"]);
                    return RedirectToAction("EditProfile", new { id = userId });
                }
            }
            return View(signup);
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
        public ActionResult ChangePassword(ChangePassword changePassword)
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
                        TempData["Sucess-Message"] = "Password updated Please login again";
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

            return View(changePassword);
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
        public ActionResult AddPost(PostViewModel postViewModel)
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
                            imageType= postViewModel.ImageFile.ContentType;
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

            return View("AddPost", postViewModel);
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
            try
            {
                var post = userRepository.GetPostById(id);
                if (post == null)
                {
                    return HttpNotFound();
                }

                var viewModel = new ReportPost
                {
                    PostId = post.PostId,
                    Content = post.Content,
                    ImageUrl = post.ImageUrl,
                    ReportedBy = (int)Session["UserID"]
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
               Console.Write(ex.Message);
                return RedirectToAction("UserDashboard");
            }
        }


        /// <summary>
        /// store the data that reported by user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ReportPost(ReportPost reportPost)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    userRepository.SaveReport(reportPost.PostId, reportPost.Reason, reportPost.ReportedBy);
                    TempData["Message"] = "You reported the post";
                    return RedirectToAction("UserDashboard");
                }

                return View(reportPost);
            }
            catch(Exception exception)
            {
            
                Console.WriteLine(exception.Message);
                return RedirectToAction("UserDashboard");
             }
           
        }

        /// <summary>
        /// navigate to friends  page
        /// </summary>
        /// <returns></returns>
        public ActionResult FindFriends()
        {
            return View();
        }


        /// <summary>
        /// search friends based on name
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public ActionResult SearchFriends(string searchTerm)
        {

            try
            {
                var profiles = userRepository.SearchProfiles(searchTerm);

                var result = profiles.Select(p => new
                {
                    p.UserID,
                    p.FirstName,
                    p.LastName,
                    ProfilePicture = string.IsNullOrEmpty(p.ProfilePicture) ? null : "data:image/jpeg;base64," + p.ProfilePicture
                });
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception) { 
                   Console.WriteLine(exception.Message);
                return RedirectToAction("FindFriends");

            }
            
        }



        /// <summary>
        /// To view friends profile
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ActionResult ViewFriendsProfile(int userId)
        {
            try {
                var profile = userRepository.GetUserDetails(userId);
                var posts = userRepository.GetPostsByUserId(userId);
                var loggedInUserId = Convert.ToInt32(Session["UserID"]);
                var isFollowing = userRepository.IsUserFollowing(loggedInUserId, profile.UserID);
                var followersCount = userRepository.GetFollowersCount(profile.UserID);
                var followingCount = userRepository.GetFollowingCount(profile.UserID);

                var viewModel = new FriendsProfile
                {
                    Profile = profile,
                    Posts = posts,
                    IsFollower = isFollowing,
                    FollowersCount = followersCount,
                    FollowingCount = followingCount
                };

                return View(viewModel);
            }
            catch (Exception exception) { 
                Console.WriteLine(exception.Message);
                return RedirectToAction("FindFriends");
            }
            
        }


        /// <summary>
        /// follow a Person
        /// </summary>
        /// <param name="userId"> the id to person </param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult Follow(int userId)
        {
            try
            {
                var loggedInUserId = Convert.ToInt32(Session["UserID"]);
                userRepository.FollowUser(loggedInUserId, userId);

                return RedirectToAction("ViewFriendsProfile", new { userId });
            }
            catch (Exception exception)
            {
                return RedirectToAction("ViewFriendsProfile", new { userId });
            }
        }

        /// <summary>
        ///            To unfollow user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult Unfollow(int userId)
        {
            try
            {
                var loggedInUserId = Convert.ToInt32(Session["UserID"]);
                userRepository.UnfollowUser(loggedInUserId, userId);

                return RedirectToAction("ViewFriendsProfile", new { userId });
            }
            catch (Exception exception)
            {
                return RedirectToAction("ViewFriendsProfile", new { userId });
            }
        }


        /// <summary>
        /// Add comment to a particular post
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="commentText"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult AddComment(int postId, string commentText)
        {
            try
            {
                int userId = Convert.ToInt32(Session["UserID"]);
                userRepository.AddComment(postId, userId, commentText);
                return RedirectToAction("UserDashboard");
            }
            catch (Exception exception)
            {     
                Console.WriteLine($"An error occurred while adding the comment: {exception.Message}");            
                return RedirectToAction("UserDashboard");
            }
        }


        /// <summary>
        /// Display all the comments of particular post
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        public ActionResult DisplayComment(int postId)
        {
            var postDetails = userRepository.PostByPostId(postId);
            var postComments = userRepository.CommentsByPostId(postId);
            var viewModel = new ViewComments
            {
                PostDetails = postDetails,
                Comments = postComments
            };
            ViewBag.LoggedInUserId = Convert.ToInt32(Session["UserID"]);

            return View(viewModel);

        }


        /// <summary>
        /// Delete the comment added by the login user
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteComment(int commentId)
        {
            try
            {
                userRepository.DeleteComment(commentId);
                TempData["Message"] = "Comment deleted successfully!";
            }
            catch (Exception exception)
            {
                TempData["Message"] = "Error deleting comment. Please try again.";
            }
            return RedirectToAction("UserDashboard", "User");
        }


        [HttpPost]
        public JsonResult ToggleLikePost(int postId)
        {
            int userId = Convert.ToInt32(Session["UserID"]);

            // Call repository to toggle the like
            int updatedLikeCount = userRepository.ToggleLikePost(postId, userId);

            return Json(new { success = true, likeCount = updatedLikeCount });
        }


        [HttpPost]
        public JsonResult CheckLikeStatus(int postId)
        {
            int userId = Convert.ToInt32(Session["UserID"]);

            // Check if the user has liked the post
            bool hasLiked = userRepository.IsLikedPost(userId, postId);  // Check if the user has liked the post

            return Json(new { liked = hasLiked });
        }




    }
}