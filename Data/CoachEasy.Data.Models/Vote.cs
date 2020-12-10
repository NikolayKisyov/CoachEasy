namespace CoachEasy.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using CoachEasy.Data.Common.Models;

    public class Vote : BaseModel<int>
    {
        public string CoachId { get; set; }

        public virtual Coach Cach { get; set; }

        public string ClientId { get; set; }

        public virtual Client Client { get; set; }

        public byte Value { get; set; }
    }
}
