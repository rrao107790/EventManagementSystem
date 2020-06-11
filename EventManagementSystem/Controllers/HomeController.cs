using EventManagementSystem.Data;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventManagementSystem.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var events = this.db.Events.OrderBy(e => e.StartDateTime)
                                       .Where(e => e.IsPublic)
                                       .Select(e => new EventViewModel()
                                       {
                                           Id = e.Id,
                                           Title = e.Title,
                                           StartDateTime = e.StartDateTime,
                                           Duration = e.Duration,
                                           Author = e.Author.FullName,
                                           Location = e.Location
                                       });

            // getting upcoming events and passed events on Index page
            var upcomingEvents = events.Where(e => e.StartDateTime > DateTime.Now);
            var passedEvents = events.Where(e => e.StartDateTime <= DateTime.Now);


            return View(new UpcomingPassedEventsViewModel() { 
                UpcomingEvents = upcomingEvents,
                PassedEvents = passedEvents
            });
        }

        // get event details
        public ActionResult EventDetailsById(int id)
        {
            var currentUserId = User.Identity.GetUserId();
            var isAdmin = IsAdmin();
            var eventDetails =  db.Events
                                .Where(e => e.Id == id)
                                .Where(e => e.IsPublic || isAdmin || (e.AuthorId != null && e.AuthorId == currentUserId))
                                .Select(EventDetailsViewModel.ViewModel).FirstOrDefault();
            var isOwner = (eventDetails != null || eventDetails.AuthorId != null && eventDetails.AuthorId == currentUserId);
            this.ViewBag.CanEdit = isOwner || isAdmin;

            return PartialView("_EventDetails",eventDetails);
        }

        // get the comments posted by User
        public ActionResult OpenComments(int id)
        {
            var currentUserId = User.Identity.GetUserId();
            var isAdmin = IsAdmin();
            var eventDetails = db.Comments
                                .Where(e => e.Id == id)
                                .Where(e=>e.AuthorId != null && e.AuthorId == currentUserId)
                                .Select(CommentViewModel.ViewModel).FirstOrDefault();
            var isOwner = (eventDetails != null || (eventDetails.AuthorId != null && eventDetails.AuthorId == currentUserId));
            this.ViewBag.CanEdit = isOwner || isAdmin;

            return PartialView("_AddComment", eventDetails);
        }

        public ActionResult GetCommentSection(int id)
        {
            return PartialView("_AddComment");
        }
    }
}