﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using win_project_2.DAO;
using win_project_2.Service;
using win_project_2.UserControls;

namespace win_project_2.Forms.UC
{
    public partial class UCApplication : UserControl
    {
        public UCApplication()
        {
            InitializeComponent();
        }
        ApplicationDAO application = new ApplicationDAO();

        public void load()
        {
            fpanelHienThi.Controls.Clear();

            DataTable dt = application.DoDuLieuApplication(UserDangNhap.userId);
            foreach (DataRow row in dt.Rows)
            {
                UCComponents.Application application = new UCComponents.Application();
                int applicationID = (int)row["ApplicationID"];
                int jobid = (int)row["JobID"];
                string title = row["ApplicationTitle"].ToString();
                string date = row["ApplicationDate"].ToString();
                string status = row["Status"].ToString();
                
                application.themThongTin(applicationID,jobid,title,date,status);
                fpanelHienThi.Controls.Add(application);
                application.BringToFront();
            }


        }
        private void UCApplication_Load(object sender, EventArgs e)
        {
            load();
        }
    }
}
