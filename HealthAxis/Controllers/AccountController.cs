using HealthAxis.Mvc.Services.Interfaces;
using HealthAxis.Shared.DTOs;
using System.Web.Mvc;
using System.Web.Security;

namespace HealthAxis.Mvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthMvcService _authService;

        public AccountController(IAuthMvcService authService)
        {
            _authService = authService;
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Login(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new LoginDto { Role = role });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var user = _authService.Login(dto, out string errorMessage);

            if (user == null || !user.IsSuccess)
            {
                ModelState.AddModelError("", errorMessage);
                return View(dto);
            }

            FormsAuthentication.SetAuthCookie(user.UserId, false);

            Session["UserId"] = user.UserId;
            Session["Role"] = user.Role;
            Session["ReferenceId"] = user.ReferenceId;
            Session["Email"] = user.Email;

            if (user.Role == "Doctor")
            {
                return RedirectToAction("DoctorAppointments", "Appointments");
            }

            if (user.Role == "Patient")
            {
                return RedirectToAction("Index", "Patients");
            }

            return RedirectToAction("Index", "Home");
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult SignUp(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Role = role;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUpPatient(PatientDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Role = "Patient";
                return View("SignUp");
            }

            bool result = _authService.SignUpPatient(dto, out string errorMessage);

            if (!result)
            {
                ModelState.AddModelError("", errorMessage);
                ViewBag.Role = "Patient";
                return View("SignUp");
            }

            TempData["Success"] = "Registration successful. Please login.";
            return RedirectToAction("Login", new { role = "Patient" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUpDoctor(DoctorDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Role = "Doctor";
                return View("SignUp");
            }

            bool result = _authService.SignUpDoctor(dto, out string errorMessage);

            if (!result)
            {
                ModelState.AddModelError("", errorMessage);
                ViewBag.Role = "Doctor";
                return View("SignUp");
            }

            TempData["Success"] = "Registration successful. Please login.";
            return RedirectToAction("Login", new { role = "Doctor" });
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();

            return RedirectToAction("Index", "Home");
        }
    }
}