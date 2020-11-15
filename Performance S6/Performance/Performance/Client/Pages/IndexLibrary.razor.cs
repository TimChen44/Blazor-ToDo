using AntDesign;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;
using Performance.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Performance.Client.Pages
{
    public partial class IndexLibrary
    {
        [Inject] public HttpClient Http { get; set; }

        [Inject] public DrawerService DrawerSvr { get; set; }

        [Inject] public MessageService MsgSvr { get; set; }

        [Inject] public ConfirmService ConfirmSvr { get; set; }

        private bool isLoading = false;

        List<IndexLibraryDto> datas = new List<IndexLibraryDto>();

        private int total = 0;

        IndexLibraryQueryDto queryDto = new IndexLibraryQueryDto();

        ITable tableRef;

        private async Task OnSearch()
        {
            queryDto.PageIndex = 1;
            await OnQuery();
            StateHasChanged();
        }

        private async Task OnChange(AntDesign.TableModels.QueryModel<IndexLibraryDto> queryModel)
        {
            queryDto.Sorts = queryModel.SortModel.Where(x => x.SortType.Name != null)
                .Select(x => new QuerySort() { SortField = x.FieldName, SortOrder = x.SortType.Name }).ToList();
            await OnQuery();
        }

        private async Task OnQuery()
        {
            isLoading = true;
            var result = await Http.PostFromJsonAsync<IndexLibraryQueryDto, ResultDataSet<IndexLibraryDto>>($"api/IndexLibrary/Search", queryDto);
            datas = result.DataSet;
            total = result.Total;
            isLoading = false;
        }

        public async Task OnAdd()
        {
            var result = await DrawerSvr.CreateDialogAsync<IndexLibraryEdit, Guid?, IndexLibraryDto>(null, title: "新增指标", width: 450);
            if (result != null)
            {
                datas.Add(result);
            }
        }

        public async Task OnEdit(IndexLibraryDto model)
        {
            var result = await DrawerSvr.CreateDialogAsync<IndexLibraryEdit, Guid?, IndexLibraryDto>(model.IndexLibraryId, title: $"编辑指标 {model.Name}", width: 450);
            if (result != null)
            {
                var index = datas.IndexOf(model);
                datas[index] = result;
            }
        }

        public async void OnDelete(IndexLibraryDto model)
        {
            if (await ConfirmSvr.Show($"是否删除指标 {model.Name}", "删除", ConfirmButtons.YesNo, ConfirmIcon.Question) == ConfirmResult.Yes)
            {
                var result = await Http.DeleteFromJsonAsync<ResultMsg>($"api/IndexLibrary/Delete?id={model.IndexLibraryId}");
                if (result.IsSuccess)
                {
                    datas.Remove(model);
                    MsgSvr.Success($"{model.Name} 删除成功");
                    await InvokeAsync(StateHasChanged);
                }
                else
                    MsgSvr.Error($"{model.Name} 删除失败，错误:{result.Msg}");

            }
        }
        public async void OnChart(IndexLibraryDto model)
        {
            await DrawerSvr.CreateDialogAsync<IndexLibraryChart, Guid, object>(model.IndexLibraryId, title: $"指标分析 {model.Name}", width: 450);
        }

    }

}
