using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo.Shared;

namespace ToDo.Client.Pages
{
    public partial class TaskItem
    {
        //项目属性
        [Parameter]
        public TaskDto Item { get; set; }

        [Parameter]
        public EventCallback<TaskDto> OnFinish { get; set; }

        public async void OnFinishClick()
        {
            if (OnFinish.HasDelegate)
                await OnFinish.InvokeAsync(Item);
        }

        [Parameter]
        public EventCallback<TaskDto> OnCard { get; set; }

        public async void OnCardClick()
        {
            if (OnCard.HasDelegate)
                await OnCard.InvokeAsync(Item);
        }

        [Parameter]
        public EventCallback<TaskDto> OnDel { get; set; }

        public async void OnDelClick()
        {
            if (OnDel.HasDelegate)
                await OnDel.InvokeAsync(Item);
        }

        [Parameter]
        public EventCallback<TaskDto> OnStar { get; set; }

        public async void OnStarClick()
        {
            if (OnStar.HasDelegate)
                await OnStar.InvokeAsync(Item);
        }

        //属性
        [Parameter]
        public bool ShowStar { get; set; } = true;

        //模板
        [Parameter]
        public RenderFragment TitleTemplate { get; set; }


    }
}
