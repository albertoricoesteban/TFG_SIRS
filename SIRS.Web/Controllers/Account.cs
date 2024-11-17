using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SIRS.Web.Controllers
{
    public class Account : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        // GET: Account/Details/5
        public ActionResult Register(int id)
        {
            return View();
        }
    }
}