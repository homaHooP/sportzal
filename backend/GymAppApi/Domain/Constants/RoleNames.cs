namespace GymAppApi.Domain.Constants
{
    public static class RoleNames
    {
        public const string Client = "Client";
        public const string Trainer = "Trainer";
        public const string Manager = "Manager";
        public const string HeadManager = "HeadManager";

        public static readonly string[] All = { Client, Trainer, Manager, HeadManager };
    }
}