using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IdentitySample.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "信箱")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "驗證碼")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "記住這個視窗?")]
        public bool RememberBrowser { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "信箱")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "信箱")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密碼")]
        public string Password { get; set; }

        [Display(Name = "記住我?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "信箱")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "至少六個字元並包含數字，英文及特殊符號", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密碼")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "確認密碼")]
        [Compare("Password", ErrorMessage = "密碼不符!")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "信箱")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "至少六個字元並包含數字，英文及特殊符號", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密碼")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "確認密碼")]
        [Compare("Password", ErrorMessage = "密碼不符")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "信箱")]
        public string Email { get; set; }
    }
}