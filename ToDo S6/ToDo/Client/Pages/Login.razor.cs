using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ToDo.Shared;

namespace ToDo.Client.Pages
{
    public partial class Login
    {
        [Inject] public HttpClient Http { get; set; }
        [Inject] public MessageService MsgSvr { get; set; }
        [Inject] public AuthenticationStateProvider AuthProvider { get; set; }

        LoginDto model = new LoginDto();
        bool isLoading;

        async void OnLogin()
        {
            isLoading = true;

            var httpResponse = await Http.PostAsJsonAsync<LoginDto>($"api/Auth/Login", model);
            UserDto result = await httpResponse.Content.ReadFromJsonAsync<UserDto>();

            if (string.IsNullOrWhiteSpace(result?.Token) == false)
            {
                MsgSvr.Success($"登录成功");
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);
                ((AuthProvider)AuthProvider).MarkUserAsAuthenticated(result);
            }
            else
            {
                MsgSvr.Error($"用户名或密码错误");
            }
            isLoading = false;
            InvokeAsync(StateHasChanged);
        }
    }
}
