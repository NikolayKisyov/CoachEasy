namespace CoachEasy.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using CoachEasy.Data.Common.Models;

    public class CoachClients : BaseDeletableModel<string>
    {
        public CoachClients()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string CoachId { get; set; }

        public virtual Coach Coach { get; set; }

        public string ClientId { get; set; }

        public virtual Client Client { get; set; }
    }
}
