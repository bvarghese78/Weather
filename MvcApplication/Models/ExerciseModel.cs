using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using System.Data;
using MvcApplication.Models.Definitions;
using System.Configuration;
using System.Threading.Tasks;

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

        public async Task<List<PastReps>> GetPastWorkouts(int exerciseID)
        {
            List<PastReps> ret = new List<PastReps>();
            if (mysql.State == ConnectionState.Open)
            {
                await mysql.CloseAsync();
                await mysql.OpenAsync();
            }   

            var command = mysql.CreateCommand();
            command.CommandText = @"select idExerciseIndividual, reps, weight, date, exercisename, notes, intensity from dailyreps inner join exerciseindividual on dailyreps.ExerciseIndividualid = exerciseindividual.idExerciseIndividual where ExerciseGroups_idExerciseGroups=@ei order by iddailyreps asc";
            command.Parameters.AddWithValue("@ei", exerciseID);

            var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                ret.Add(new PastReps(reader));
            }

            return ret;
        }
    }
}