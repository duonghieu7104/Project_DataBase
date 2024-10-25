﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using win_project_2.SQLConn;
using win_project_2.Models;

namespace win_project_2.DAO
{
    public class JobDAO
    {
        private DatabaseConnection dbConn;

        public JobDAO()
        {
            dbConn = new DatabaseConnection();
        }

        // Lấy tất cả công việc từ view
        public List<Job> GetAllJobs()
        {
            List<Job> jobs = new List<Job>();
            using (SqlConnection conn = dbConn.GetConnection())
            {
                string query = "SELECT * FROM ViewAllJobs"; // Giả định bạn có view này
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Job job = new Job(
                        (int)reader["JobID"],
                        reader["Title"].ToString(),
                        reader["Description"].ToString(),
                        reader["Location"].ToString(),
                        reader["SkillRequire"].ToString(),
                        reader["Salary"].ToString(),
                        reader["Type"]?.ToString(),
                        reader["Company"]?.ToString(),
                        (DateTime)reader["PostedDate"],
                        reader["Status"]?.ToString(),
                        (int)reader["EmployerID"]
                    );
                    jobs.Add(job);
                }
            }
            return jobs;
        }

        // Thêm công việc sử dụng stored procedure
        public void AddJob(string title, string description, string location, string skillRequire, decimal salary, string type, string company, string status, int employer)
        {
            using (SqlConnection connection = dbConn.GetConnection())
            {
                SqlCommand command = new SqlCommand("sp_AddJob", connection);
                command.CommandType = CommandType.StoredProcedure;

                // Thêm tham số theo đúng định nghĩa của stored procedure
                command.Parameters.AddWithValue("@Title", title);
                command.Parameters.AddWithValue("@Description", description);
                command.Parameters.AddWithValue("@Location", location);
                command.Parameters.AddWithValue("@SkillRequire", skillRequire);
                command.Parameters.AddWithValue("@Salary", salary);
                command.Parameters.AddWithValue("@Type", type);
                command.Parameters.AddWithValue("@Company", company);
                command.Parameters.AddWithValue("@Status", status);
                command.Parameters.AddWithValue("@EmployerID", employer);

                connection.Open();
                command.ExecuteNonQuery(); // Thực thi stored procedure
            }
        }


        // Lấy công việc theo ID từ view
        public Job GetJobByID(int jobId)
        {
            Job job = null;

            using (SqlConnection connection = dbConn.GetConnection())
            {
                string query = "SELECT * FROM ViewJobDetails WHERE JobID = @JobID"; // Giả định bạn có view này
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@JobID", jobId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    job = new Job(
                        (int)reader["JobID"],
                        reader["Title"].ToString(),
                        reader["Description"].ToString(),
                        reader["Location"].ToString(),
                        reader["SkillRequire"].ToString(),
                        reader["Salary"].ToString(),
                        reader["Type"]?.ToString(),
                        reader["Company"]?.ToString(),
                        (DateTime)reader["PostedDate"],
                        reader["Status"]?.ToString(),
                        (int)reader["EmployerID"]
                    );
                }
            }

            return job;
        }

        // Cập nhật công việc sử dụng stored procedure
        public void UpdateJob(Job job)
        {
            using (SqlConnection connection = dbConn.GetConnection())
            {
                string query = "sp_UpdateJob"; // Tên stored procedure
                SqlCommand command = new SqlCommand(query, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@JobID", job.JobID);
                command.Parameters.AddWithValue("@Title", job.Title);
                command.Parameters.AddWithValue("@Description", job.Description);
                command.Parameters.AddWithValue("@Location", job.Location);
                command.Parameters.AddWithValue("@Salary", job.Salary);
                command.Parameters.AddWithValue("@Type", job.Type);
                command.Parameters.AddWithValue("@Company", job.Company);
                command.Parameters.AddWithValue("@Status", job.Status);
                command.Parameters.AddWithValue("@PostedDate", job.PostedDate);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // Xóa công việc sử dụng stored procedure
        public void DeleteJob(int jobId)
        {
            using (SqlConnection connection = dbConn.GetConnection())
            {
                string query = "sp_DeleteJob"; // Tên stored procedure
                SqlCommand command = new SqlCommand(query, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@JobID", jobId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // Tìm kiếm công việc
        public DataTable TimKiemJob(string timKiem)
        {
            using (SqlConnection connection = dbConn.GetConnection())
            {
                DataTable dt = new DataTable();
                string query = "SELECT * FROM ViewJobSearch WHERE Title LIKE '%' + @TimKiem + '%'"; // Giả định bạn có view này
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@TimKiem", timKiem);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                return dt;
            }
        }

        // Lấy danh sách địa điểm
        public DataTable HienThiLocation()
        {
            using (SqlConnection connection = dbConn.GetConnection())
            {
                DataTable dt = new DataTable();
                try
                {
                    string query = "SELECT DISTINCT Location FROM ViewAllJobs"; // Giả định bạn có view này
                    SqlCommand cmd = new SqlCommand(query, connection);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                return dt;
            }
        }

        // Lấy dữ liệu theo địa điểm
        public DataTable LayDuLieuLocation(string location)
        {
            using (SqlConnection connection = dbConn.GetConnection())
            {
                DataTable dt = new DataTable();
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM ViewJobsByLocation WHERE Location = @Location"; // Giả định bạn có view này
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Location", location);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                return dt;
            }
        }

        // Lấy dữ liệu công việc
        public DataTable DoDuLieuJob()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = dbConn.GetConnection())
            {
                string sql = "SELECT * FROM ViewAllJobs"; // Giả định bạn có view này
                SqlCommand cmd = new SqlCommand(sql, connection);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dataTable);
            }
            return dataTable;
        }
    }
}
