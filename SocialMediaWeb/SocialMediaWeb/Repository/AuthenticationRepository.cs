using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Mvc;
using SocialMediaWeb.Models;


namespace SocialMediaWeb.Repository
{
    public class AuthenticationRepository
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["SocialMediaDBConnectionString"].ConnectionString;


        /// <summary>
        /// Get the state details from database
        /// </summary>
        /// <returns>  list of states in dropdown </returns>
        public List<SelectListItem> GetStates()
        {
            var states = new List<SelectListItem>();
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand("GetStates", connection) { CommandType = CommandType.StoredProcedure };
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        states.Add(new SelectListItem
                        {
                            Value = reader["StateID"].ToString(),
                            Text = reader["StateName"].ToString()
                        });
                    }
                }
            }
            return states;
        }


        /// <summary>
        /// get the district based on the state
        /// </summary>
        /// <param name="stateId">  only select the districts of particular state</param>
        /// <returns>  list of district in dropdown </returns>
        public List<SelectListItem> GetDistricts(int stateId)
        {
            var districts = new List<SelectListItem>();
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand("GetDistricts", connection) { CommandType = CommandType.StoredProcedure };
                command.Parameters.AddWithValue("@StateID", stateId);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        districts.Add(new SelectListItem
                        {
                            Value = reader["DistrictID"].ToString(),
                            Text = reader["DistrictName"].ToString()
                        });
                    }
                }
            }
            return districts;
        }


        /// <summary>
        /// Add the signup details in database
        /// </summary>
        /// <param name="user"> user details (email , password )</param>
        /// <param name="profile"> remaning details </param>
        public void AddUser(Users user, Profile profile)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    using (var command = new SqlCommand("SPI_User", connection) { CommandType = CommandType.StoredProcedure })
                    {
                        command.Parameters.AddWithValue("@email", user.Email);
                        command.Parameters.AddWithValue("@userPassword", user.UserPassword);
                        command.Parameters.AddWithValue("@role", "User");
                        connection.Open();
                        var userId = Convert.ToInt32(command.ExecuteScalar());

                        command.CommandText = "SPI_Profile";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@userID", userId);
                        command.Parameters.AddWithValue("@firstName", profile.FirstName);
                        command.Parameters.AddWithValue("@lastName", profile.LastName);
                        command.Parameters.AddWithValue("@dateOfBirth", profile.DateOfBirth);
                        command.Parameters.AddWithValue("@gender", profile.Gender);
                        command.Parameters.AddWithValue("@profilePicture", profile.ProfilePicture);
                        command.Parameters.AddWithValue("@phoneNumber", profile.PhoneNumber);
                        command.Parameters.AddWithValue("@address", profile.Address);
                        command.Parameters.AddWithValue("@StateID", profile.StateID);
                        command.Parameters.AddWithValue("@DistrictID", profile.DistrictID);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
              
            }
            
        }



        /// <summary>
        /// for login the verified user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User AuthenticateUser(string email, string password)
        {
            User user = null;

            using (var con = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand("SPS_Login", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", email);
                   

                    con.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string hashedPassword = reader["userPassword"].ToString();
                            if (BCrypt.Net.BCrypt.Verify(password, hashedPassword))
                            {
                                user = new User
                                {
                                    UserID = Convert.ToInt32(reader["userID"]),
                                    Email = reader["email"].ToString(),
                                    Role = reader["role"].ToString()
                                };
                            }
                        }
                    }
                }
            }

            return user;
        }
    }
}
