using EventManagementSystem.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace EventManagementSystem.Models
{
    public class EventInputModel
    {
        [Required(ErrorMessage ="Event Title is required")]
        [StringLength(200, ErrorMessage ="The {0} must between {2} and {1} characters long",MinimumLength =2)]
        public string Title { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name ="Date and Time *")]
        public DateTime StartDateTime { get; set; }
        public TimeSpan? Duration { get; set; }
        public string Description { get; set; }

        [MaxLength(200)]
        public string Location { get; set; }

        [Display(Name ="Is Public?")]
        public bool IsPublic { get; set; }

        
        public static Expression<Func<Event, EventInputModel>> CreateFromEvent
        {
            get
            {
                return e => new EventInputModel()
                {
                    Title = e.Title,
                    StartDateTime = e.StartDateTime,
                    Duration = e.Duration,
                    Description = e.Description,
                    Location = e.Location
                };
            }
        }
    }
}