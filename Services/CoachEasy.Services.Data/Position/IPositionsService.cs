namespace CoachEasy.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using CoachEasy.Data.Models;
    using CoachEasy.Data.Models.Enums;
    using CoachEasy.Web.ViewModels;

    public interface IPositionsService
    {
        Task<PositionViewModel> GetPlayerAsync(string id);

        Position GetPositionByName(PositionName name);

        Position GetPositionById(string id);
    }
}
