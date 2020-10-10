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
    public partial class TaskSearch
    {
        //7、	查询代办
        [Inject]
        public HttpClient Http { get; set; }

        private bool isLoading = false;

        List<TaskDto> datas = new List<TaskDto>();

        private string queryTitle;

        private int total = 0;

        private async Task OnSearch()
        {
            await OnQuery(1, 10);
        }

        private async Task OnChange(AntDesign.TableModels.QueryModel<TaskDto> queryModel)
        {
            //await OnQuery(queryModel.PageIndex, queryModel.PageSize);

            await OnQuery2(
                queryModel.PageIndex,
                queryModel.PageSize,
                queryModel.SortModel.Where(x => string.IsNullOrEmpty(x.SortType.Name) == false).OrderBy(x => x.Priority)
                .Select(x => new SortFieldName() { FieldName = x.FieldName, SortType = x.SortType.Name }).ToList()
                );
        }

        private async Task OnQuery(int pageIndex, int pageSize)
        {
            isLoading = true;
            var result = await Http.GetFromJsonAsync<GetSearchRsp>($"api/Task/GetSearch?title={queryTitle}&pageIndex={pageIndex}&pageSize={pageSize}");
            datas = result.Data;
            total = result.Total;
            isLoading = false;
        }

        private async Task OnQuery2(int pageIndex, int pageSize, List<SortFieldName> sort)
        {
            isLoading = true;
            var req = new GetSearchReq()
            {
                QueryTitle = queryTitle,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Sorts = sort,
            };
            var httpRsp = await Http.PostAsJsonAsync<GetSearchReq>($"api/Task/GetSearch2", req);
            var result = await httpRsp.Content.ReadFromJsonAsync<GetSearchRsp>();
            datas = result.Data;
            total = result.Total;

            isLoading = false;
        }

        //8、	查看详细抽屉
        [Inject]
        public TaskService TaskSrv { get; set; }

        private async Task OnDetail(TaskDto taskDto)
        {
            var result = await TaskSrv.EditTask(taskDto);
            if (result != null)
                TaskSrv.ReplaceItem(datas, result);
        }
    }
}
