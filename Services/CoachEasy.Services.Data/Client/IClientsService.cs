namespace CoachEasy.Services.Data.Client
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using CoachEasy.Data.Models;
    using CoachEasy.Services.Data.Models;
    using CoachEasy.Web.ViewModels;
    using CoachEasy.Web.ViewModels.Workouts;

    public interface IClientsService
    {
        int GetCount(string userId);

        ClientDto GetClient(string userId);

        Task<IEnumerable<WorkoutInListViewModel>> GetWorkouts(string userId, int page, int itemsPerPage);

        Task<bool> CreateClientAsync(CreateClientInputModel input, ApplicationUser user);

        Task<bool> AddWorkoutToClientList(string id, string userId);

        Task Delete(string workoutId, string userId);
    }
}
