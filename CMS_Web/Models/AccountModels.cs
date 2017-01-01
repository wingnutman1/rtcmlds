using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace CMS_Web.Models
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("CMS_WebContext")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }

        #region Added Code

        public DbSet<Membership> Membership { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<OAuthMembership> OAuthMembership { get; set; }
        public DbSet<UsersInRole> UsersInRoles { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Membership>()
        //      .HasMany<Role>(r => r.UsersInRoles)
        //      .WithMany(u => u.UsersInRoles)
        //      .Map(m =>
        //      {
        //          m.ToTable("webpages_UsersInRoles");
        //          m.MapLeftKey("UserId");
        //          m.MapRightKey("RoleId"); 
        //      });
        //}

        #endregion
    }

    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string SignalRID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return string.Format("{0} {1}", FirstName, LastName); } }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Suburb { get; set; }
        public string State { get; set; }
        public string Postcode { get; set; }
        public string HomePhone { get; set; }
        public string PrimaryMobilePhone { get; set; }
        public string SecondaryMobilePhone { get; set; }
        public string PrimaryEmail { get; set; }
        public string SecondaryEmail { get; set; }
        public bool? Inactive { get; set; }
        public string Note { get; set; }
        public string IMEI_Number { get; set; }
        public bool? LoggedOn { get; set; }
        public string PositionDescription { get; set; }
        public int? reportsTo { get; set; }
        public string currentLocation { get; set; }
        public int currentLocationID { get; set; }
        public int? delegateID { get; set; }
        public bool? available { get; set; }
        public string availablityStatus { get; set; }
        public DateTime? lastOnlineEventTime { get; set; }
        public DateTime? lastOfflineEventTime { get; set; }



    }

    #region Added Code

    [Table("webpages_Membership")]
    public class Membership
    {
        public Membership()
        {
            Roles = new List<Role>();
            OAuthMemberships = new List<OAuthMembership>();
            UsersInRoles = new List<UsersInRole>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }
        public DateTime? CreateDate { get; set; }
        [StringLength(128)]
        public string ConfirmationToken { get; set; }
        public bool? IsConfirmed { get; set; }
        public DateTime? LastPasswordFailureDate { get; set; }
        public int PasswordFailuresSinceLastSuccess { get; set; }
        [Required, StringLength(128)]
        public string Password { get; set; }
        public DateTime? PasswordChangedDate { get; set; }
        [Required, StringLength(128)]
        public string PasswordSalt { get; set; }
        [StringLength(128)]
        public string PasswordVerificationToken { get; set; }
        public DateTime? PasswordVerificationTokenExpirationDate { get; set; }

        public ICollection<Role> Roles { get; set; }

        [ForeignKey("UserId")]
        public ICollection<OAuthMembership> OAuthMemberships { get; set; }
        
        [ForeignKey("UserId")]
        public ICollection<UsersInRole> UsersInRoles { get; set; }
    }

    [Table("webpages_OAuthMembership")]
    public class OAuthMembership
    {
        [Key, Column(Order = 0), StringLength(30)]
        public string Provider { get; set; }

        [Key, Column(Order = 1), StringLength(100)]
        public string ProviderUserId { get; set; }

        public int UserId { get; set; }

        [Column("UserId"), InverseProperty("OAuthMemberships")]
        public Membership User { get; set; }
    }

    [Table("webpages_UsersInRoles")]
    public class UsersInRole
    {
        [Key, Column(Order = 0)]
        public int UserId { get; set; }

        [Key, Column(Order = 1)]
        public int RoleId { get; set; }

        [Column("RoleId"), InverseProperty("UsersInRoles")]
        public Role Roles { get; set; }

        [Column("UserId"), InverseProperty("UsersInRoles")]
        public Membership Members { get; set; }

    }

    [Table("webpages_Roles")]
    public class Role
    {
        public Role()
        {
            UsersInRoles = new List<UsersInRole>();
        }

        [Key]
        public int RoleId { get; set; }
        [StringLength(256)]
        public string RoleName { get; set; }

        //public ICollection<Membership> Members { get; set; }
        
        [ForeignKey("RoleId")]
        public ICollection<UsersInRole> UsersInRoles { get; set; }
    }

    #endregion

    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }
}
