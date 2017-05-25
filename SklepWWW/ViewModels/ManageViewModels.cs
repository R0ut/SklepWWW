using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SklepWWW.Models;
using System.ComponentModel.DataAnnotations;

namespace SklepWWW.ViewModels
{
    public class ManageCredentialsViewModel
    {
        public ChangePasswordViewModel ChangePasswordViewModel { get; set; }
        public SklepWWW.Controllers.ManageController.ManageMessageId? Message { get; set; }
        public DaneUzytkownika DaneUzytkownika { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Hasło {0} musi mieć conajmniej {2} znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }
        
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "Oba hasła muszą pasować")]
        public string ConfirmPassword { get; set; }
    }

}