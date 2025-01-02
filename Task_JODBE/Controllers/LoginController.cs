using System.Web.Mvc;
using System.Web.Security;
using System;
using Task_JODBE.Models;
using System.Linq;

public class LoginController : Controller
{
    private readonly UserManagementDBEntities _dbContext;

    public LoginController()
    {
        _dbContext = new UserManagementDBEntities();
    }

    // GET: Login/Index
    public ActionResult Index()
    {
        if (Request.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home"); // Redirect to Home if already logged in
        }
        return View();
    }

    // POST: Login/Index
    [HttpPost]
    [AllowAnonymous]
    public JsonResult Index([Bind(Include = "Email, password")] AdminUser adminUser)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var user = _dbContext.AdminUsers.SingleOrDefault(u => u.Email == adminUser.Email);
                if (user != null && user.password == adminUser.password)
                {
                    // Create authentication ticket
                    FormsAuthentication.SetAuthCookie(user.Email, true);
                    return Json(new { isLoggedIn = true, message = "Login successful" });
                }
                else
                {
                    return Json(new { isLoggedIn = false, message = "Invalid email or password" });
                }
            }
            return Json(new { isLoggedIn = false, message = "Invalid data provided" });
        }
        catch (Exception ex)
        {
            return Json(new { isLoggedIn = false, message = "An error occurred on the server." });
        }
    }
    public ActionResult Logout()
    {
        // Clear the authentication cookie
        System.Web.Security.FormsAuthentication.SignOut();

        // Redirect to the login page or any other page you desire
        return RedirectToAction("Index", "Login");
    }
}
