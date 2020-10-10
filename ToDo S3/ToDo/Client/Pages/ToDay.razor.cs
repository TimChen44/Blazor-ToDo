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
    public partial class ToDay
    {
        [Inject]
        public HttpClient Http { get; set; }

        [Inject]
        public MessageService MsgSrv { get; set; }

        bool isLoading = true;

        private List<TaskDto> taskDtos = new List<TaskDto>();

        protected async override Task OnInitializedAsync()
        {
            taskDtos = await Http.GetFromJsonAsync<List<TaskDto>>("api/Task/GetToDayTask");
            isLoading = false;
            await base.OnInitializedAsync();
        }

        TaskDto newTask = new TaskDto() { PlanTime = DateTime.Now.Date };

        bool isNewLoading = false;

        async void OnInsert(KeyboardEventArgs e)
        {
            if (e.Code == "Enter")
            {
                isNewLoading = true;
                var result = await Http.PostAsJsonAsync<TaskDto>("api/Task/SaveTask", newTask);
                if (result.IsSuccessStatusCode)
                {
                    newTask.TaskId = await result.Content.ReadFromJsonAsync<Guid>();
                    taskDtos.Add(newTask);
                    newTask = new TaskDto() { PlanTime = DateTime.Now.Date };
                    MsgSrv.Success("添加成功");
                }
                else
                {
                    MsgSrv.Success($"添加失败{result.StatusCode}");
                }
                isNewLoading = false;
                StateHasChanged();
            }
        }

        async void OnCardClick(TaskDto task)
        {
            var options = new DrawerOptions()
            {
                Title = task.Title,
                Width = 450,
            };

            var drawerRef = await DrawerSvr.CreateAsync<TaskInfo, TaskDto, TaskDto>(options, task);

            drawerRef.OnClosed = async result =>
            {
                if (result == null) return;
                var index = taskDtos.FindIndex(x => x.TaskId == result.TaskId);
                taskDtos[index] = result;
                await InvokeAsync(StateHasChanged);
            };
        }

        private async void OnStar(TaskDto task)
        {
            SetStarReq req = new SetStarReq()
            {
                TaskId = task.TaskId,
                IsStar = !task.IsImportant,
            };

            var result = await Http.PostAsJsonAsync<SetStarReq>("api/Task/SetStar", req);
            if (result.IsSuccessStatusCode)
            {
                task.IsImportant = req.IsStar;
                StateHasChanged();
            }
        }

        private async void OnFinish(TaskDto task)
        {
            SetFinishReq req = new SetFinishReq()
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

        public async Task OnDel(TaskDto task)
        {
            if (await ConfigSvr.Show($"是否删除任务 {task.Title}", "删除", ConfirmButtons.YesNo, ConfirmIcon.Info) == ConfirmResult.Yes)
            {
                var result = await Http.DeleteAsync($"api/Task/DelTaskDto?taskId={task.TaskId}");
                if (result.IsSuccessStatusCode)
                {
                    taskDtos.Remove(task);
                    StateHasChanged();
                }
            }
        }
    }
}
