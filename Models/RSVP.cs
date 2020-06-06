using System;

namespace MeetUp.Models
{
    public class RSVP
    {
        public int RSVPId {get;set;}
        public int UserId {get;set;}
        public User User {get;set;}
        public int GroupEventId {get;set;}
        public GroupEvent GroupEvent {get;set;}
        public DateTime JoinAt {get;set;}=DateTime.Now; 
    }
}