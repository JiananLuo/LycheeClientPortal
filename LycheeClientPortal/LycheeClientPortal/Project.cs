using System;
using System.Collections.Generic;
using System.Text;

namespace LycheeClientPortal
{
    public enum ProjectStage
    {
        STAGE_ONTRACK,
        STAGE_FAIR,
        STAGE_DANGER
    }
    public enum ProjectStatus
    {
        STATUS_ACTIVE,
        STATUS_INACTIVE,
        STATUS_LOCKED
    }

    public class Project
    {
        public string projectName { get; set; }

        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }

        public ProjectStage projectStage { get; set; }
        public ProjectStatus projectStatus;

        public List<long> Users { get; set; }
        public List<Invoice> invoices { get; set; }
        public Project(string projectName,
                DateTime startDate,
                DateTime endDate,
                ProjectStage projectStage,
                ProjectStatus projectStatus)
        {
            this.projectName = projectName;
            this.startDate = startDate;
            this.endDate = endDate;
            this.projectStage = projectStage;
            this.projectStatus = projectStatus;
        }
    }
}