using AntDesign;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ToDo.Shared;

using System.Net.Http.Json;

namespace ToDo.Client.Pages
{
    public partial class TaskInfo : DrawerTemplate<TaskDto, TaskDto>
    {
        [Inject]
        public HttpClient Http { get; set; }

        [Inject]
        public MessageService MsgSvr { get; set; }

        TaskDto taskDto;

        bool isLoading = false;

        protected override async Task OnInitializedAsync()
        {
            taskDto = await Http.GetFromJsonAsync<TaskDto>($"api/Task/GetTaskDto?taskId={base.Options.TaskId}");
            await base.OnInitializedAsync();
        }

        async void OnSave()
        {
            var result = await Http.PostAsJsonAsync<TaskDto>($"api/Task/SaveTask", taskDto);
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                await base.CloseAsync(taskDto);
            }
            else
            {
                MsgSvr.Error($"请求发生错误 {result.StatusCode}");
            }
        }

        async void OnCancel()
        {
            await base.CloseAsync(null);
        }

    }
}
