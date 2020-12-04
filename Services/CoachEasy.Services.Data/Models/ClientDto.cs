namespace CoachEasy.Services.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using CoachEasy.Data.Models;
    using CoachEasy.Web.ViewModels;

    public class ClientDto
    {
        public string Id { get; set; }

        public ICollection<WorkoutsList> WorkoutsList { get; set; }
    }
}
