using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using System.Data;
using MvcApplication.Models.Definitions;
using System.Configuration;

namespace MvcApplication.Models
{
    public class ExerciseModel : IExerciseModel
    {
        private MySqlConnection mysql = new MySqlConnection(ConfigurationManager.ConnectionStrings["benjiJeen"].ConnectionString);
        public List<Workout> GetWorkout()
        {
            List<Workout> ret = new List<Workout>();
            mysql.Open();

            var command = mysql.CreateCommand();
            command.CommandText = "select * from workouts";

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                ret.Add(new Workout(reader));
            }

            return ret;
        }

        public List<PastReps> GetPastWorkouts(int exerciseID)
        {
            List<PastReps> ret = new List<PastReps>();
            mysql.Open();

            var command = mysql.CreateCommand();
            command.CommandText = @"select idExerciseIndividual, reps, weight, date, exercisename from dailyreps inner join exerciseindividual on dailyreps.idExerciseIndividual = exerciseindividual.idExerciseIndividual where idExerciseIndividual=@ei";
            command.Parameters.AddWithValue("@ei", exerciseID);

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                ret.Add(new PastReps(reader));
            }

            return ret;
        }
    }
}