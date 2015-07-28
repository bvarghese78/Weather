using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication.Models;

namespace MvcApplication.Controllers
{
    public class ExerciseController : Controller
    {
        //
        // GET: /Exercise/

        public ActionResult Index()
        {
            return View();
        }

    }
}
