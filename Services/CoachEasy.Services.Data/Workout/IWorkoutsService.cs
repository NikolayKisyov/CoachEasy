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
        (IEnumerable<T> Workouts, int Count) GetSearchedPositions<T>(SearchWorkoutInputModel inputModel, int page, int itemsPerPage);

        int GetCount();

        Task CreateAsync(CreateWorkoutInputModel input, string userId);

        EditWorkoutViewModel GetWorkoutForEdit(string id);

        Task EditAsync(EditWorkoutViewModel input);

        Task DeleteAsync(string id);

        Workout GetWorkoutById(string id);
    }
}
