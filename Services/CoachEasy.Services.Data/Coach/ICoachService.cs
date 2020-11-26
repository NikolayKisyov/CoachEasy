﻿namespace CoachEasy.Services.Data.Coach
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using CoachEasy.Web.ViewModels;

    public interface ICoachService
    {
        Task<bool> CreateCoachAsync(CreateCoachInputModel input);
    }
}