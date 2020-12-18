namespace CoachEasy.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CoachEasy.Services.Data;
    using CoachEasy.Web.ViewModels.Players;
    using Microsoft.AspNetCore.Mvc;

    public class PlayerController : BaseController
    {
        private readonly IPlayersService playersService;

        public PlayerController(IPlayersService playersService)
        {
            this.playersService = playersService;
        }

        public async Task<IActionResult> All()
        {
            var viewModel = await this.playersService.GetAllPlayersAsync<PlayerViewModel>();

            return this.View(viewModel);
        }
    }
}
