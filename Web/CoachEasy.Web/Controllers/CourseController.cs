namespace CoachEasy.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CoachEasy.Common;
    using CoachEasy.Data.Models;
    using CoachEasy.Services.Data.Coach;
    using CoachEasy.Services.Data.Course;
    using CoachEasy.Web.ViewModels.Coaches;
    using CoachEasy.Web.ViewModels.Courses;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class CourseController : BaseController
    {
        private readonly ICoachesService coachesService;
        private readonly ICoursesService coursesService;
        private readonly UserManager<ApplicationUser> userManager;

        public CourseController(
            ICoachesService coachesService,
            ICoursesService coursesService,
            UserManager<ApplicationUser> userManager)
        {
            this.coachesService = coachesService;
            this.coursesService = coursesService;
            this.userManager = userManager;
        }

        [Authorize(Roles = "Coach")]
        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new CreateCourseInputModel();
            return this.View(viewModel);
        }

        [Authorize(Roles = "Coach")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateCourseInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var user = await this.userManager.GetUserAsync(this.User);
            await this.coursesService.CreateCourseAsync(input, user.Id);

            return this.Redirect("/");
        }

        [HttpGet]
        public async Task<IActionResult> All(int id = 1)
        {
            if (id <= 0)
            {
                return this.NotFound();
            }

            var user = await this.userManager.GetUserAsync(this.User);

            var userId = user != null ? user.Id : string.Empty;

            const int ItemsPerPage = 9;
            var viewModel = new CoursesListViewModel
            {
                ItemsPerPage = ItemsPerPage,
                PageNumber = id,
                CoursesCount = this.coursesService.GetCount(),
                Courses = await this.coursesService.GetAll(userId, id, ItemsPerPage),
            };

            return this.View(viewModel);
        }

        [Authorize(Roles = GlobalConstants.ClientRoleName)]
        [HttpGet]
        public async Task<IActionResult> ApplyToCourse(string id)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var result = await this.coursesService.AddClientToCourse(id, user.Id);

            if (result == "created")
            {
                this.TempData["SuccessMessage"] = GlobalConstants.SuccessfullyAppliedToCourse;
                return this.RedirectToAction("SuccessfullyAdded");
            }

            return this.RedirectToAction("All");
        }

        [HttpGet]
        public IActionResult SuccessfullyAdded()
        {
            return this.View();
        }
    }
}
