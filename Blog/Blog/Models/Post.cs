using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string UrlSlug { get; set; }
        public DateTime Published { get; set; }
        public string Author { get; set; } 
        
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public ICollection<Tag> Tags { get; set; }
        public Post()
        {
            Tags = new List<Tag>();
        }
    }
}