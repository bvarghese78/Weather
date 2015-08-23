using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace MvcApplication.Models.Definitions
{
    /// <summary>
    /// This represents the Workout table within the DB
    /// </summary>
    public class Workout
    {
        /// <summary>
        /// This is the unique identifier for the workout
        /// </summary>
        public int Id;
        /// <summary>
        /// This is the name of the workout type
        /// </summary>
        public string WorkoutName;

        public Workout(IDataReader reader)
        {
            this.Id = Convert.ToInt32(reader["idExerciseGroups"]);
            this.WorkoutName = Convert.ToString(reader["WorkoutName"]);
        }
    }
}