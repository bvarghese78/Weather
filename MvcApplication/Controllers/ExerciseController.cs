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
        List<Workout> workoutType = new List<Workout>();

        //
        // GET: /Exercise/

        public ActionResult Index()
        {
            //GetWorkout();
            TempGetWorkout();
            return View();
        }

        // Get Workout Types from DB and populate to a combo box
        private void GetWorkout()
        {
            var workouts = model.GetWorkout();
            /* Improvements */
            if (workoutType.Count == 0)
                workoutType = model.GetWorkout();
            /* Improvements */

            List<SelectListItem> items = new List<SelectListItem>(); // This list allows you to add items to a combo box ad asign text name and value
            foreach (var item in workouts)
            {
                items.Add(new SelectListItem { Text = item.WorkoutName, Value = Convert.ToString(item.Id) });
            }

            ViewBag.id = items;
        }

        private void TempGetWorkout()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Chest & Back", Value = "1" });
            items.Add(new SelectListItem { Text = "Plyometrics", Value = "2" });
            items.Add(new SelectListItem { Text = "Shoulders & Arms", Value = "3" });

            ViewBag.id = items;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        [HttpPost]
        public ActionResult WorkoutChosen(string id)
        {
            TempGetWorkout();
            var pastWorkoutResults = model.GetPastWorkouts(Convert.ToInt32(id));
            //List<PastReps> p = new List<PastReps>();
            //p.Add(new PastReps());
            //p[0].Date = new DateTime(2015, 8, 1);
            //p[0].Id = 1;
            //p[0].Reps = 8;
            //p[0].Weight = null;
            //p[0].ExerciseName = "Standarad Push Ups";
            //p.Add(new PastReps());
            //p[1].Date = new DateTime(2015, 8, 8);
            //p[1].Id = 1;
            //p[1].Reps = 10;
            //p[1].Weight = null;
            //p[1].ExerciseName = "Wide Fly Pull Ups";
            //p.Add(new PastReps());
            //p[2].Date = new DateTime(2015, 8, 15);
            //p[2].Id = 1;
            //p[2].Reps = 12;
            //p[2].Weight = null;
            //p[2].ExerciseName = "Military Pushups";

            ViewBag.CurrentExerciseGroup = "Chest & Back";
            return View(p);
        }
    }
}
