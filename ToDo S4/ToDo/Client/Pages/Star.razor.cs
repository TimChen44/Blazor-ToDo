using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ToDo.Shared;

namespace ToDo.Client.Pages
{
    public partial class Star
    {
        // 1、	列出当天的所有代办工作
        [Inject] public HttpClient Http { get; set; }

        bool isLoading = true;
        private List<TaskDto> taskDtos = new List<TaskDto>();
        protected async override Task OnInitializedAsync()
        {
            isLoading = true;
            taskDtos = await Http.GetFromJsonAsync<List<TaskDto>>("api/Task/GetStarTask");
            isLoading = false;
            await base.OnInitializedAsync();
        }

        //2、	添加代办
        public MessageService MsgSrv { get; set; }
        async void OnInsert(TaskDto item)
        {
            taskDtos.Add(item);
        }

        //3、	编辑抽屉
        [Inject] public TaskDetailServices TaskSrv { get; set; }
        async void OnCardClick(TaskDto task)
        {
            TaskSrv.EditTask(task, taskDtos);
            await InvokeAsync(StateHasChanged);
        }

        //4、	修改重要程度
        private async void OnStar(TaskDto task)
        {
            var req = new SetImportantReq()
            {
                TaskId = task.TaskId,
                IsImportant = !task.IsImportant,
            };

            var result = await Http.PostAsJsonAsync<SetImportantReq>("api/Task/SetImportant", req);
            if (result.IsSuccessStatusCode)
            {
                task.IsImportant = req.IsImportant;
                StateHasChanged();
            }
        }

        //5、	修改完成与否
        private async void OnFinish(TaskDto task)
        {
            var req = new SetFinishReq()
            {
                TaskId = task.TaskId,
                IsFinish = !task.IsFinish,
            };

            var result = await Http.PostAsJsonAsync<SetFinishReq>("api/Task/SetFinish", req);
            if (result.IsSuccessStatusCode)
            {
                task.IsFinish = req.IsFinish;
                StateHasChanged();
            }
        }

        //6、	删除代办
        [Inject] public ConfirmService ConfirmSrv { get; set; }

        public async Task OnDel(TaskDto task)
        {
            if (await ConfirmSrv.Show($"是否删除任务 {task.Title}", "删除", ConfirmButtons.YesNo, ConfirmIcon.Info) == ConfirmResult.Yes)
            {
                taskDtos.Remove(task);
            }
        }
    }
}
