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
        Dictionary<int, string> currentWorkoutDict = new Dictionary<int, string>();

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
            //var workouts = model.GetWorkout();

            /* Improvements */
            workoutType = model.GetWorkout();

            // save a set of workout id and name so we can use it at other places
            foreach(var entry in workoutType)
            {
                currentWorkoutDict.Add(entry.Id, entry.WorkoutName);
            }
            /* Improvements */

            List<SelectListItem> items = new List<SelectListItem>(); // This list allows you to add items to a combo box ad asign text name and value
            foreach (var item in workoutType)
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
        public async Task<ActionResult> WorkoutChosen(string id)
        {
            GetWorkout(); // Populate the combo box
            List<PastReps> pastWorkoutResults = await model.GetPastWorkouts(Convert.ToInt32(id));

            Dictionary<DateTime, List<PastReps>> dict = new Dictionary<DateTime, List<PastReps>>();
            List<PastReps> tempPastReps = new List<PastReps>();
            DateTime tempDate = DateTime.MinValue;
            int counter = 0;

            // Takes the item in PastWorkoutResult and put it in a dictionary with date as the key and the values are the exercises done for that date.
            foreach (var item in pastWorkoutResults)
            {
                counter++;
                if (tempDate != item.Date || counter == pastWorkoutResults.Count)
                {
                    if(tempDate == DateTime.MinValue)
                        tempDate = item.Date;
                    else
                    {
                        if (counter == pastWorkoutResults.Count)
                            tempPastReps.Add(item);

                        dict.Add(tempDate, tempPastReps);
                        tempPastReps = new List<PastReps>();
                        tempDate = item.Date.Date;
                    }
                }

                tempPastReps.Add(item);
            }

            string currExerciseName;
            currentWorkoutDict.TryGetValue(Convert.ToInt32(id), out currExerciseName);
            ViewBag.CurrentExerciseGroup = currExerciseName;

            PopulateNewEntry(pastWorkoutResults);
            return View(dict);
        }

        // Not complete. Sanity checks
        private void PopulateNewEntry(List<PastReps> ExerciseNames)
        {
            if (ViewBag.exerciseName == null)
                ViewBag.exerciseName = ExerciseNames[0].ExerciseName;
            else
            {
                var currentName = ViewBag.exerciseName;
                for(int i = 0; i < ExerciseNames.Count; i++)
                {
                    if(ExerciseNames[i].ExerciseName== currentName)
                    {
                        ViewBag.exerciseName = ExerciseNames[i + 1].ExerciseName;
                    }
                }
            }
        }
    }
}
