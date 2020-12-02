namespace CoachEasy.Services.Data.Client
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using CoachEasy.Data.Models;
    using CoachEasy.Web.ViewModels;

    public interface IClientsService
    {
        Task<bool> CreateClientAsync(CreateClientInputModel input, ApplicationUser user);

        Task<bool> AddWorkoutToClientList(string id, string userId);

        Client GetClientById(string userId);
    }
}
