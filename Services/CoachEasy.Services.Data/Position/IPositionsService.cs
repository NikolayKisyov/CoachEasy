namespace CoachEasy.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using CoachEasy.Web.ViewModels;

    public interface IPositionsService
    {
        Task<PositionViewModel> GetPlayerAsync(string id);
    }
}
