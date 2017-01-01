using System;

namespace CMS_Web.Models
{
    
    public class Incident
    {
        public int ID { get; set; }
        public int TypeID { get; set; }
        
        public incidentState currentState { get; set; }
        public DateTime? currentActionByDate { get; set; }

        public int? LocationID { get; set; }
        public Location Location { get; set; }

        public int? ClientID { get; set; }
        public Client Client { get; set; }

        public int? UserProfileID { get; set; }
        public UserProfile UserProfile { get; set; }

        public string Description { get; set; }
        public string CurrentAction { get; set; }
        public DateTime incidentDate { get; set; }
        public DateTime reportedDate { get; set; }
        public int CurrentManagerID { get; set; }
        public int StaffReportedID { get; set; }

    }
}