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

    /// <summary>
    /// This represents today's exercise information
    /// </summary>
    public class DailyReps
    {
        public int id;
        public string ExerciseName;
        public int Reps;
        public int? Weight;
        public bool? Intensity;
        public DateTime Date;
        public int ProfileID;
        public int ExercisePhase;
        public int WorkoutID;

        public DailyReps() { }
        public DailyReps(IDataReader reader)
        {
            this.id = Convert.ToInt32(reader["idExerciseIndividual"]);
            this.ExerciseName = Convert.ToString(reader["ExerciseName"]);
            this.Reps = Convert.ToInt32(reader["Reps"]);
            this.Weight = reader["Weight"] is DBNull ? null : (int?)Convert.ToInt32(reader["Weight"]);
            this.Intensity = reader["Intensity"] is DBNull ? null : (bool?)Convert.ToBoolean(reader["Intensity"]);
            this.Date = Convert.ToDateTime(reader["Date"]);
            this.ProfileID = Convert.ToInt32(reader["profile_id"]);
            this.ExercisePhase = Convert.ToInt32(reader["ExercisePhaseID"]);
            this.WorkoutID = Convert.ToInt32(reader["WorkoutID"]);
        }
    }
}