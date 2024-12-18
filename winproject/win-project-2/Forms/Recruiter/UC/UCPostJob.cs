﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Xml.Linq;
using System.Web.Security;
using win_project_2.DAO;
using win_project_2.Service;

namespace win_project_2.Forms.Recruiter.UC
{
    public partial class UCPostJob : UserControl
    {
        public UCPostJob()
        {
            InitializeComponent();
        }
        private void btn_Post_Click(object sender, EventArgs e)
        {
            int employer = UserDangNhap.userId;
            string title = txt_title.Text;
            string description = txt_des.Text;
            string location = txt_locaton.Text;
            string skillRequire = txt_skill.Text;
            DateTime postedDate = DateTime.Now;
            decimal salary = decimal.Parse(txt_salary.Text);
            string type = txt_Type.Text;
            string company = txt_company.Text;

            // Kiểm tra xem các trường có bị bỏ trống hoặc không hợp lệ
            if (string.IsNullOrWhiteSpace(title) ||
                string.IsNullOrWhiteSpace(description) ||
                string.IsNullOrWhiteSpace(location) ||
                string.IsNullOrWhiteSpace(skillRequire) ||
                !decimal.TryParse(txt_salary.Text, out salary) ||
                string.IsNullOrWhiteSpace(company))
            {
                lbError.Visible = true;
                lbError.Text = "Vui lòng điền đầy đủ thông tin!";
                return;
            }

            // Kiểm tra trạng thái công việc
            string status = "";
            if (rbOpen.Checked) { status = "Open"; }
            else if (rbClosed.Checked) { status = "Closed"; }
            else if (rbPaused.Checked) { status = "Paused"; }

            try
            {
                JobDAO jobDAO = new JobDAO();
                jobDAO.AddJob(title, description, location, skillRequire, salary, type, company, status, employer);

                MessageBox.Show("Thêm công việc thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                lbError.Visible = true;
                lbError.Text = $"Đã xảy ra lỗi: {ex.Message}";
            }
        }
    }
}
