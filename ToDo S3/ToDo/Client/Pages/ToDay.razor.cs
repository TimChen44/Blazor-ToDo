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
        [Inject] public HttpClient Http { get; set; }

        // 1、	列出当天的所有代办工作
        private List<TaskDto> taskDtos = new List<TaskDto>();

        bool isLoading = true;
        protected async override Task OnInitializedAsync()
        {
            isLoading = true;
            taskDtos = await Http.GetFromJsonAsync<List<TaskDto>>("api/Task/GetToDayTask");
            isLoading = false;
            await base.OnInitializedAsync();
        }

        //2、	添加代办
        TaskDto newTask = new TaskDto() { PlanTime = DateTime.Now.Date };
        [Inject] public MessageService MsgSrv { get; set; }
        bool isNewLoading = false;
        async void OnInsert(KeyboardEventArgs e)
        {
            if (e.Code == "Enter")
            {
                if (string.IsNullOrWhiteSpace(newTask.Title))
                {
                    MsgSrv.Error($"标题必须填写");
                    return;
                }
                isNewLoading = true;
                var result = await Http.PostAsJsonAsync<TaskDto>($"api/Task/SaveTask", newTask);
                if (result.IsSuccessStatusCode)
                {
                    newTask.TaskId = await result.Content.ReadFromJsonAsync<Guid>();
                    taskDtos.Add(newTask);
                    newTask = new TaskDto() { PlanTime = DateTime.Now.Date };
                }
                else
                {
                    MsgSrv.Error($"请求发生错误 {result.StatusCode}");
                }
                isNewLoading = false;
                StateHasChanged();
            }
        }

        //3、	编辑待办
        [Inject] public DrawerService DrawerSrv { get; set; }

        async void OnCardClick(TaskDto task)
        {
            var result = await DrawerSrv.CreateDialogAsync<TaskInfo, TaskDto, TaskDto>(task, title: task.Title, width: 450);
            if (result == null) return;
            var index = taskDtos.FindIndex(x => x.TaskId == result.TaskId);
            taskDtos[index] = result;
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

        //5、	修改完成状态
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
