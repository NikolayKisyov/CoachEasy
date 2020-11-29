namespace CoachEasy.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CoachEasy.Services.Data;
    using Microsoft.AspNetCore.Mvc;

    public class PositionController : BaseController
    {
        private readonly IPositionsService positionsService;

        public PositionController(IPositionsService positionsService)
        {
            this.positionsService = positionsService;
        }

        public async Task<IActionResult> Position(string id)
        {
            var viewModel = await this.positionsService.GetPlayerAsync(id);

            return this.View(viewModel);
        }
    }
}
