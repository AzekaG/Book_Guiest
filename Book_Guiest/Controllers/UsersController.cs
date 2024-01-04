using Book_Guiest.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace Book_Guiest.Controllers
{
    public class UsersController : Controller
    {
        private readonly UsersContext _context;
       
      

        public UsersController(UsersContext context)
        {
            _context = context;
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginModel logon)
        {
            if (ModelState.IsValid)
            {
                if (_context.Users.ToList().Count == 0)
                {
                    ModelState.AddModelError("", "Неверный логин или пароль");
                    return View(logon);
                }
                var users = _context.Users.Where(a => a.Email == logon.Email);
                if (users.ToList().Count == 0)
                {
                    ModelState.AddModelError("", "Неверный логин или пароль");
                    return View(logon);
                }
                var user = users.First();
                string? salt = user.Salt;

                //переводим пароль в байт-массив  
                byte[] password = Encoding.Unicode.GetBytes(salt + logon.Password);

                //создаем объект для получения средств шифрования  
                var md5 = MD5.Create();

                //вычисляем хеш-представление в байтах  
                byte[] byteHash = md5.ComputeHash(password);

                StringBuilder hash = new StringBuilder(byteHash.Length);
                for (int i = 0; i < byteHash.Length; i++)
                    hash.Append(string.Format("{0:X2}", byteHash[i]));

                if (user.Password != hash.ToString())
                {
                    ModelState.AddModelError("", "Неверный логин или пароль");
                    return View(logon);
                }
                
                HttpContext.Session.SetString("FirstName", user.FirstName);
                HttpContext.Session.SetString("LastName", user.LastName);
                HttpContext.Session.SetString("Id", user.Id.ToString());
                return RedirectToAction("Index", "Home");
            }
            return View(logon);
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Registration(RegistrationModel reg)
        {
            if (ModelState.IsValid)
            {
                Users user = new Users();
                user.FirstName = reg.FirstName;
                user.LastName = reg.LastName;
                user.Email = reg.Email;

                byte[] saltbuf = new byte[16];

                RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
                randomNumberGenerator.GetBytes(saltbuf);

                StringBuilder sb = new StringBuilder(16);
                for (int i = 0; i < 16; i++)
                    sb.Append(string.Format("{0:X2}", saltbuf[i]));
                string salt = sb.ToString();

                //переводим пароль в байт-массив  
                byte[] password = Encoding.Unicode.GetBytes(salt + reg.Password);

                //создаем объект для получения средств шифрования  
                var md5 = MD5.Create();

                //вычисляем хеш-представление в байтах  
                byte[] byteHash = md5.ComputeHash(password);

                StringBuilder hash = new StringBuilder(byteHash.Length);
                for (int i = 0; i < byteHash.Length; i++)
                    hash.Append(string.Format("{0:X2}", byteHash[i]));

                user.Password = hash.ToString();
                user.Salt = salt;
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }

            return View(reg);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EnterMessage([Bind("Id, msg")] Message mes)
        {
           
                int id = int.Parse(HttpContext.Session.GetString("Id"));

                mes.date = DateTime.Now;



                mes.User = _context.Users.Where(x => x.Id == id).First();
                
                _context.Add(mes);
                _context.SaveChanges();
            
            return RedirectToAction("Index", "Home");
            

        }

        public IActionResult GetMessages() 
        {
           List<Message> res =  _context.Messages.ToList();
           return View(res);
        }
        
     











    }
}
