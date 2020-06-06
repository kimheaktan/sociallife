using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MeetUp.Models
{
    public class User 
    {
        [Key]// denotes PK, not needed if named ModelNameId 
        public int UserId {get;set;} 

        [Required(ErrorMessage = "First name is required.")] 
        [Display(Name = "First Name")] 
        [MinLength(2, ErrorMessage = "First name must be at least 2 characters.")] 

        public string FirstName {get;set;} 

        [Required(ErrorMessage = "Last name is required.")] 
        [Display(Name = "Last Name")] 
        [MinLength(2, ErrorMessage = "Last name must be at least 2 characters.")] 

        public string LastName {get;set;} 

        [Required(ErrorMessage = "Email is required.")] 
        [Display(Name = "Email Address")] 
        [EmailAddress] 
        public string Email {get;set;}   

        [Required(ErrorMessage = "Password is required.")] 
        [Display(Name = "Password")] 
        [MinLength(8,ErrorMessage ="Password has to be at least 8 characters.")]
        [DataType(DataType.Password)] 
        public string Password{get;set;} 

        [NotMapped] 
        [Compare("Password", ErrorMessage = "Confirm Password does not match Password.")] 
        [Display(Name = "Confirm Password")] 
        [DataType(DataType.Password)] 
        public string ConfirmPassword {get;set;}
        public DateTime CreatedAt {get;set;}=DateTime.Now; 
        public DateTime UpdatedAt {get;set;}=DateTime.Now; 

        public List<RSVP> Events  {get;set;}

        public List<Membership> Memberships {get;set;}
        public string FullName() 
        { 
            return LastName + ", " + FirstName; 
        } 

    }
}