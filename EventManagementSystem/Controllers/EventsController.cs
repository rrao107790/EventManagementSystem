using EventManagementSystem.Data;
using EventManagementSystem.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventManagementSystem.Controllers
{
    [Authorize]
    public class EventsController : BaseController
    {
        // GET: Events
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EventInputModel model)
        {
            if(model !=null && ModelState.IsValid)
            {
                var e = new Event()
                {
                    AuthorId = this.User.Identity.GetUserId(),
                    Title = model.Title,
                    StartDateTime = model.StartDateTime,
                    Duration = model.Duration,
                    Description = model.Description,
                    Location = model.Location,
                    IsPublic = model.IsPublic
                };

                db.Events.Add(e);
                db.SaveChanges();

                return RedirectToAction("MyEvents");
            }
            return View(model);
        }

        public ActionResult MyEvents()
        {
            string currentUserId = User.Identity.GetUserId();
            var events = db.Events
                        .Where(e => e.AuthorId == currentUserId)
                        .OrderBy(e => e.StartDateTime)
                        .Select(EventViewModel.ViewModel);
            var upcomingEvents = events.Where(e => e.StartDateTime > DateTime.Now);
            var passedEvents = events.Where(e => e.StartDateTime <= DateTime.Now);

            return View(new UpcomingPassedEventsViewModel()
            {
                UpcomingEvents = upcomingEvents,
                PassedEvents = passedEvents
            });
        }

        // edits the forms
        public ActionResult Edit(int id)
        {
            var eventToEdit = LoadEvent(id);
            if(eventToEdit == null)
            {
                return RedirectToAction("MyEvents");
            }
            var model = EventInputModel.CreateFromEvent(eventToEdit);
            return View(model);
        }

        // Loads the event
        private Event LoadEvent(int id)
        {
            var currentUserId = User.Identity.GetUserId();
            var isAdmin = IsAdmin();
            var eventToEdit = db.Events.Where(x => x.Id == id).FirstOrDefault(e => e.AuthorId == currentUserId || isAdmin);
            return eventToEdit;
        }
    }
}