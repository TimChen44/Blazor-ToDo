using AntDesign;
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
    public partial class IndexLibraryEdit : DrawerTemplate<Guid?, IndexLibraryDto>
    {
        [Inject] public HttpClient Http { get; set; }

        [Inject] public MessageService MsgSvr { get; set; }

        IndexLibraryDto model;

        bool isLoading = false;

        private List<string> unitOptions = new() { "元", "万元", "件", "吨" };

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            Console.WriteLine(base.Options);
            if (base.Options.HasValue)
                model = (await Http.GetFromJsonAsync<ResultData<IndexLibraryDto>>($"api/IndexLibrary/Get?id={base.Options}")).Data;
            else
                model = (await Http.GetFromJsonAsync<ResultData<IndexLibraryDto>>($"api/IndexLibrary/New")).Data;
            await base.OnInitializedAsync();
            isLoading = false;
        }

        async void OnSave()
        {
            var result = await Http.PutFromJsonAsync<IndexLibraryDto, ResultData<IndexLibraryDto>>($"api/IndexLibrary/Save", model);
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
}
