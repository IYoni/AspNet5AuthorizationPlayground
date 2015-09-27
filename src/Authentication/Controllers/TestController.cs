using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;

namespace Authentication.Controllers
{
    public class TestController : Controller
    {
        [Authorize(Policy = "SalesOnly")]
        public IActionResult SalesOnly()
        {
            return View("success");
        }

        [Authorize(Policy = "SalesSenior")]
        public IActionResult SalesSenior()
        {
            return View("success");
        }

        [Authorize(Policy = "DevInterns")]
        public IActionResult DevIntern()
        {
            return View("success");
        }

        [Authorize(Roles = "NoOneHasAccess")]
        public IActionResult NoOneHasAccess()
        {
            return View("success");
        }
    }
}