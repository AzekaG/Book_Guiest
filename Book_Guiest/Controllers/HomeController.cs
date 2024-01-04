using Book_Guiest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Book_Guiest.Controllers
{
    public class HomeController : Controller
    {

        private readonly UsersContext _context;

        public HomeController(UsersContext context)
        {
            _context = context;
        }

        
        

        public ActionResult Index()
        {
            
            List<Message> mes = _context.Messages.Include(m => m.User).ToList();
            ViewBag.Mes = mes;
            return View();
        }


        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Users");
        }
    }
}
