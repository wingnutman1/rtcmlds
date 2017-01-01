using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CMS_Web.ViewModels
{
    public class LostPasswordModel
    {
        [Required(ErrorMessage = "Please enter your email so a reset link can be set.")]
        [Display(Name = "Your account email.")]
        [EmailAddress(ErrorMessage = "Invalid email.")]
        public string Email { get; set; }
    }
}