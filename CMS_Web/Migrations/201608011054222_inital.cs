namespace CMS_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inital : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActivityLogEntry",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Type = c.String(),
                        Details = c.String(),
                        RelatedTableName = c.String(),
                        RelatedRecordID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserProfile", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        SignalRID = c.String(),
                        UserName = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Address1 = c.String(),
                        Address2 = c.String(),
                        Suburb = c.String(),
                        State = c.String(),
                        Postcode = c.String(),
                        HomePhone = c.String(),
                        PrimaryMobilePhone = c.String(),
                        SecondaryMobilePhone = c.String(),
                        PrimaryEmail = c.String(),
                        SecondaryEmail = c.String(),
                        Inactive = c.Boolean(),
                        Note = c.String(),
                        IMEI_Number = c.String(),
                        LoggedOn = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Client",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Address1 = c.String(),
                        Address2 = c.String(),
                        Suburb = c.String(),
                        State = c.String(),
                        Postcode = c.String(),
                        HomePhone = c.String(),
                        PrimaryMobilePhone = c.String(),
                        SecondaryMobilePhone = c.String(),
                        PrimaryEmail = c.String(),
                        SecondaryEmail = c.String(),
                        SiteID = c.Int(),
                        Inactive = c.Boolean(nullable: false),
                        Note = c.String(),
                        IMEI_Number = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ExceptionEscallationDetail",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ParentID = c.Int(nullable: false),
                        Position = c.Int(nullable: false),
                        NextStaffID = c.Int(nullable: false),
                        DelayBeforeEscalateToNextStaff = c.Time(nullable: false, precision: 7),
                        EscallationDetailDescription = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ExceptionEscallation",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ExceptionHistory",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ParentID = c.Int(nullable: false),
                        ActionStaffID = c.Int(nullable: false),
                        ActionDate = c.DateTime(nullable: false),
                        ActionDescription = c.String(),
                        EscallationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ExceptionNote",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ParentID = c.Int(nullable: false),
                        CreationStaffID = c.Int(nullable: false),
                        NoteDate = c.DateTime(nullable: false),
                        NoteText = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ExceptionDetail",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                        CurrentActionByDate = c.DateTime(nullable: false),
                        CurrentActionByStaff = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        ExceptionType = c.Int(nullable: false),
                        IncidentID = c.Int(),
                        EscalationParentID = c.Int(),
                        EscalationChildID = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.IncidentHistory",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        incidentID = c.Int(nullable: false),
                        creationDate = c.DateTime(nullable: false),
                        description = c.String(),
                        staffID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Incident",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        LocationID = c.Int(),
                        ClientID = c.Int(),
                        StaffID = c.Int(),
                        Description = c.String(),
                        CurrentAction = c.String(),
                        creationDate = c.DateTime(nullable: false),
                        ManagerID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.IncidentType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        firstStaffEsc = c.Int(),
                        firstTimeToEsc = c.Time(precision: 7),
                        secondStaffEsc = c.Int(),
                        secondTimeToEsc = c.Time(precision: 7),
                        thirdStaffEsc = c.Int(),
                        thirdTimeToEsc = c.Time(precision: 7),
                        fourthStaffEsc = c.Int(),
                        fourthTimeToEsc = c.Time(precision: 7),
                        fifthStaffEsc = c.Int(),
                        fifthTimeToEsc = c.Time(precision: 7),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.JournalEntry",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LocationID = c.Int(),
                        ClientID = c.Int(),
                        TypeID = c.Int(),
                        StaffID = c.Int(),
                        lastActionStaffID = c.Int(),
                        lastActionDate = c.DateTime(nullable: false),
                        Note = c.String(),
                        creationDate = c.DateTime(nullable: false),
                        JournalEntryType_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.JournalEntryType", t => t.JournalEntryType_ID)
                .ForeignKey("dbo.Location", t => t.LocationID)
                .Index(t => t.LocationID)
                .Index(t => t.JournalEntryType_ID);
            
            CreateTable(
                "dbo.JournalEntryType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Location",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address1 = c.String(),
                        Address2 = c.String(),
                        Suburb = c.String(),
                        State = c.String(),
                        Postcode = c.String(),
                        Phone = c.String(),
                        Comment = c.String(),
                        Note = c.String(),
                        Latitude = c.String(),
                        Longtitude = c.String(),
                        Geofence_Radius = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.webpages_Membership",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        CreateDate = c.DateTime(),
                        ConfirmationToken = c.String(maxLength: 128),
                        IsConfirmed = c.Boolean(),
                        LastPasswordFailureDate = c.DateTime(),
                        PasswordFailuresSinceLastSuccess = c.Int(nullable: false),
                        Password = c.String(nullable: false, maxLength: 128),
                        PasswordChangedDate = c.DateTime(),
                        PasswordSalt = c.String(nullable: false, maxLength: 128),
                        PasswordVerificationToken = c.String(maxLength: 128),
                        PasswordVerificationTokenExpirationDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.webpages_OAuthMembership",
                c => new
                    {
                        Provider = c.String(nullable: false, maxLength: 30),
                        ProviderUserId = c.String(nullable: false, maxLength: 100),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Provider, t.ProviderUserId })
                .ForeignKey("dbo.webpages_Membership", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.webpages_UsersInRoles",
                c => new
                    {
                        RoleId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoleId, t.UserId })
                .ForeignKey("dbo.webpages_Membership", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.webpages_Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.webpages_Roles",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        RoleName = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.Message",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SenderStaff = c.Int(nullable: false),
                        RecipientStaff = c.Int(nullable: false),
                        SendDate = c.DateTime(nullable: false),
                        ReadDate = c.DateTime(),
                        MessageText = c.String(),
                        MessageRead = c.Boolean(nullable: false),
                        TimeToRead = c.Int(),
                        ReadTimeOut = c.Boolean(nullable: false),
                        MessageDeleted = c.Boolean(nullable: false),
                        MessageDeleteDate = c.DateTime(),
                        UserProfile_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserProfile", t => t.UserProfile_UserId)
                .Index(t => t.UserProfile_UserId);
            
            CreateTable(
                "dbo.ToDoListItem",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StaffID = c.Int(nullable: false),
                        Description = c.String(),
                        Complete = c.Boolean(nullable: false),
                        BumpedOut = c.Boolean(nullable: false),
                        CompletedDate = c.DateTime(),
                        CompletedNote = c.String(),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(),
                        BumpOutDate = c.DateTime(),
                        BumpOutNote = c.String(),
                        RelatedLocationID = c.Int(),
                        RelatedClientID = c.Int(),
                        BumpInDate = c.DateTime(),
                        BumpInNote = c.String(),
                        BumpInID = c.Int(),
                        BumpOutWithoutAuthorisation = c.Boolean(nullable: false),
                        AuthorisingStaffID = c.Int(),
                        AwaitingAuthorisation = c.Boolean(nullable: false),
                        RequiredCompletionBy = c.DateTime(nullable: false),
                        RelatedTo = c.String(),
                        UserProfile_UserId = c.Int(),
                        UserProfile1_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserProfile", t => t.UserProfile_UserId)
                .ForeignKey("dbo.UserProfile", t => t.UserProfile1_UserId)
                .Index(t => t.UserProfile_UserId)
                .Index(t => t.UserProfile1_UserId);
            
            CreateTable(
                "dbo.ToDoListRecurringItem",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SiteID = c.Int(),
                        ClientID = c.Int(),
                        StaffID = c.Int(),
                        AutoGenerateEvent = c.String(),
                        Task = c.String(),
                        TimeToComplete = c.DateTime(),
                        DayOfWeek = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ToDoListItem", "UserProfile1_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.ToDoListItem", "UserProfile_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Message", "UserProfile_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.webpages_UsersInRoles", "RoleId", "dbo.webpages_Roles");
            DropForeignKey("dbo.webpages_UsersInRoles", "UserId", "dbo.webpages_Membership");
            DropForeignKey("dbo.webpages_OAuthMembership", "UserId", "dbo.webpages_Membership");
            DropForeignKey("dbo.JournalEntry", "LocationID", "dbo.Location");
            DropForeignKey("dbo.JournalEntry", "JournalEntryType_ID", "dbo.JournalEntryType");
            DropForeignKey("dbo.ActivityLogEntry", "UserID", "dbo.UserProfile");
            DropIndex("dbo.ToDoListItem", new[] { "UserProfile1_UserId" });
            DropIndex("dbo.ToDoListItem", new[] { "UserProfile_UserId" });
            DropIndex("dbo.Message", new[] { "UserProfile_UserId" });
            DropIndex("dbo.webpages_UsersInRoles", new[] { "UserId" });
            DropIndex("dbo.webpages_UsersInRoles", new[] { "RoleId" });
            DropIndex("dbo.webpages_OAuthMembership", new[] { "UserId" });
            DropIndex("dbo.JournalEntry", new[] { "JournalEntryType_ID" });
            DropIndex("dbo.JournalEntry", new[] { "LocationID" });
            DropIndex("dbo.ActivityLogEntry", new[] { "UserID" });
            DropTable("dbo.ToDoListRecurringItem");
            DropTable("dbo.ToDoListItem");
            DropTable("dbo.Message");
            DropTable("dbo.webpages_Roles");
            DropTable("dbo.webpages_UsersInRoles");
            DropTable("dbo.webpages_OAuthMembership");
            DropTable("dbo.webpages_Membership");
            DropTable("dbo.Location");
            DropTable("dbo.JournalEntryType");
            DropTable("dbo.JournalEntry");
            DropTable("dbo.IncidentType");
            DropTable("dbo.Incident");
            DropTable("dbo.IncidentHistory");
            DropTable("dbo.ExceptionDetail");
            DropTable("dbo.ExceptionNote");
            DropTable("dbo.ExceptionHistory");
            DropTable("dbo.ExceptionEscallation");
            DropTable("dbo.ExceptionEscallationDetail");
            DropTable("dbo.Client");
            DropTable("dbo.UserProfile");
            DropTable("dbo.ActivityLogEntry");
        }
    }
}
