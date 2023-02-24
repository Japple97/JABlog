using JABlog.Data;
using JABlog.Models;
using JABlog.Services;
using JABlog.Services.Interfaces;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using X.PagedList;

namespace JABlog.Controllers
{
    public class HomeController : Controller
    {
     
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogPostService _blogPostService;
        private readonly IEmailSender _emailService;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IBlogPostService blogPostService, IEmailSender emailService)
        {
           
            _logger = logger;
            _blogPostService = blogPostService;
            _emailService = emailService;
        }

        public async Task<IActionResult> Index(int? pageNum, string? swalMessage = null)
        {
            ViewData["SwalMessage"] = swalMessage;
            int pageSize = 3;
            int page = pageNum ?? 1;
            
            IPagedList<BlogPost> model = (await _blogPostService.GetRecentPostsAsync()).ToPagedList(page,pageSize);

            return View(model);
        }

        public IActionResult ContactMe(string? swalMessage = null)
        {
            ViewData["SwalMessage"] = swalMessage;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EmailMe (EmailData emailData)
        {
            if (ModelState.IsValid)
            {
                string? swalMessage = string.Empty;
                try
                {
                    emailData.EmailBody = ($"""<strong>{emailData.FullName}</strong> sent a message:<br><br>{emailData.EmailBody}<br><br><strong>Their email is:<a href="mailto:{emailData.EmailAddress}">{emailData.EmailAddress}</a></strong>""");
              
                    await  _emailService.SendEmailAsync("ja999dev@gmail.com",
                                                       emailData.EmailSubject!,
                                                       emailData.EmailBody!);
                    swalMessage = "Your email has been sent.";
                    return RedirectToAction(nameof(Index), new { swalMessage } );
                }
                catch (Exception e)
                {
                    swalMessage = "Error: Email Send Failed!";
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                    return RedirectToAction(nameof(Index), new { swalMessage });
                    throw;
                }
            }
            return View(emailData);
        }

        public IActionResult SearchIndex(string? searchString, int? pageNum)
        {
            int pageSize = 5;
            int page = pageNum ?? 1;
            IPagedList<BlogPost> model = (_blogPostService.SearchBlogPosts(searchString)).ToPagedList(page, pageSize);

            return View(nameof(Index), model);
        }





        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}