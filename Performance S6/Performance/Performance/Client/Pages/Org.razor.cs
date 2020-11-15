using AntDesign;
using AutoMapper;
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
    public partial class Org
    {
        [Inject] public HttpClient Http { get; set; }

        [Inject] public DrawerService DrawerSvr { get; set; }

        [Inject] public ConfirmService ConfirmSvr { get; set; }

        [Inject] public MessageService MsgSvr { get; set; }

        [Inject] public IMapper Mapper { get; set; }

        protected async override Task OnInitializedAsync()
        {
            await LoadRootOrgs();
            await LoadIndexOptions();
        }

        #region 载入与显示

        List<OrgTreeDto> datas = new();

        private bool isLoading = false;

        private async Task LoadRootOrgs()
        {
            isLoading = true;
            var result = await LoadChilds(null);
            datas.AddRange(result);
            await base.OnInitializedAsync();
            isLoading = false;
        }

        public async Task OnNodeLoadDelayAsync(TreeEventArgs args)
        {
            if (args.Node.IsExpanded == true) return;
            isLoading = true;
            var dataItem = ((OrgTreeDto)args.Node.DataItem);
            var result = await LoadChilds(dataItem.OrganizationId);
            dataItem.Childs.Clear();
            dataItem.Childs.AddRange(result);
            isLoading = false;
        }

        private async Task<IEnumerable<OrgTreeDto>> LoadChilds(Guid? parentId)
        {
            var result = await Http.GetFromJsonAsync<ResultDataSet<OrgTreeDto>>($"api/Org/GetOrgChilds?parentId={parentId}");
            return result.DataSet;
        }

        public OrgDto currentOrgDto { get; set; }

        public TreeNode currentTreeNode { get; set; }

        private bool isCurrentLoading = false;


        #endregion

        #region 树的维护

        public async void OnAddRoot()
        {
            var result = await DrawerSvr.CreateDialogAsync<OrgEdit, OrgEditParameter, OrgDto>(new OrgEditParameter(), title: "新增企业", width: 450);
            if (result != null)
            {
                datas.Add(Mapper.Map<OrgTreeDto>(result));
                await InvokeAsync(StateHasChanged);
            }

        }

        public async void OnAddChild(TreeNode node)
        {
            if (node.DataItem is OrgTreeDto dataItem)
            {
                var result = await DrawerSvr.CreateDialogAsync<OrgEdit, OrgEditParameter, OrgDto>(
                    new() { ParentId = dataItem.OrganizationId }, title: $"新增部门", width: 450);
                if (result != null)
                {
                    dataItem.Childs.Add(Mapper.Map<OrgTreeDto>(result));
                    await InvokeAsync(StateHasChanged);
                }
            }

        }

        public async void OnEdit(TreeNode node)
        {
            if (node.DataItem is OrgTreeDto dataItem)
            {
                var result = await DrawerSvr.CreateDialogAsync<OrgEdit, OrgEditParameter, OrgDto>(
                new() { OrganizationId = dataItem.OrganizationId }, title: $"编辑 {dataItem.Name}", width: 450);
                if (result != null)
                {
                    dataItem.Name = result.Name;

                    if (dataItem.OrganizationId == currentOrgDto?.OrganizationId)
                    {
                        currentOrgDto = result;
                    }
                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        public async void OnDelete(TreeNode node)
        {
            if (node.DataItem is OrgTreeDto dataItem)
            {
                if (await ConfirmSvr.Show($"是否删除节点 {node.Name}", "删除", ConfirmButtons.YesNo, ConfirmIcon.Question) == ConfirmResult.Yes)
                {
                    var result = await Http.DeleteFromJsonAsync<ResultMsg>($"api/Org/Delete?id={dataItem.OrganizationId}");
                    if (result.IsSuccess)
                    {
                        MsgSvr.Success($"{dataItem.Name} 删除成功");

                        if (node.ParentNode == null)
                        {
                            datas.Remove(dataItem);
                        }
                        else
                        {
                            ((OrgTreeDto)node.ParentNode.DataItem).Childs.Remove(dataItem);
                        }

                        if (dataItem.OrganizationId == currentOrgDto?.OrganizationId)
                        {
                            currentOrgDto = null;
                            currentTreeNode = null;
                        }
                        await InvokeAsync(StateHasChanged);
                    }
                    else
                        MsgSvr.Error($"{dataItem.Name} 删除失败，错误:{result.Msg}");
                }
            }
        }

        #endregion


        #region 关联指标

        private async Task OnSelectNode(TreeEventArgs args)
        {
            if (args.Node.DataItem is OrgTreeDto dataItem)
            {
                isCurrentLoading = true;
                var result = await Http.GetFromJsonAsync<ResultData<OrgDto>>($"api/Org/Get?id={dataItem.OrganizationId}");
                currentOrgDto = result.Data;
                currentTreeNode = args.Node;
                await LoadIndexs(dataItem.OrganizationId);
                isCurrentLoading = false;
            }
        }

        private List<SelectOptionCore> indexOptions = new();

        private List<IndexLibraryDto> linkIndexs = new();

        AutoComplete<SelectOptionCore> auto;

        private string indexValue;
        private async Task LoadIndexs(Guid orgId)
        {
            linkIndexs = (await Http.GetFromJsonAsync<ResultDataSet<IndexLibraryDto>>($"api/Org/GetIndexs?OrgId={orgId}")).DataSet;
        }


        private async Task LoadIndexOptions()
        {
            indexOptions = (await Http.GetFromJsonAsync<ResultDataSet<SelectOptionCore>>($"api/IndexLibrary/GetIndexOptions")).DataSet;
        }

        Func<object, object, bool> CompareWith = (a, b) =>
        {
            if (a is SelectOptionCore o1 && b is SelectOptionCore o2)
            {
                return o1.Value == o2.Value;
            }
            else
            {
                return false;
            }
        };

        private async Task OnIndexSelection(AutoCompleteOption option)
        {
            if (option.Value is SelectOptionCore select)
            {
                var result = await Http.PostFromJsonAsync<OrgIndexLinkDto, ResultData<IndexLibraryDto>>($"api/Org/LinkOrg", new OrgIndexLinkDto() { OrganizationId = currentOrgDto.OrganizationId, IndexLibraryId = new Guid(select.Value) });
                if (result.IsSuccess)
                {
                    linkIndexs.Add(result.Data);
                    MsgSvr.Success($"{select.Text} 添加成功");

                    indexValue = "";
                    auto.SelectedValue = null;
                }
                else
                {
                    MsgSvr.Error(result.Msg);
                }

            }
        }

        private async Task OnDeleteLink(IndexLibraryDto indexLibrary)
        {
            var result = await Http.DeleteFromJsonAsync<ResultMsg>($"api/Org/DelLinkOrg?orgId={currentOrgDto.OrganizationId}&indexId={indexLibrary.IndexLibraryId}");
            if (result.IsSuccess)
            {
                linkIndexs.Remove(indexLibrary);
                MsgSvr.Success($"{indexLibrary.Name} 删除成功");
            }
            else
                MsgSvr.Error(result.Msg);
        }

        #endregion

    }
}
