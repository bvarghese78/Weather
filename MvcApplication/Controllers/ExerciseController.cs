using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication.Models;
using MvcApplication.Models.Definitions;

namespace MvcApplication.Controllers
{
    public class ExerciseController : Controller
    {
        ExerciseModel model = new ExerciseModel();

        //
        // GET: /Exercise/

        public ActionResult Index()
        {
            GetWorkout();
            return View();
        }

        // Get Workout Types from DB and populate to a combo box
        private void GetWorkout()
        {
            var workouts = model.GetWorkout();

            List<SelectListItem> items = new List<SelectListItem>(); // This list allows you to add items to a combo box ad asign text name and value
            foreach (var item in workouts)
            {
                items.Add(new SelectListItem { Text = item.WorkoutName, Value = Convert.ToString(item.Id) });
            }

            ViewBag.id = items;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        [HttpPost]
        public void WorkoutChosen(string id)
        {
            ViewBag.Attempts = id;
        }
    }
}
