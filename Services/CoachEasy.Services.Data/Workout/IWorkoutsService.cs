namespace CoachEasy.Services.Data.Workout
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using CoachEasy.Data.Models;
    using CoachEasy.Web.ViewModels.Workouts;

    public interface IWorkoutsService
    {
        ICollection<T> GetAll<T>();

        Task CreateWorkoutAsync(CreateWorkoutInputModel input, string userId);
    }
}
