using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using SocialMediaWeb.Models;
using System.Web.Mvc;
using System.Collections.Generic;

namespace SocialMediaWeb.Repository
{
    public class UserRepository
    {
        private string connectionString;

        public UserRepository()
        {
            try
            {
                connectionString = ConfigurationManager.ConnectionStrings["SocialMediaDBConnectionString"].ConnectionString;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        /// <summary>
        /// get all the details of user to display
        /// </summary>
        /// <param name="userId">  get particular user </param>
        /// <returns></returns>
        public ProfileViewModel GetUserDetails(int userId)
        {
            ProfileViewModel userProfile = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPS_UserDetails", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", userId);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userProfile = new ProfileViewModel
                            {
                                UserID = Convert.ToInt32(reader["userID"]),
                                Email = reader["email"].ToString(),
                                FirstName = reader["firstName"].ToString(),
                                LastName = reader["lastName"].ToString(),
                                isActive = Convert.ToInt32(reader["isActive"]),
                                DateOfBirth = Convert.ToDateTime(reader["dateOfBirth"]),
                                Gender = reader["gender"].ToString(),
                                ProfilePicture = reader["profilePicture"].ToString() ,
                                
                                PhoneNumber = reader["phoneNumber"].ToString(),
                                Address = reader["address"].ToString(),
                                StateName = reader["StateName"].ToString(),
                                DistrictName = reader["DistrictName"].ToString()
                            };
                        }
                    }
                }
            }

            return userProfile;
        }


        /// <summary>
        /// For the EditProfile view . get the details of corresponding user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns> returns the details fetched as UpdateProfileViewModel object </returns>
        public UpdateProfileViewModel GetUserDetailsById(int userId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand("SPS_UserDetails_ById", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@UserID", userId);

                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new UpdateProfileViewModel
                    {
                        Email = reader["email"].ToString(),
                        FirstName = reader["firstName"].ToString(),
                        LastName = reader["lastName"].ToString(),
                        DateOfBirth = Convert.ToDateTime(reader["dateOfBirth"]),
                        Gender = reader["gender"].ToString(),
                        ProfilePicture = reader["profilePicture"].ToString(),
                        PhoneNumber = reader["phoneNumber"].ToString(),
                        Address = reader["address"].ToString(),
                        StateID = Convert.ToInt32(reader["StateID"]),
                        DistrictID = Convert.ToInt32(reader["DistrictID"])
                    };
                }
                return null;
            }
        }


        /// <summary>
        /// post the data to database for update user
        /// </summary>
        /// <param name="model"> remaning user details</param>
        /// <param name="profilePicturePath">profile picture path </param>
        public void UpdateUserProfile(Signup signup, string profilePicturePath ,int UserID)
        {

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand("SPU_UpdateUserProfile", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@UserID", UserID);
                command.Parameters.AddWithValue("@Email", signup.Email);
                command.Parameters.AddWithValue("@FirstName", signup.FirstName);
                command.Parameters.AddWithValue("@LastName", signup.LastName);
                command.Parameters.AddWithValue("@DateOfBirth", signup.DateOfBirth);
                command.Parameters.AddWithValue("@Gender", signup.Gender);
                command.Parameters.AddWithValue("@ProfilePicture", profilePicturePath);
                command.Parameters.AddWithValue("@PhoneNumber", signup.PhoneNumber);
                command.Parameters.AddWithValue("@Address", signup.Address);
                command.Parameters.AddWithValue("@StateID", signup.StateID);
                command.Parameters.AddWithValue("@DistrictID", signup.DistrictID);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// for changing the password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="newPassword"></param>
        public void UpdatePassword(string email, string newPassword)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand("SPU_Password", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@NewPassword", newPassword);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }


        /// <summary>
        /// add post
        /// </summary>
        /// <param name="post"> consist of the post details</param>
        public void AddPost(Post post)
        {
                using (var connection = new SqlConnection(connectionString))
                {
                    using (var command = new SqlCommand("SPI_Post", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@userId", post.UserId);
                         command.Parameters.AddWithValue("@content", post.Content);
                         command.Parameters.AddWithValue("@imageUrl", post.ImageUrl);
                         command.Parameters.AddWithValue("@createdAt", post.CreatedAt);
                        command.Parameters.AddWithValue("@imageType", post.ImageType);

                    connection.Open();
                    command.ExecuteNonQuery();
                    }
                }   
        }


        /// <summary>
        /// retrive all the post
        /// </summary>
        /// <returns></returns>
        public List<PostDisplayViewModel> GetAllPost()
        {
            List<PostDisplayViewModel> posts = new List<PostDisplayViewModel>();

                using (var connection = new SqlConnection(connectionString))
                {
                    using (var command = new SqlCommand("SPS_Post", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var post = new PostDisplayViewModel
                                {
                                    PostId = Convert.ToInt32(reader["postId"]),
                                    UserId = Convert.ToInt32(reader["userId"]),
                                    Email = reader["email"].ToString(),
                                    FirstName = reader["firstName"].ToString(),
                                    LastName = reader["lastName"].ToString(),
                                    ProfilePicture = reader["profilePicture"].ToString(),
                                    Content = reader["content"].ToString(),
                                    ImageUrl = reader["imageUrl"] != DBNull.Value ? reader["imageUrl"].ToString() : null,
                                   
                                    CreatedAt = Convert.ToDateTime(reader["createdAt"]),
                                    LikeCount = Convert.ToInt32(reader["likeCount"])
                                };

                                posts.Add(post);
                            }
                        }
                    }
                
            }

            return posts;
        }


        /// <summary>
        /// get the posts added by logged in user
        /// </summary>
        /// <param name="userId">login user id</param>
        /// <returns></returns>
        public List<Post> GetPostsByUserId(int userId)
        {
            List<Post> posts = new List<Post>();


                using (var connection = new SqlConnection(connectionString))
                {
                    using (var cmd = new SqlCommand("SPS_GetPostsByUserId", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@userId", userId);

                        connection.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                posts.Add(new Post
                                {
                                    PostId = (int)reader["postId"],
                                    UserId = (int)reader["userId"],
                                    Content = reader["content"].ToString(),
                                    ImageUrl = reader["imageUrl"].ToString(),
                                    CreatedAt = (DateTime)reader["createdAt"]
                                });
                            }
                        }
                    }
                }

            return posts;
        }

        /// <summary>
        /// For deleting the post added 
        /// </summary>
        /// <param name="postId"></param>
        public void DeletePost(int postId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand("SPD_Post", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@postId", postId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        ///  get a particular post . while clicking on report post.
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        public Post GetPostById(int postId)
        {
            Post post = null;
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand("SPS_PostById", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PostId", postId);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        post = new Post
                        {
                            PostId = (int)reader["postId"],
                            Content = reader["content"].ToString(),
                            ImageUrl = reader["imageUrl"].ToString()
                        };
                    }
                }
            }
            return post;
        }

        /// <summary>
        /// store the data that a user is reported 
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="reason"></param>
        /// <param name="reportedBy"></param>
        public void SaveReport(int postId, string reason, int reportedBy)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand("SPI_ReportPost", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PostId", postId);
                command.Parameters.AddWithValue("@Reason", reason);
                command.Parameters.AddWithValue("@ReportedBy", reportedBy);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }


        /// <summary>
        /// search for a particular user profile
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public List<ProfileSearchResultViewModel> SearchProfiles(string searchTerm)
        {
            var profiles = new List<ProfileSearchResultViewModel>();
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand("SPS_SearchProfiles", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SearchTerm", searchTerm);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        profiles.Add(new ProfileSearchResultViewModel
                        {
                            UserID = Convert.ToInt32(reader["userID"]),
                            FirstName = reader["firstName"].ToString(),
                            LastName = reader["lastName"].ToString(),
                            ProfilePicture = reader["profilePicture"] .ToString(),
                        });
                    }
                }
            }
            return profiles;
        }



        /// <summary>
        /// to check the logged in user follow the other user
        /// </summary>
        /// <param name="loggedInUserId"></param>
        /// <param name="profileUserId"></param>
        /// <returns></returns>
        public bool IsUserFollowing(int loggedInUserId, int profileUserId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand("SPS_IsUserFollowing", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@LoggedInUserId", loggedInUserId);
                    command.Parameters.AddWithValue("@ProfileUserId", profileUserId);

                    connection.Open();
                    var result = (int)command.ExecuteScalar();
                    return result == 1;
                }
            }
        }

        /// <summary>
        /// get the count of followers of other users
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetFollowersCount(int userId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand("SPS_GetFollowersCount", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserId", userId);

                    connection.Open();
                    return (int)command.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// get the count of following of other users
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetFollowingCount(int userId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand("SPS_GetFollowingCount", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserId", userId);

                    connection.Open();
                    return (int)command.ExecuteScalar();
                }
            }
        }


        /// <summary>
        /// to follow a particular user
        /// </summary>
        /// <param name="followerId"></param>
        /// <param name="followedId"></param>
        public void FollowUser(int followerId, int followedId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand("SPI_FollowUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FollowerId", followerId);
                    command.Parameters.AddWithValue("@FollowedId", followedId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// to unfollow a user
        /// </summary>
        /// <param name="followerId"></param>
        /// <param name="followedId"></param>
        public void UnfollowUser(int followerId, int followedId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand("SPD_UnfollowUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FollowerId", followerId);
                    command.Parameters.AddWithValue("@FollowedId", followedId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }


        /// <summary>
        /// For adding comment to a particular post
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="userId"></param>
        /// <param name="commentText"></param>
        public void AddComment(int postId, int userId, string commentText)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SPI_Comment", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PostId", postId);
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@CommentText", commentText);
                    command.ExecuteNonQuery();
                }
            }
        }
        /// <summary>
        /// Get the profile and post details of user with particular postId
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        public CommentProfile PostByPostId(int postId) {
            CommentProfile commentProfile = null;
            using(var connection = new SqlConnection(connectionString)) 
            {
                var command = new SqlCommand("SPS_PostByPostId", connection);
                command.CommandType= CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PostId", postId);
                connection.Open();
                using (var reader = command.ExecuteReader()) {
                    if (reader.Read()) {
                        commentProfile = new CommentProfile
                        {
                            PostId = (int)reader["postId"],
                            PostContent = reader["PostContent"].ToString(),
                            PostImageUrl = reader["PostImageUrl"].ToString(),
                            Firstname = reader["firstName"].ToString(),
                            Lastname = reader["lastName"].ToString(),
                            ProfilePicture = reader["profilePicture"].ToString()
                        };       
                    }              
                }         
            }
            return commentProfile;

        }


        /// <summary>
        /// Get all the comments and details of who posted comment of a particular post
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        public List<CommentData> CommentsByPostId(int postId) 
        {
            List<CommentData> commentData = new List<CommentData>();
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand("SPS_CommentsByPostId",connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PostId",postId);
                    connection.Open();

                    using(var reader  = command.ExecuteReader())
                    {
                        while (reader.Read()) {
                            commentData.Add(new CommentData
                            {
                                CommentId = (int)reader["commentId"],
                                PostId    = (int)reader["postId"],
                                UserId = (int)reader["userId"],
                                CommentText = reader["commentText"].ToString(),
                                CommentedAt = (DateTime)reader["commentedAt"],
                                FirstName = reader["firstName"].ToString(),
                                LastName = reader["lastName"].ToString(),
                                ProfilePicture= reader["profilePicture"].ToString(),
                            });                       
                        }
                    }
                }
            }
            return commentData;        
        }



        /// <summary>
        /// Delete the comment 
        /// </summary>
        /// <param name="commentId"></param>
        public void DeleteComment(int commentId)
        {
            using(var connection = new SqlConnection(connectionString))
            {
                using(var command = new SqlCommand("SPD_Comment", connection))
                {
                    command.CommandType= CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CommentId", commentId);
                    connection.Open();
                    command.ExecuteNonQuery();
                }

            }
        }


        public int ToggleLikePost(int postId, int userId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand("SPU_ToggleLike", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PostId", postId);
                command.Parameters.AddWithValue("@UserId", userId);

                connection.Open();
                var likeCount = (int)command.ExecuteScalar(); 
                return likeCount;
            }
        }



        public bool IsLikedPost(int userId, int postId)
        {
            bool hasLiked = false;

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand("SPS_HasUserLikedPost", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PostId", postId);
                command.Parameters.AddWithValue("@UserId", userId);

                connection.Open();
                var result = command.ExecuteScalar();

                if (result != null && Convert.ToInt32(result) == 1)
                {
                    hasLiked = true;
                }
            }

            return hasLiked;
        }


        public void InsertContactUs(ContactUs model)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand("SPI_ContactUs", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FirstName", model.FirstName);
                    command.Parameters.AddWithValue("@LastName", model.LastName);
                    command.Parameters.AddWithValue("@Email", model.Email);
                    command.Parameters.AddWithValue("@Description", model.Description);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }


        public void AddNotification(int postId, string message , string messageTitle)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SPI_Notification", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@postId", postId);
                        command.Parameters.AddWithValue("@message", message);
                        command.Parameters.AddWithValue("@messageTitle", messageTitle);
                        command.ExecuteNonQuery();
                    }   
                
            }
        }

        public List<Notification> GetNotifications(int userId)
        {
            List<Notification> notifications = new List<Notification>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SPS_Notification", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserId", userId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                notifications.Add(new Notification
                                {
                                    NotificationId=(int)reader["notificationId"],
                                    Message = reader["message"].ToString(),
                                    messageTitle = reader["messageTitle"].ToString(),
                                    CreatedAt = Convert.ToDateTime(reader["createdAt"]),
                                    ImageUrl = reader["imageUrl"].ToString()
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return notifications;
        }



        public void UpdateViewStatus(int notificationId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand("SPU_Notification", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@NotificationId", notificationId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }










    }
}
