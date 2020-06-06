using System.Linq;
using MeetUp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeetUp.Controllers
{
    public class GroupEventController : Controller
    {
        private MUContext _db; 

        public GroupEventController(MUContext context) 
        { 
            _db = context; 
        } 

        private int? _uid
        {
            get
            {
                return HttpContext.Session.GetInt32("UserId");
            }
        }
        private bool _isLoggedIn
        {
            get
            {
                int? uid = _uid;

                if (uid != null)
                {
                    User loggedInUser =
                        _db.Users.FirstOrDefault(u => u.UserId == uid);

                    HttpContext.Session
                        .SetString("FullName", loggedInUser.FullName());
                }
                return uid != null;
            }
        }

        public IActionResult NewGroupEvent(int groupId)
        {
            if(!_isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            HttpContext.Session.SetInt32("groupID", groupId);

            return View();
        }

        // [HttpPost("CreateEvent/{groupId}")]
        public IActionResult CreateGroupEvent(GroupEvent newEvent, int groupId)
        {
            if(!_isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            if(ModelState.IsValid)
            {
                User currUser = _db.Users.FirstOrDefault(u => u.UserId == _uid);

                newEvent.CreatorId = currUser.UserId;
                newEvent.Creator = currUser;
                newEvent.GroupId = groupId;

                _db.Add(newEvent);
                _db.SaveChanges();

                RSVP hostRSVP = new RSVP{
                    UserId = currUser.UserId,
                    GroupEventId = newEvent.GroupEventId
                };

                _db.RSVPs.Add(hostRSVP);
                _db.SaveChanges();

                return RedirectToAction("ViewGroup", "Group", new{groupId = groupId});
            }
            return View("NewGroupEvent");
        }

        public IActionResult RSVP(int groupId, int GEId)
        {
            if(!_isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            User currUser = _db.Users.FirstOrDefault(u => u.UserId == _uid);

            RSVP rsvp = new RSVP{
                UserId = currUser.UserId,
                GroupEventId = GEId
            };
            _db.Add(rsvp);
            _db.SaveChanges();

            return RedirectToAction("ViewGroupEvent" ,new{GEID = GEId});
        }

        public IActionResult UnRSVP(int GEId, int groupId)
        {
            if(!_isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            RSVP TBRemoved = _db.RSVPs.FirstOrDefault(r => r.GroupEventId == GEId && r.UserId == HttpContext.Session.GetInt32("UserId"));
            _db.RSVPs.Remove(TBRemoved);
            _db.SaveChanges();

            return RedirectToAction("ViewGroupEvent", "GroupEvent",new{GEID = GEId});
        }
        public IActionResult ViewGroupEvent(int GEID)
        {
            if(!_isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            GroupEvent TBViewed = _db.GroupEvents.Include(ge => ge.Participants).ThenInclude(r => r.User).FirstOrDefault(ge => ge.GroupEventId == GEID);

            return View(TBViewed);
        }

        public IActionResult DeleteGroupEvent(int groupId, int GEId)
        {
            if(!_isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            GroupEvent TBRemoved = _db.GroupEvents.FirstOrDefault(ge => ge.GroupEventId == GEId);
            _db.GroupEvents.Remove(TBRemoved);
            _db.SaveChanges();

            return RedirectToAction("ViewGroup", "Group", new{groupId = groupId});
        }


        public IActionResult GroupEventEditForm(int GEId, int groupId)
        {
            if(!_isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            HttpContext.Session.SetInt32("EID", GEId);
            HttpContext.Session.SetInt32("GID", groupId);

            GroupEvent TBEdited = _db.GroupEvents.FirstOrDefault(ge => ge.GroupEventId == GEId);

            GroupEventEditView data = new GroupEventEditView{
                thisEvent = TBEdited
            };
            return View(data);
        }

        public IActionResult EditGroupEvent( int GEId, GroupEventEditView newInput)
        {
            if(!_isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            GroupEvent TBEdited = _db.GroupEvents.FirstOrDefault(ge => ge.GroupEventId == GEId);
            TBEdited.GroupEventName = newInput.GroupEvent.GroupEventName;
            TBEdited.GroupEventDetails = newInput.GroupEvent.GroupEventDetails;
            TBEdited.GroupEventDate = newInput.GroupEvent.GroupEventDate;
            TBEdited.Street = newInput.GroupEvent.Street;
            TBEdited.City = newInput.GroupEvent.City;
            TBEdited.State = newInput.GroupEvent.State;
            TBEdited.Zip = newInput.GroupEvent.Zip;

            _db.SaveChanges();

            return RedirectToAction("ViewGroupEvent", new{GEId = GEId});
        }
    }

}