using EventManagementSystem.Data;
using System;
using System.Linq.Expressions;

namespace EventManagementSystem.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public string AuthorId { get; set; }
        public string Text { get; set; }

        public string Author { get; set; }

        public int EventId { get; set; }
        public static Expression<Func<Comment, CommentViewModel>> ViewModel
        {
            get
            {
                return c => new CommentViewModel()
                {
                    AuthorId = c.AuthorId,
                    Text = c.Text,
                    Author = c.Author.FullName
                };
            }
        }
    }
}