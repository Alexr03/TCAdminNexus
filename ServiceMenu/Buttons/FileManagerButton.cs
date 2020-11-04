using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TCAdminNexus.Modules;
using TCAdminNexus.Objects;
using TCAdminNexus.Objects.Emulators;

namespace TCAdminNexus.ServiceMenu.Buttons
{
    public class FileManagerButton : ServiceMenuModule
    {
        public override void DefaultSettings()
        {
            Name = "File Manager Button";
            var attribute =
                new ActionCommandAttribute("File Manager", "Access server files", ":file_folder:",
                    new List<string> {"FileManager"},
                    true);
            Settings.ViewOrder = 5;
            Settings.ActionCommandAttribute = attribute;

            Configuration.SetConfiguration(Settings);
        }

        public override async System.Threading.Tasks.Task DoAction()
        {
            await base.DoAction();
            await CommandContext.TriggerTypingAsync();

            try
            {
                var tcaFileManager =
                    new TcaFileManager(CommandContext, Authentication, Authentication.Service.RootDirectory);
                await tcaFileManager.InitializeFileManagerAsync();
            }
            catch (Exception e)
            {
                await CommandContext.RespondAsync(
                    "**An error occurred when using the File Manager**");
                await CommandContext.RespondAsync(e.Message);
                await CommandContext.RespondAsync(e.StackTrace);
                await CommandContext.RespondAsync("EWGAWSG");
                await CommandContext.RespondAsync(e.InnerException.Message);
                await CommandContext.RespondAsync(e.InnerException.StackTrace);
                throw;
            }
        }
    }
}