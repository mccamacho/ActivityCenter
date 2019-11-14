using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using DojoActivity.Models;


namespace DojoActivity.Models
{
    public class User 
    {
        [Key]
        public int UserID {get; set;}
        [Required]
        [MinLength(2,ErrorMessage = "A real First Name is required")]
        public string FirstName{ get; set;}
        [Required]
        [MinLength(2,ErrorMessage = "A real Last Name is required")]
        public string LastName{ get; set;}
        [Required]
        [EmailAddress]
        public string Email{ get; set;}
        [Required]
        [MinLength(5)]
        [DataType(DataType.Password)]
        [RegularExpression(@"(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "Password must contain at least one uppercase, one lowercase, and one number.")]
        public string Password{ get; set;}

        [Compare("Password", ErrorMessage = "Password and confirmation must match.")]
        [DataType(DataType.Password)]
        [Display(Name="Confirm Password")]
        public string PasswordConfirm{ get; set;}
        public DateTime CreatedAt {get; set;}
        public DateTime UpdatedAt {get; set;}
        public List<UserActivity> JoiningActivity{ get; set;}
        public User()
        {
            JoiningActivity = new List<UserActivity>();
        }

    }
}