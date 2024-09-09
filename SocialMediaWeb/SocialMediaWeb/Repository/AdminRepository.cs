using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Mvc;
using SocialMediaWeb.Models;

namespace SocialMediaWeb.Repository
{
   
    public class AdminRepository
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["SocialMediaDBConnectionString"].ConnectionString;

        /// <summary>
        /// To see the reported post that report by users
        /// </summary>
        /// <returns></returns>
        public List<ReportedPostViewModel> GetReportedPosts()
        {
            var reportedPosts = new List<ReportedPostViewModel>();

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand("SPS_ReportedPosts", connection);
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        reportedPosts.Add(new ReportedPostViewModel
                        {
                            ReportId = (int)reader["reportId"],
                            PostId = (int)reader["postId"],
                            Content = reader["content"].ToString(),
                            ImageUrl = reader["imageUrl"] != DBNull.Value ? reader["imageUrl"].ToString() : null,
                            Reason = reader["reason"].ToString()
                        });
                    }
                }
            }

            return reportedPosts;
        }


        /// <summary>
        /// admin can remove the reported post
        /// </summary>
        /// <param name="postId"></param>
        public void DeleteReportedPost(int postId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand("SPD_ReportedPosts", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PostId", postId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

    }
}