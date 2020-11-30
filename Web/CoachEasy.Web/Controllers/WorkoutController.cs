namespace CoachEasy.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CoachEasy.Data.Models;
    using CoachEasy.Services.Data.Workout;
    using CoachEasy.Web.ViewModels.Workouts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class WorkoutController : BaseController
    {
        private readonly IWorkoutsService workoutsService;
        private readonly UserManager<ApplicationUser> userManager;

        public WorkoutController(
            IWorkoutsService workoutsService,
            UserManager<ApplicationUser> userManager)
        {
            this.workoutsService = workoutsService;
            this.userManager = userManager;
        }

        [Authorize(Roles = "Coach")]
        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new CreateWorkoutInputModel();
            return this.View(viewModel);
        }

        [Authorize(Roles = "Coach")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateWorkoutInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var user = await this.userManager.GetUserAsync(this.User);

            await this.workoutsService.CreateWorkoutAsync(input, user.Id);

            return this.Redirect("/");
        }

        [HttpGet]
        public IActionResult All()
        {
            var viewModel = this.workoutsService.GetAll<WorkoutViewModel>();
            return this.View(viewModel);
        }
    }
}
