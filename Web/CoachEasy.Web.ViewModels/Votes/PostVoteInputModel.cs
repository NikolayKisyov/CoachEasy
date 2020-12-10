namespace CoachEasy.Web.ViewModels.Votes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class PostVoteInputModel
    {
        public string CoachId { get; set; }

        [Range(1, 5)]
        public int Value { get; set; }
    }
}
