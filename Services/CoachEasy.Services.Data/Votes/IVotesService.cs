namespace CoachEasy.Services.Data.Votes
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface IVotesService
    {
        Task SetVoteAsync(string coachId, string userId, int value);

        double GetAverageVotes(string coachId);
    }
}
