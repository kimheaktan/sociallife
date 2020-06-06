using System;
using System.Collections.Generic;
using System.Linq;
using MeetUp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeetUp.Controllers
{
    public class GroupController : Controller
    {
        private MUContext _db; 

        public GroupController(MUContext context) 
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

        [HttpGet("createGroup")]
        public IActionResult NewGroup()
        {
            if(!_isLoggedIn)
            {
                return View("PleaseSingup");
            }
            return View();
        }

        [HttpPost("newgroup")]
        public IActionResult CreateGroup(Group newGroup)
        {
            if(!_isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            if(ModelState.IsValid)
            {
                // int? _uid = HttpContext.Session.GetInt32("UserId");
                User currUser = _db.Users.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("UserId"));

                newGroup.CreatorId = currUser.UserId;

                _db.Add(newGroup);

                Membership member = new Membership {
                    UserId = currUser.UserId,
                    GroupId = newGroup.GroupId
                };
                _db.Add(member);
                _db.SaveChanges();

                return RedirectToAction ("Dashboard", "Home");
            }
            return View ("NewGroup");
        }

        // [HttpGet("group/{groupId}")]
        public IActionResult ViewGroup(int groupId)
        {
            Group CurrGroup = _db.Groups
            .Include(g => g.Creator)
            .Include(g => g.Members)
            .ThenInclude(m => m.User)
            .Include(g => g.GroupEvents)
            .ThenInclude(ge => ge.Participants)
            .FirstOrDefault(g => g.GroupId == groupId);

            List<GroupEvent> thisGroupEvents = _db.GroupEvents.Include(ge => ge.Participants).ThenInclude(r => r.User).Include(ge => ge.Group).Where(ge => ge.GroupId == groupId).OrderBy(ge => ge.GroupEventDate).ToList();

            int Comming = 0;
            int Past = 0;
            foreach(var e in thisGroupEvents)
            {
                if(e.GroupEventDate > DateTime.Now)
                {
                    Comming += 1;
                }
                else
                {
                    Past += 1;
                }
            }

            ViewGroupModel data = new ViewGroupModel{
                EventsInGroup = thisGroupEvents,
                Group = CurrGroup,
                C = Comming,
                P = Past
                
            };

            return View(data);
        }

        [HttpGet("join/{groupId}")]
        public IActionResult JoinGroup(int groupId)
        {
            if(!_isLoggedIn)
            {
                return View("CannotJoin");
            }
            int? _uid = HttpContext.Session.GetInt32("UserId");
            User currUser = _db.Users.FirstOrDefault(u => u.UserId == _uid);

            Membership newMembership = new Membership{
                GroupId = groupId,
                UserId = currUser.UserId
            };

            _db.Memberships.Add(newMembership);
            _db.SaveChanges();

            return RedirectToAction("ViewGroup", new{groupId = groupId});
        }

        [HttpGet("leavegroup")]
        public IActionResult LeaveGroup(int groupId)
        {
            if(!_isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            Membership TBRemoved = _db.Memberships.FirstOrDefault(m => m.UserId == _uid && m.GroupId == groupId);

            _db.Memberships.Remove(TBRemoved);
            _db.SaveChanges();

            return RedirectToAction("Dashboard", "Home");
        }

        [HttpGet("editgroup/{groupId}")]
        public IActionResult GroupEditForm(int groupId)
        {
            if(!_isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            HttpContext.Session.SetInt32("EditGId", groupId);

            Group tbEditedGroup = _db.Groups.FirstOrDefault(g => g.GroupId == groupId);

            GroupEditViewModel data = new GroupEditViewModel {
                TBEditedGroup = tbEditedGroup
            };
            return View(data);
        }

        [HttpPost("edit/{groupId}")]
        public IActionResult EditGroup(GroupEditViewModel NewGroupInfo, int groupId)
        {
            if(!_isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            if(ModelState.IsValid)
            {
                Group EditedGroup = _db.Groups.FirstOrDefault(g => g.GroupId == groupId);

                EditedGroup.GroupName = NewGroupInfo.Group.GroupName;
                EditedGroup.Description = NewGroupInfo.Group.Description;
                EditedGroup.UpdatedAt = DateTime.Now;

                _db.SaveChanges();
                return RedirectToAction("ViewGroup", new{groupId = groupId});
            }
            return View("GroupEditForm");
        }

        [HttpGet("groups")]
        public IActionResult SearchForGroup()
        {
            List<Group> AllGroups = _db.Groups.Include(g => g.Members).ThenInclude(m => m.User).OrderBy(g => g.GroupName).ToList();
            
            return View(AllGroups);
        }

        [HttpGet("deletegroup/groupId")]
        public IActionResult DeleteGroup(int groupId)
        {
            if(!_isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            Group TBRemoved = _db.Groups.FirstOrDefault(g => g.GroupId == groupId);
            _db.Remove(TBRemoved);
            _db.SaveChanges();

            return RedirectToAction("Dashboard", "Home");
        }


    }
}

