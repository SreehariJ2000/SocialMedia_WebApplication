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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
        /// get the post added by logged in user
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
        /// For deleting the post added by 
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







    }
}
