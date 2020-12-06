namespace CoachEasy.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CoachEasy.Data.Models;
    using CoachEasy.Services.Data.Coach;
    using CoachEasy.Services.Data.Workout;
    using CoachEasy.Web.ViewModels.Workouts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class WorkoutController : BaseController
    {
        public const int ItemsPerPage = 9;

        private readonly IWorkoutsService workoutsService;
        private readonly UserManager<ApplicationUser> userManager;

        public WorkoutController(
            IWorkoutsService workoutsService,
            UserManager<ApplicationUser> userManager)
        {
            this.workoutsService = workoutsService;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult All(SearchWorkoutInputModel inputModel, int id = 1)
        {
            if (id <= 0)
            {
                return this.NotFound();
            }

            var result = this.workoutsService.GetSearchedPositions<WorkoutInListViewModel>(inputModel, id, ItemsPerPage);

            //if (inputModel != null)
            //{   
            var viewModel = new ListOfWorkoutsViewModel
                {
                    ItemsPerPage = ItemsPerPage,
                    PageNumber = id,
                    WorkoutsCount = result.Count,
                    Workouts = result.Workouts,
                    PointGuard = inputModel.PointGuard,
                    ShootingGuard = inputModel.ShootingGuard,
                    SmallForward = inputModel.SmallForward,
                    PowerForward = inputModel.PowerForward,
                    Center = inputModel.Center,
                };

            //}
            //else
            //{
            //    viewModel = new ListOfWorkoutsViewModel
            //    {
            //        ItemsPerPage = ItemsPerPage,
            //        PageNumber = id,
            //        WorkoutsCount = this.workoutsService.GetCount(),
            //        Workouts = this.workoutsService.GetAll<WorkoutInListViewModel>(id, ItemsPerPage),
            //    };
            //}

            return this.View(viewModel);
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
            await this.workoutsService.CreateAsync(input, user.Id);

            return this.RedirectToAction("All");
        }

        [HttpGet]
        [Authorize(Roles = "Coach")]
        public IActionResult Edit(string id)
        {
            var result = this.workoutsService.GetWorkoutForEdit(id);

            return this.View(result);
        }

        [HttpPost]
        [Authorize(Roles = "Coach")]
        public async Task<IActionResult> Edit(EditWorkoutViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.workoutsService.EditAsync(input);
            return this.RedirectToAction("All");
        }

        [Authorize(Roles = "Coach")]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            await this.workoutsService.DeleteAsync(id);
            return this.RedirectToAction("All");
        }
    }
}
