using System.Collections.Generic;

namespace MeetUp.Models
{
    public class DashView
    {
        public User User{get;set;}
        public List<RSVP> RSVPsOfThisUser {get;set;}
        public List<Membership> UserGroups{get;set;}

        public List<Membership> AllMemberships {get;set;}
    }
}