using EventManagementSystem.Models;
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

    }
}