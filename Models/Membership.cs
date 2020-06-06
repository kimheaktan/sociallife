using System;

namespace MeetUp.Models
{
    public class Membership
    {
        public int MembershipId {get;set;}
        public int UserId {get;set;}
        public User User {get;set;}
        public int GroupId {get;set;}
        public Group Group {get;set;}

        public DateTime JoinAt {get;set;}=DateTime.Now; 

    }
}