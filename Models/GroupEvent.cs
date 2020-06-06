using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MeetUp.Models
{
    public class GroupEvent
    {
        [Key]
        public int GroupEventId {get;set;}

        [Required(ErrorMessage="Event is required.")]
        [Display(Name ="Event:")]
        [MinLength (5, ErrorMessage = "Event name must be at least 5 characters.")]
        public string GroupEventName {get;set;}

        [Required(ErrorMessage="Details is required.")]
        [Display(Name ="Details:")]
        [MinLength (10, ErrorMessage = "Event details must be at least 10 characters.")]
        public string GroupEventDetails {get;set;}

        [Required(ErrorMessage="Date is required.")]
        [Display(Name ="Date:")]
        [OnlyDateInFuture]
        public DateTime GroupEventDate {get;set;}

        [Required (ErrorMessage = "Street is required.")]
        [MinLength (2, ErrorMessage = "Street address must be at least 2 characters.")]
        [Display (Name = "Street Address:")]
        public string Street { get; set; }

        [Required (ErrorMessage = "City is required.")]
        [MinLength (2, ErrorMessage = "City/Town must be at least 2 characters.")]
        [Display (Name = "City/Town:")]
        public string City { get; set; }

        [Required (ErrorMessage = "State is required.")]
        [Display (Name = "State:")]
        public string State { get; set; }

        [Required (ErrorMessage = "Zip code is required.")]
        [MinLength (5, ErrorMessage = "Zip code must be at least 5 characters.")]
        [Display (Name = "Zip code:")]
        public string Zip { get; set; }

        public int CreatorId {get;set;}
        public User Creator {get;set;}
        public int GroupId {get;set;}
        public Group Group {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        public List<RSVP> Participants {get;set;}


        public string Address()
            {
                return Street + " " + City + ", " + State + " " + Zip;
            }

    }

    public class OnlyDateInFutureAttribute: ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if((DateTime)value < DateTime.Now)
                    return new ValidationResult("Date must be in the Future");
                return ValidationResult.Success;
            }
        
        }
}