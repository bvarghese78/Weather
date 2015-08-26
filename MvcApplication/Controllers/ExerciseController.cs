using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication.Models;
using MvcApplication.Models.Definitions;
using System.Threading.Tasks;

namespace MvcApplication.Controllers
{
    public class ExerciseController : Controller
    {
        ExerciseModel model = new ExerciseModel();
        List<Workout> workoutType = new List<Workout>();
        Dictionary<int, string> currentExerciseDict = new Dictionary<int, string>();

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
            // save a set of workout id and name so we can use it at other places
            foreach(var entry in workouts)
            {
                currentExerciseDict.Add(entry.Id, entry.WorkoutName);
            }
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
            ViewBag.CurrentExerciseType = "Shoulders & Arms";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        [HttpPost]
        public async Task<ActionResult> WorkoutChosen(string id)
        {
            TempGetWorkout();
            //List<PastReps> pastWorkoutResults = await model.GetPastWorkouts(Convert.ToInt32(id));

            Dictionary<DateTime, List<PastReps>> d = new Dictionary<DateTime, List<PastReps>>();
            List<PastReps> p = new List<PastReps>();
            p.Add(new PastReps());
            p[0].Date = new DateTime(2015, 8, 1);
            p[0].Id = 1;
            p[0].Reps = 8;
            p[0].Weight = null;
            p[0].ExerciseName = "Standarad Push Ups";
            p.Add(new PastReps());
            p[1].Date = new DateTime(2015, 8, 1);
            p[1].Id = 1;
            p[1].Reps = 10;
            p[1].Weight = null;
            p[1].ExerciseName = "Wide Fly Pull Ups";
            p.Add(new PastReps());
            p[2].Date = new DateTime(2015, 8, 1);
            p[2].Id = 1;
            p[2].Reps = 12;
            p[2].Weight = null;
            p[2].ExerciseName = "Military Pushups";
            p.Add(new PastReps());
            p[3].Date = new DateTime(2015, 8, 8);
            p[3].Id = 1;
            p[3].Reps = 10;
            p[3].Weight = null;
            p[3].ExerciseName = "Standarad Push Ups";
            p.Add(new PastReps());
            p[4].Date = new DateTime(2015, 8, 8);
            p[4].Id = 1;
            p[4].Reps = 10;
            p[4].Weight = null;
            p[4].ExerciseName = "Wide Fly Pull Ups";
            p.Add(new PastReps());
            p[5].Date = new DateTime(2015, 8, 8);
            p[5].Id = 1;
            p[5].Reps = 15;
            p[5].Weight = null;
            p[5].ExerciseName = "Military Pushups";

            Dictionary<DateTime, List<PastReps>> dict = new Dictionary<DateTime, List<PastReps>>();
            List<PastReps> tempPastReps = new List<PastReps>();
            DateTime tempDate = DateTime.MinValue;
            int counter = 0;

            // Takes the item in PastWorkoutResult and put it in a dictionary with date as the key and the values are the exercises done for that date.
            foreach (var item in p) // Change "p" to PastWorkoutResults
            {
                counter++;
                if (tempDate != item.Date || counter == p.Count)  // Change "p" to PastWorkoutResults
                {
                    if(tempDate == DateTime.MinValue)
                        tempDate = item.Date;
                    else
                    {
                        if (counter == p.Count)   // Change "p" to PastWorkoutResults
                            tempPastReps.Add(item);

                        dict.Add(tempDate, tempPastReps);
                        tempPastReps = new List<PastReps>();
                        tempDate = item.Date.Date;
                    }
                }

                tempPastReps.Add(item);
            }

            ViewBag.CurrentExerciseGroup = "Chest & Back";

            PopulateNewEntry();
            return View(dict);
        }

        private void PopulateNewEntry()
        {
            if(ViewBag.exerciseName == "")
                ViewBag.exerciseName = "Standard Pushups";
            else
            {
                var currentName = ViewBag.exerciseName;
                var key = currentExerciseDict.FirstOrDefault(x => x.Value == currentName).Key;
                ViewBag.exerciseName = currentExerciseDict.FirstOrDefault(x => x.Key == key + 1);
            }
        }
    }
}
