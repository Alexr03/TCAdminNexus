using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Alexr03.Common.Configuration;
using Alexr03.Common.TCAdmin.Configuration;
using Alexr03.Common.TCAdmin.Proxy;
using Alexr03.Common.Web.Attributes.ActionFilters;
using DSharpPlus.Entities;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using TCAdmin.SDK.Misc;
using TCAdmin.SDK.VirtualFileSystem;
using TCAdmin.SDK.Web.FileManager;
using TCAdmin.SDK.Web.MVC.Controllers;
using TCAdmin.Web.MVC;
using TCAdminNexus.Configurations;
using TCAdminNexus.Models;
using TCAdminNexus.Proxy.Requests.Bot;
using TCAdminNexus.Proxy.Requests.Discord;
using JsonNetResult = Alexr03.Common.Web.HttpResponses.JsonNetResult;

namespace TCAdminNexus.Controllers
{
    [ExceptionHandler]
    [RequestActionLog("Nexus", true)]
    public class NexusController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [ParentAction("Index")]
        public ActionResult Guilds()
        {
            return View();
        }

        [ParentAction("Index")]
        public ActionResult Guild(ulong guildId)
        {
            var guild = new GetGuild().Request<DiscordGuild>(guildId, out _, ProxyRequestType.Json);
            return new JsonNetResult(guild);
        }

        [ParentAction("Index")]
        public ActionResult Test()
        {
            return View();
        }

        [ParentAction("Index")]
        public ActionResult Members(ulong guildId) 
        {
            var members = new GetGuildMembers().Request<List<GetGuildMembers>>(guildId, out _, ProxyRequestType.Json);
            return new JsonNetResult(members);
        }

        public ActionResult Statistics()
        {
            Globals.NexusPermissionsManager.ThrowIfCurrentUserLackPermission(NexusPermissions.ViewStatistics);

            var model = new NexusViewModel
            {
                Statistics = new GetStatistics().Request<GetStatistics>(null, out _),
                Power = new GetPower().Request<GetPower>(null, out _),
            };

            return View("_Statistics", model);
        }

        public ActionResult Logs()
        {
            Globals.NexusPermissionsManager.ThrowIfCurrentUserLackPermission(NexusPermissions.ViewLogs);

            var server = TCAdmin.GameHosting.SDK.Objects.Server.GetServerFromCache(1);
            var files = server.FileSystemService.FindFiles(server.ServerUtilitiesService.GetMonitorLogsDirectory(),
                "*-nexus.log",
                false).OrderByDescending(x => x.LastWriteTime);
            var remoteTail = new RemoteTail(server, new VirtualDirectorySecurity(),
                files.First().FullName, "Console Log", string.Empty, string.Empty);
            return Content(
                $"<iframe src='{remoteTail.GetUrl()}' style='height: 1000px; width: 100%;'>Your browser doesn't support iframes.</iframe>");
        }

        [HttpPost]
        public ActionResult Power(NexusPowerRequest request)
        {
            Globals.NexusPermissionsManager.ThrowIfCurrentUserLackPermission(NexusPermissions.StartStop);

            var server = TCAdmin.GameHosting.SDK.Objects.Server.GetServerFromCache(1);
            var fileSystem = server.FileSystemService;
            var commandPath = FileSystem.CombinePath(server.ServerUtilitiesService.GetMonitorDirectory(), "command.do",
                server.OperatingSystem);
            fileSystem.CreateTextFile(commandPath, Encoding.Default.GetBytes("service nexus " + request));
            return Json(new
            {
                Message = $"Nexus power command <strong>{request.ToString()}</strong> has been sent. Check logs."
            });
        }

        public ActionResult Configuration()
        {
            Globals.NexusPermissionsManager.ThrowIfCurrentUserLackPermission(NexusPermissions.EditConfiguration);

            var model = new DatabaseConfiguration<BotConfiguration>(Globals.ModuleId, nameof(BotConfiguration)).GetConfiguration();
            return PartialView("_Configuration", model);
        }

        [HttpPost]
        public ActionResult Configuration(BotConfiguration model)
        {
            Globals.NexusPermissionsManager.ThrowIfCurrentUserLackPermission(NexusPermissions.EditConfiguration);
            new DatabaseConfiguration<BotConfiguration>(Globals.ModuleId, nameof(BotConfiguration)).SetConfiguration(model);
            return PartialView("_Configuration", model);
        }

        public ActionResult GuildsRead([DataSourceRequest] DataSourceRequest request)
        {
            var discordGuilds = new GetGuilds().Request<List<DiscordGuild>>(null, out _, ProxyRequestType.Json);
            var guildModels = discordGuilds.Select(DiscordGuildModel.Map).ToList();

            return new JsonNetResult(guildModels.ToDataSourceResult(request), behavior: JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuildsDestroy(DiscordGuildModel model, [DataSourceRequest] DataSourceRequest request)
        {
            Console.WriteLine("Attempting to leave - " + model.Id);
            var leaveGuild = new LeaveGuild().Request<bool>(model.Id, out _, ProxyRequestType.Json);
            if (leaveGuild)
            {
                return Json(new[] {model}.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            ModelState.AddModelError("LeaveError", "Could not leave the guild");
            return new JsonNetResult(new {}, behavior: JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult MembersRead(ulong guildId, [DataSourceRequest] DataSourceRequest request)
        {
            var members = new GetGuildMembers().Request<List<DiscordMemberModel>>(guildId, out _, ProxyRequestType.Json);
            return new JsonNetResult(members.ToDataSourceResult(request), behavior: JsonRequestBehavior.AllowGet);
        }

        public ActionResult MembersDestroy(DiscordGuildModel model, [DataSourceRequest] DataSourceRequest request)
        {
            // Console.WriteLine("Attempting to leave - " + model.Id);
            // var leaveGuild = new LeaveGuild().Request<bool>(model.Id, out _, ProxyRequestType.Json);
            // if (leaveGuild)
            // {
            //     return Json(new[] {model}.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            // }
            // ModelState.AddModelError("LeaveError", "Could not leave the guild");
            return new JsonNetResult(new {}, behavior: JsonRequestBehavior.AllowGet);
        }
    }
}