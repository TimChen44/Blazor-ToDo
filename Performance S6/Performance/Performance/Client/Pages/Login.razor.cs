using AntDesign;
using Microsoft.AspNetCore.Components;
using Performance.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Performance.Client.Pages
{
    public partial class Login : DrawerTemplate<object, UserDto>
    {
        [Inject] public HttpClient Http { get; set; }

        [Inject] public MessageService MsgSvr { get; set; }

        LoginDto model=new LoginDto();

        bool isLoading;

        async void OnSave()
        {
            isLoading = true;
            var result = await Http.PostFromJsonAsync<LoginDto, ResultData<UserDto>>($"api/Auth/Login", model);
            if (result.IsSuccess == true)
            {
                MsgSvr.Success($"登录成功");

                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Data.Token);
                await base.CloseAsync(result.Data);
            }
            else
            {
                MsgSvr.Error($"用户名或密码错误");
            }
            isLoading = false;
        }

        async void OnCancel()
        {
            await base.CloseAsync();
        }
    }
}
