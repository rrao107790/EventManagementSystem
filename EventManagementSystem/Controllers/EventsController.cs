using EventManagementSystem.Data;
using EventManagementSystem.Extensions;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

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
            if (model != null && ModelState.IsValid)
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
                this.AddNotification("Event Created Successfully!", NotificationType.SUCCESS);
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

            var currentUserId = this.User.Identity.GetUserId();
            var isAdmin = this.IsAdmin();
            var eventDetails = this.db.Events
                                .Where(e => e.Id == id)
                                .Where(e => e.IsPublic && e.AuthorId == currentUserId || isAdmin || (e.AuthorId != null && e.AuthorId == currentUserId))
                                .Select(EventInputModel.CreateFromEvent).FirstOrDefault();
            var isOwner = (eventDetails != null);
            this.ViewBag.CanEdit = isOwner || isAdmin;
            return View(eventDetails);
        }

        // edit an event
        [HttpPost]
        public ActionResult Edit(int id, EventInputModel model)
        {

            var eventToEdit = db.Events.Where(x => x.Id == id).FirstOrDefault();

            if (model != null && ModelState.IsValid)
            {
                eventToEdit.Title = model.Title;
                eventToEdit.StartDateTime = model.StartDateTime;
                eventToEdit.Duration = model.Duration;
                eventToEdit.Description = model.Description;
                eventToEdit.Location = model.Location;
                eventToEdit.IsPublic = model.IsPublic;

                db.SaveChanges();
                return RedirectToAction("MyEvents");
            }

            return RedirectToAction("MyEvents");
        }

        // delete an event
        public ActionResult Delete(int id)
        {
            var currentUserId = this.User.Identity.GetUserId();
            var isAdmin = this.IsAdmin();
            var eventDetails = this.db.Events
                                .Where(e => e.Id == id)
                                .Where(e => e.IsPublic && e.AuthorId == currentUserId || isAdmin || (e.AuthorId != null && e.AuthorId == currentUserId))
                                .Select(EventInputModel.CreateFromEvent).FirstOrDefault();
            var isOwner = (eventDetails != null);
            this.ViewBag.CanEdit = isOwner || isAdmin;
            return View(eventDetails);
        }

        // call the ajax button method and redirect the specific action
        public ActionResult ManageComments()
        {
            return RedirectToAction("ShowComments");
        }

        // show all comments
        public ActionResult ShowComments()
        {
            var showComments = db.Comments.ToList();
            return View(showComments);
        }
        [HttpPost]
        public ActionResult ShowComments(int id)
        {
            var showComments = db.Comments.ToList();

            return View(showComments);
        }

        // get the comment to delete
        public ActionResult DeleteComment(int id)
        {
            var currentUserId = this.User.Identity.GetUserId();
            var commentToDelete = db.Comments.Where(x => x.Id == id && x.AuthorId == currentUserId).FirstOrDefault();

            bool status = commentToDelete == null;
            if (status)
            {
                this.AddNotification("You cannot delete someone else's comment because you are authorized", NotificationType.ERROR);
            }

            db.Comments.Remove(commentToDelete);
            db.SaveChanges();
            this.AddNotification("Comment Deletion Success!", NotificationType.SUCCESS);
            return RedirectToAction("Index");
        }
    }
}