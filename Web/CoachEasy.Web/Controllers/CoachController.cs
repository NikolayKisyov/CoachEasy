namespace CoachEasy.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CoachEasy.Services.Data.Coach;
    using CoachEasy.Web.ViewModels.Coaches;
    using Microsoft.AspNetCore.Mvc;

    public class CoachController : BaseController
    {
        private readonly ICoachesService coachesService;

        public CoachController(ICoachesService coachesService)
        {
            this.coachesService = coachesService;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var viewModel = await this.coachesService.GetAllCoachesAsync<CoachViewModel>();

            return this.View(viewModel);
        }
    }
}
