using empmgmt.Data;
using empmgmt.Extension.Captcha;
using empmgmt.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO;

namespace empmgmt.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDBContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(AppDBContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [Route("get-captcha-image")]
        public IActionResult GetCaptchaImage()
        {
            int width = 100;
            int height = 36;
            var captchaCode = Captcha.GenerateCaptchaCode();
            var result = Captcha.GenerateCaptchaImage(width, height, captchaCode);
            HttpContext.Session.SetString("CaptchaCode", result.CaptchaCode);
            Stream s = new MemoryStream(result.CaptchaByteData);
            return new FileStreamResult(s, "image/png");
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Index(LoginViewModel login)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!Captcha.ValidateCaptchaCode(login.CaptchaCode, HttpContext))
                    {
                        ViewBag.error = "!!Please Enter valid Captcha";
                        return View();
                    }

                    if (_context.login_user.Any(a => a.login_user_name == login.UserName && a.login_password == login.Password))
                    {
                        HttpContext.Session.SetString("username", login.UserName);
                        return RedirectToAction("Index", "Employee");
                    }
                    else
                    {
                        ViewBag.error = "!!Please Enter valid login credentials.";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.error = $"!!There is some error. {ex.Message}";
            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}