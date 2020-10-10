using AntDesign;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ToDo.Shared;

namespace ToDo.Client.Pages
{
    public partial class TaskInfo
    {
        [Inject]
        public HttpClient Http { get; set; }

        [Inject]
        public MessageService MsgSrv { get; set; }

        bool isLoading = true;

        TaskDto taskDto;

        protected override async Task OnInitializedAsync()
        {
            taskDto = await Http.GetFromJsonAsync<TaskDto>($"api/Task/GetTaskDto?taskId={base.Options.TaskId}");
            isLoading = false;
            await base.OnInitializedAsync();
        }

        async void OnSave()
        {
            var result = await Http.PostAsJsonAsync<TaskDto>("api/Task/SaveTask", taskDto);
            if (result.IsSuccessStatusCode)
            {
                await base.CloseAsync(taskDto);
            }
            else
            {
                MsgSrv.Success($"保存失败{result.StatusCode}");
            }
        }


    }
}
