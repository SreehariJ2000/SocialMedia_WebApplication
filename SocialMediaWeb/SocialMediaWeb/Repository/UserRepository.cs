using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using SocialMediaWeb.Models;
using System.Web.Mvc;

namespace SocialMediaWeb.Repository
{
    public class UserRepository
    {
        private string connectionString;

        public UserRepository()
        {
            connectionString = ConfigurationManager.ConnectionStrings["SocialMediaDBConnectionString"].ConnectionString;
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
                                ProfilePicture = reader["profilePicture"].ToString(),
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
        public void UpdateUserProfile(SignupViewModel model, string profilePicturePath ,int UserID)
        {

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand("SPU_UpdateUserProfile", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@UserID", UserID);
                command.Parameters.AddWithValue("@Email", model.Email);
                command.Parameters.AddWithValue("@FirstName", model.FirstName);
                command.Parameters.AddWithValue("@LastName", model.LastName);
                command.Parameters.AddWithValue("@DateOfBirth", model.DateOfBirth);
                command.Parameters.AddWithValue("@Gender", model.Gender);
                command.Parameters.AddWithValue("@ProfilePicture", profilePicturePath);
                command.Parameters.AddWithValue("@PhoneNumber", model.PhoneNumber);
                command.Parameters.AddWithValue("@Address", model.Address);
                command.Parameters.AddWithValue("@StateID", model.StateID);
                command.Parameters.AddWithValue("@DistrictID", model.DistrictID);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
