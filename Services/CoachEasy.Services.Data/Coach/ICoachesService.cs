namespace CoachEasy.Services.Data.Coach
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using CoachEasy.Data.Models;
    using CoachEasy.Web.ViewModels;

    public interface ICoachesService
    {
        Task<bool> CreateCoachAsync(CreateCoachInputModel input, ApplicationUser user);
    }
}
