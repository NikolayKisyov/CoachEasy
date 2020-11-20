namespace CoachEasy.Data.Seeding.WorkoutsSeeding
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class PositionWorkoutJsonDTO
    {
        public string Position { get; set; }

        public ICollection<WorkoutJsonDTO> Workouts { get; set; }
    }
}
