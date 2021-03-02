using AntDesign.Charts;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ToDo.Shared;
using System.Net.Http.Json;

namespace ToDo.Client.Pages
{
    public partial class Statistics
    {
        [Inject] public HttpClient Http { get; set; }

        bool isLoading = false;

        IChartComponent amountChart;

        readonly StackedColumnConfig amountConfig = new StackedColumnConfig
        {
            Title = new Title
            {
                Visible = true,
                Text = "每日代办数量统计"
            },
            ForceFit = true,
            Padding = "auto",
            XField = "day",
            YField = "value",
            YAxis = new ValueAxis
            {
                Min = 0,
            },
            Meta = new
            {
                day = new
                {
                    Alias = "日期"
                },
            },
            Color = new[] { "#ae331b", "#1a6179" },
            StackField = "type"
        };

        protected async override Task OnInitializedAsync()
        {
            isLoading = true;
            var amountData = await Http.GetFromJsonAsync<List<ChartAmountDto>>($"api/Chart/GetAmountDto");
            await amountChart.ChangeData(amountData);
            await base.OnInitializedAsync();

            isLoading = false;
        }

    }
}
