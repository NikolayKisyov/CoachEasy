namespace CoachEasy.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "CoachEasy";

        public const string AdministratorRoleName = "Administrator";

        public const string CoachRoleName = "Coach";

        public const string ClientRoleName = "Client";

        public const string InvalidOperationExceptionWhileCreatingCoach = "Exception happened in CoachService while creating coach in IDeletableEntityRepository<Coach>";

        public const string InvalidOperationExceptionInPictureService = "Exception happened in PictureService while adding the Picture to IDeletableEntityRepository<Picture>";
    }
}
