using Alexr03.Common.TCAdmin.Permissions;

namespace TCAdminNexus
{
    public static class Globals
    {
        public const string ModuleId = "6188e2ee-72f8-445d-9951-bce13326a6a0";

        public static readonly PermissionManager<NexusPermissions> NexusPermissionsManager =
            new PermissionManager<NexusPermissions>(ModuleId);
    }

    public enum NexusPermissions
    {
        ViewStatistics = 1,
        ViewPowerStatus,
        StartStop,
        EditConfiguration,
        ViewLogs
    }
}