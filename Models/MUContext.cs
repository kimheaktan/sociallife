using Microsoft.EntityFrameworkCore;

namespace MeetUp.Models
{
    public class MUContext : DbContext
    {
        public MUContext(DbContextOptions options) : base(options) { } 
        public DbSet<User> Users {get;set;}
        public DbSet<GroupEvent> GroupEvents {get;set;}
        public DbSet<Group> Groups {get;set;}
        public DbSet<Membership> Memberships {get;set;}
        public DbSet<RSVP> RSVPs {get;set;}

    }
}