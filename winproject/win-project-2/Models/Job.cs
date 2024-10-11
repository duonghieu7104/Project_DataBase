﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace win_project_2.DAO
{
    public class Job
    {
        public int JobID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime PostedDate { get; set; }
        public string Status { get; set; }

        public Job(int jobID, string title, string description, string location, DateTime postedDate, string status)
        {
            JobID = jobID;
            Title = title;
            Description = description;
            Location = location;
            PostedDate = postedDate;
            Status = status;
        }
    }

}
