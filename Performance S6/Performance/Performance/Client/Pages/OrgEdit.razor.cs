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
    public partial class OrgEdit : DrawerTemplate<OrgEditParameter, OrgDto>
    {
        [Inject] public HttpClient Http { get; set; }

        [Inject] public MessageService MsgSvr { get; set; }

        OrgDto model;

        bool isLoading = false;

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            Console.WriteLine(base.Options);
            if (base.Options.OrganizationId.HasValue)
                model = (await Http.GetFromJsonAsync<ResultData<OrgDto>>($"api/Org/Get?id={base.Options.OrganizationId}")).Data;
            else
                model = (await Http.GetFromJsonAsync<ResultData<OrgDto>>($"api/Org/New?parentId={base.Options.ParentId}")).Data;
            await base.OnInitializedAsync();
            isLoading = false;
        }

        async void OnSave()
        {
            var result = await Http.PutFromJsonAsync<OrgDto, ResultData<OrgDto>>($"api/Org/Save", model);
            if (result.IsSuccess == true)
            {
                MsgSvr.Success($"{model.Name} 保存成功");
                await base.CloseAsync(result.Data);
            }
            else
            {
                MsgSvr.Error($"错误：{result.Msg}");
            }
        }

        async void OnCancel()
        {
            await base.CloseAsync(null);
        }
    }

    public class OrgEditParameter
    {
        public Guid? OrganizationId { get; set; }//如果有此值说明是编辑

        public Guid? ParentId { get; set; }//如果OrganizationId无值，且ParentId有值那么就是添加子节点，否则是添加根节点
    }
}
