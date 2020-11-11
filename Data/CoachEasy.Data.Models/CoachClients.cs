namespace CoachEasy.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class CoachClients
    {
        public CoachClients()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public string CoachId { get; set; }

        public virtual Coach Coach { get; set; }

        public string ClientId { get; set; }

        public virtual Client Client { get; set; }
    }
}
