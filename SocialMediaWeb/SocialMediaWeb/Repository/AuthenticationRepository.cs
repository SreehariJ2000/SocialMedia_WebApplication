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
    }
}
