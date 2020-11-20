namespace CoachEasy.Web.Controllers
{
    using System.Diagnostics;
    using System.Linq;

    using CoachEasy.Data;
    using CoachEasy.Data.Common.Repositories;
    using CoachEasy.Data.Models;
    using CoachEasy.Data.Models.Enums;
    using CoachEasy.Web.ViewModels;

    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly ApplicationDbContext db;
        private readonly IDeletableEntityRepository<Position> positions;

        public HomeController(ApplicationDbContext db, IDeletableEntityRepository<Position> positions)
        {
            this.db = db;
            this.positions = positions;
        }

        public IActionResult Index()
        {
            var viewModel = this.db.Players.Select(x => new PlayerViewModel
            {
                Name = x.Name,
                PositionName = x.Position.Name,
                TeamName = x.TeamName,
                Championships = x.Championships,
                Height = x.Height,
                Weight = x.Weight,
                Experience = x.Experience,
                ImageUrl = x.ImageUrl,
            }).ToList();

            return this.View(@"/Views/Home/IndexLoggedIn.cshtml", viewModel);
        }

        public IActionResult Position()
        {
            var viewModel = this.db.Players.Where(x => x.Name == "Stephen Curry").Select(x => new PlayerViewModel
            {
                Name = x.Name,
                PositionName = x.Position.Name,
                TeamName = x.TeamName,
                Championships = x.Championships,
                Height = x.Height,
                Weight = x.Weight,
                Experience = x.Experience,
                ImageUrl = x.ImageUrl,
            }).First();

            return this.View(@"/Views/Position/Position.cshtml", viewModel);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}

