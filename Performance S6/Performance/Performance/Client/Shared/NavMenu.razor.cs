using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Performance.Client.Core;
using Performance.Client.Pages;
using Performance.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Performance.Client.Shared
{
    public partial class NavMenu
    {
        [Inject] public DrawerService DrawerSvr { get; set; }

        [Inject] public AuthenticationStateProvider AuthProvider { get; set; }

        private string UserName => (AuthProvider as AuthProvider)?.UserName;

        async void OnLogin()
        {
            var result = await DrawerSvr.CreateDialogAsync<Login, object, UserDto>(null, title: $"登录", width: 450);
            if (result != null)
            {
                ((AuthProvider)AuthProvider).MarkUserAsAuthenticated(result);
            }
        }

        void OnOutLogin()
        {
            ((AuthProvider)AuthProvider).MarkUserAsLoggedOut();
        }

    }
}
