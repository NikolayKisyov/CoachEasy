namespace CoachEasy.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CoachEasy.Common;
    using CoachEasy.Data.Common.Repositories;
    using CoachEasy.Data.Models;
    using CoachEasy.Services.Data.Client;
    using CoachEasy.Web.ViewModels.Players;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class ClientController : BaseController
    {
        private readonly IClientsService clientsService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDeletableEntityRepository<Workout> cRep;

        public ClientController(
            IClientsService clientsService,
            UserManager<ApplicationUser> userManager,
            IDeletableEntityRepository<Workout> cRep)
        {
            this.clientsService = clientsService;
            this.userManager = userManager;
            this.cRep = cRep;
        }

        [HttpGet]
        public async Task<IActionResult> AddToWorkoutList(string id)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var result = await this.clientsService.AddWorkoutToClientList(id, user.Id);

            if (result == true)
            {
                this.TempData["SuccessMessage"] = GlobalConstants.SuccessfullyAddedWorkout;
            }
            else
            {
                this.TempData["ErrorMessage"] = GlobalConstants.WorkoutAlreadyAdded;
            }

            return this.RedirectToAction("All", "Workout");
        }
    }
}
