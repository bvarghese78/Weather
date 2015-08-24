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

    /// <summary>
    /// This represents the Reps/weights for each exercise done in the past.
    /// </summary>
    public class PastReps
    {
        public int Id;
        public int Reps;
        public int? Weight;
        public DateTime Date;
        public string ExerciseName;

        public PastReps() { }
        public PastReps(IDataReader reader)
        {
            this.Id = Convert.ToInt32(reader["idExerciseIndividual"]);
            this.Reps = Convert.ToInt32(reader["Reps"]);
            this.Weight = reader["Weight"] is DBNull ? null : (int?)Convert.ToInt32(reader["Weight"]);
            this.Date = Convert.ToDateTime(reader["Date"]);
            this.ExerciseName = Convert.ToString(reader["ExerciseName"]);
        }
    }
}