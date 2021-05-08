using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace app.Controllers
{
    [Route("")]
    [Route("[controller]")]
    public class AppController : Controller
    {
        public IActionResult Movies()
        {
            return View();
        }
    }
}