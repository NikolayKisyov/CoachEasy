namespace CoachEasy.Web.ViewModels.Workouts
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ListOfWorkoutsViewModel : PagingViewModel
    {
        public IEnumerable<WorkoutInListViewModel> Workouts { get; set; }
    }
}
