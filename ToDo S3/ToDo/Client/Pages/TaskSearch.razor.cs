using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Threading.Tasks;
using ToDo.Shared;

namespace ToDo.Client.Pages
{
    public partial class TaskSearch
    {
        private bool isLoading = false;

        List<TaskDto> datas = new List<TaskDto>();

        private string queryTitle;

        [Inject]
        public HttpClient Http { get; set; }

        private int total = 0;

        private async Task OnSearch()
        {
           await  OnQuery(1, 10);
        }

        private async Task OnChange(AntDesign.TableModels.QueryModel<TaskDto> queryModel)
        {
            await OnQuery(queryModel.PageIndex, queryModel.PageSize);
        }

        private async Task OnQuery(int pageIndex, int pageSize)
        {
            isLoading = true;
            var result = await Http.GetFromJsonAsync<GetSearchRsp>($"api/Task/GetSearch?title={queryTitle}&pageIndex={pageIndex}&pageSize={pageSize}");
            datas = result.Data;
            total = result.Total;
            isLoading = false;
        }

        [Inject]
        public TaskService TaskSrv { get; set; }

        private async Task OnDetail(TaskDto taskDto)
        {
            var result = await TaskSrv.EditTask(taskDto);
            if (result != null)
            {
                TaskSrv.ReplaceItem(datas, result);
                StateHasChanged();
            }
        }
    }
}
