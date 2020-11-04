using TCAdmin.GameHosting.SDK.Objects;
using TCAdmin.TaskScheduler.ModuleApi;

namespace TCAdminNexus.Objects.ClientService
{
    internal struct TaskData
    {
        public TaskInfo TaskInfo { get; set; }

        public Service Service { get; set; }

        public StepInfo StepInfo { get; set; }
    }
}