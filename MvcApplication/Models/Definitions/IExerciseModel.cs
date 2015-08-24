using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcApplication.Models.Definitions
{
    public interface IExerciseModel
    {
        List<Workout> GetWorkout();
    }
}
