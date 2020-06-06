using System.Collections.Generic;

namespace MeetUp.Models
{
    public class ViewGroupModel
    {
        public List<GroupEvent> EventsInGroup {get;set;}

        public Group Group {get;set;}
        public int C {get;set;}
        public int P {get;set;}
    }
}