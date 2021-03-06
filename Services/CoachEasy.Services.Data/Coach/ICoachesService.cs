﻿namespace CoachEasy.Services.Data.Coach
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using CoachEasy.Data.Models;
    using CoachEasy.Web.ViewModels;
    using CoachEasy.Web.ViewModels.Coaches;

    public interface ICoachesService
    {
        Task<bool> CreateCoachAsync(CreateCoachInputModel input, ApplicationUser user);

        Task<IEnumerable<T>> GetAllCoachesAsync<T>();

        Task<CoachViewModel> GetCoachById(string id);

        Coach GetCoachByUserId(string id);
    }
}
