using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class Post
    {
        public int Id { get; set; }
        [DisplayName("Заголовок")]
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string UrlSlug { get; set; }
        //[DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Опубликовано")]
        public DateTime Published { get; set; }
        [DisplayName("Автор")]
        public string Author { get; set; }
        public int CategoryId { get; set; }       
        public virtual Category Category { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public Post()
        {
            Tags = new List<Tag>();
        }
    }
}