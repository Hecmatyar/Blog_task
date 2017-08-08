using Blog.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        private PostContext db = new PostContext();
        List<Post> posts;
        List<Category> category;
        List<Tag> tags;

        public ActionResult Index(int? page,
            string currentFilter,
            string searchString,
            string currentCategory,
            string searchCategory,
            string currentTag,
            string searchTag,
            string searchTheme)
        {          
            if (searchString != null) { page = 1; }
            else { searchString = currentFilter; }

            if (searchCategory != null) { page = 1; }
            else { searchCategory = currentCategory; }

            if (searchTag != null) { page = 1; }
            else { searchTag = currentTag; }

            ViewBag.CurrentFilter = searchString;
            ViewBag.currentCategory = searchCategory;
            ViewBag.currentTag = searchTag;
            ViewBag.searchTheme = searchTheme;

            if (!String.IsNullOrEmpty(searchString))
            {
                posts = db.Posts
                    .Where(a => a.Author.Contains(searchString) || a.Title.Contains(searchString))
                    .OrderByDescending(d => d.Published)
                    .ToList();                
                ViewBag.searchTheme = "Найденные посты";
            }
            else
            if (!String.IsNullOrEmpty(searchCategory))
            {
                posts = db.Posts.Where(a => a.Category.Name.Contains(searchCategory))
                    .OrderByDescending(d=>d.Published)
                    .ToList();               
                ViewBag.searchTheme = "Посты в категории " + ViewBag.currentCategory;
            }
            else
            if (!String.IsNullOrEmpty(searchTag))
            {
                posts = db.Posts.Where(a => a.Tags.Any(b => b.Name.Contains(searchTag)))
                    .OrderByDescending(d => d.Published)
                    .ToList();                
                ViewBag.searchTheme = "Посты с тэгом " + ViewBag.currentTag;
            }
            else
            {
                ViewBag.searchTheme = "Опубликованные посты";
                posts = db.Posts.OrderByDescending(d => d.Published).ToList();
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            
            return View(posts.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult CategoryList()
        {
            category = db.Categories.ToList();
            return View("CategoryList", category);
        }
        [HttpGet]
        public ActionResult TagList()
        {
            tags = db.Tags.ToList();
            return View("TagList", tags);
        }

        public ActionResult Details(int id = 0)
        {
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }
        [HttpGet]
        public ActionResult Post(int id)
        {
            ViewBag.categories = db.Categories.ToList();
            ViewBag.tags = db.Tags.ToList();
            Post post = db.Posts.ToList().FirstOrDefault(p => p.Id == id);
            return View(post);
        }

        [HttpPost]
        public ActionResult PostList(string name, int? page)
        {
            posts = db.Posts.Where(a => a.Author.Contains(name)).ToList();
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            if (posts.Count <= 0)
            {
                return HttpNotFound();
            }
            return PartialView(posts.ToPagedList(pageNumber, pageSize));
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}