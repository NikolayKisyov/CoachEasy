namespace CoachEasy.Web.ViewModels.Coaches
{
    using CoachEasy.Data.Models;
    using CoachEasy.Services.Mapping;

    public class CoachViewModel : IMapFrom<Coach>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int Experience { get; set; }

        public string Description { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string PictureUrl { get; set; }

        public double AverageVote { get; set; }

        public int CoachWorkoutsCount { get; set; }

        public int CoursesCount { get; set; }
    }
}
