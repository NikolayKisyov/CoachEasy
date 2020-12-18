namespace CoachEasy.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "CoachEasy";

        public const string AdministratorRoleName = "Administrator";

        public const string CoachRoleName = "Coach";

        public const string ClientRoleName = "Client";

        public const string DescriptionErrorMessage = "Description is required and its length should be at least 30 characters and 200 at max.";

        public const string SuccessfullyAddedWorkout = "Workout successfully added to your list!";

        public const string SuccessfullyAppliedToCourse = "You successfully applied to this course!";

        public const string WorkoutAlreadyAdded = "You've already added this workout to your list.";

        public const string InvalidWorkout = "Please select a valid workout to add.";

        public const string InvalidOperationExceptionInWorkoutForEditSearch = "Exception happened in IWorkoutsService while trying to find the workout you want to edit";

        public const string InvalidOperationExceptionWhileCreatingCoach = "Exception happened in CoachService while creating coach in IDeletableEntityRepository<Coach>";

        public const string InvalidOperationExceptionWhileCreatingClient = "Exception happened in ClientService while creating client in IDeletableEntityRepository<Client>";

        public const string InvalidOperationExceptionInClientsService = "Exception happened in ClientsService while adding Workout to Client WorkoutList";

        public const string SystemEmail = "bjergsenpai23@gmail.com";

        public const string FromEmail = "coacheasy@abv.bg";
    }
}
