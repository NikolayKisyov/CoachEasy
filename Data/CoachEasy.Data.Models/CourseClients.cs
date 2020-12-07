namespace CoachEasy.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using CoachEasy.Data.Common.Models;

    public class CourseClients : BaseDeletableModel<string>
    {
        public CourseClients()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string CourseId { get; set; }

        public virtual Course Course { get; set; }

        public string ClientId { get; set; }

        public virtual Client Client { get; set; }
    }
}
