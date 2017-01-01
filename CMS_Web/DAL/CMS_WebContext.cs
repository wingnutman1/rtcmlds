using System;
using CMS_Web.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;



namespace CMS_Web.DAL
{
    public class CMS_WebContext : DbContext 
    {

        public CMS_WebContext()
            : base("CMS_WebContext")
        {

        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Membership> Membership { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<OAuthMembership> OAuthMembership { get; set; }
        public DbSet<UsersInRole> UsersInRoles { get; set; }

       
        public DbSet<Client> Clients { get; set; }
        public DbSet<ActivityLogEntry> ActivityLog{ get; set; }
        public DbSet<ExceptionNote> ExceptionNotes { get; set; }
        public DbSet<ExceptionDetail> Exceptions { get; set; }
        public DbSet<ExceptionHistory> ExceptionHistory { get; set; }
        public DbSet<ExceptionEscallation> ExceptionEscallations { get; set; }
        public DbSet<ExceptionEscallationDetail> ExceptionEscallationDetails { get; set; }
        public DbSet<JournalEntry> JournalEntries { get; set; }
        public DbSet<JournalEntryType> JournalEntryTypes { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ToDoListItem> ToDoListItems { get; set; }
        public DbSet<ToDoListRecurringItem> ToDoListRecurringItems { get; set; }
        public DbSet<Incident> Incidents { get; set; }
        public DbSet<IncidentHistory> IncidentHistoryEntries { get; set; }
        public DbSet<IncidentType> IncidentTypes { get; set; }
        public DbSet<ClientConfigurationModel> ClientConfiguration { get; set; }
        public DbSet<SystemStatus> SystemStatusRecord { get; set; }
        public DbSet<OnCallCalendarItem> OnCallCalendarItems { get; set; }
        public DbSet<OnCallCalendar> OnCallCalendars { get; set; }
        public DbSet<StaffTeam> StaffTeams { get; set;  }
        public DbSet<StaffInTeam> StaffInTeams { get; set; }
        public DbSet<ClientPreferredStaff> ClientPreferredStaffItems { get; set; }
        public DbSet<ClientsAtSites> ClientsAtSitesList { get; set; }
        public DbSet<UserActivityLogEntry> UserActivityLog { get; set; }
        public DbSet<SystemLogEntry> SystemLog { get; set; }
        public DbSet<Qualification> Qualifications { get; set; }
        public DbSet<StaffQualification> StaffQualifications { get; set; }
        public DbSet<LocationRequiredQualification> LocationRequiredQualifications { get; set; }
        public DbSet<ClientRequiredQualification> ClientRequiredQualifications { get; set; }
        public DbSet<RosterModel> Rosters { get; set; }
        public DbSet<RosterImportErrorDetail> RosterImportErrors { get; set; }
        public DbSet<GlobalSettingsModel> GlobalSettings { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}