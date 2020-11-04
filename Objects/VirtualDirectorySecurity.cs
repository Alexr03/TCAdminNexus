using System.Linq;
using TCAdmin.SDK.Misc;
using TCAdmin.SDK.Objects;
using TCAdmin.SDK.VirtualFileSystem;
using TCAdminNexus.Exceptions;
using TCAdminNexus.Helpers;
using Permission = TCAdmin.SDK.VirtualFileSystem.Permission;

namespace TCAdminNexus.Objects
{
    public class VirtualDirectorySecurity
    {
        public VirtualDirectorySecurity(TCAdmin.SDK.Web.References.FileSystem.FileSystem fileSystem,
            string currentDirectory, UserType type, int gameId)
        {
            UserType = type;
            if (!fileSystem.DirectoryExists(currentDirectory))
                throw new CustomMessageException(EmbedTemplates.CreateErrorEmbed("Could not find directory."));

            var game = new TCAdmin.GameHosting.SDK.Objects.Game(gameId);

            var ds = new TCAdmin.SDK.VirtualFileSystem.VirtualDirectorySecurity
            {
                PermissionMode = PermissionMode.Basic,
                Permissions = Permission.Read | Permission.Write | Permission.Delete,
                PermissionType = PermissionType.Root,
                RootPhysicalPath = currentDirectory + "\\",
                RealPhysicalPath =
                    TCAdmin.SDK.Misc.FileSystem.FixAbsoluteFilePath(currentDirectory, OperatingSystem.Windows),
                DisableOwnerCheck = true,
                DisableSymlinkCheck = true,
                VirtualDirectoryName = currentDirectory
            };

            if (type == UserType.SubAdmin)
            {
                var f = game.FileSystemPermissions.FirstOrDefault(x => x.RootPhysicalPath == "$[SubAdminFiles]");
                ds.AdditionalPermissions = f.AdditionalPermissions;
                ds.Filters = f.Filters;
            }

            if (type == UserType.User)
            {
                var f = game.FileSystemPermissions.FirstOrDefault(x => x.RootPhysicalPath == "$[UserFiles]");
                ds.AdditionalPermissions = f.AdditionalPermissions;
                ds.Filters = f.Filters;
            }

            VirtualDirectorySecurityObject = ds;
            VirtualDirectorySecurityString = ObjectXml.ObjectToXml(ds);
        }

        public VirtualDirectorySecurity(string currentDirectory)
        {
            UserType = UserType.Admin;
            var ds = new TCAdmin.SDK.VirtualFileSystem.VirtualDirectorySecurity
            {
                PermissionMode = PermissionMode.Basic,
                Permissions = Permission.Read | Permission.Write | Permission.Delete,
                PermissionType = PermissionType.Root,
                RootPhysicalPath = currentDirectory + "\\",
                DisableOwnerCheck = true,
                DisableSymlinkCheck = true
            };

            VirtualDirectorySecurityObject = ds;
            VirtualDirectorySecurityString = ObjectXml.ObjectToXml(ds);
        }

        public UserType UserType { get; }

        public TCAdmin.SDK.VirtualFileSystem.VirtualDirectorySecurity VirtualDirectorySecurityObject { get; }

        public string VirtualDirectorySecurityString { get; }
    }
}