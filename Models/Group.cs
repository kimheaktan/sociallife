using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MeetUp.Models
{
    public class Group
    {
        [Key]
        public int GroupId {get;set;}
        public int CreatorId {get;set;}
        public User Creator {get;set;}

        [Required (ErrorMessage = "is required.")]
        [MinLength (5, ErrorMessage = "Group Name must be at least 5 characters.")]
        [Display (Name = "Group Name:")]
        public string GroupName {get;set;}

        [Required (ErrorMessage = "is required.")]
        // [MinLength (5, ErrorMessage = "Group Name must be at least 5 characters.")]
        [Display (Name = "City:")]
        public string City {get;set;}

        [Required (ErrorMessage = "is required.")]
        [MinLength (5, ErrorMessage = "Description must be at least 5 characters.")]
        [Display (Name = "Description:")]
        public string Description {get;set;}

        public List<Membership> Members {get;set;}
        public List<GroupEvent> GroupEvents {get;set;}

        public DateTime CreatedAt {get;set;}=DateTime.Now; 
        public DateTime UpdatedAt {get;set;}=DateTime.Now; 

        
    }
}