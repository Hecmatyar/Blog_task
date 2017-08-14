using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class Category
    {
        public int Id { get; set; }
        [DisplayName("Категория")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string UrlSlug { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
        public Category()
        {
            Posts = new List<Post>();
        }
    }
}