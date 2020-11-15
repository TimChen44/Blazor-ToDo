using AntDesign;
using AntDesign.Charts;
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
    public partial class IndexLibraryChart : DrawerTemplate<Guid, object>
    {
        [Inject] public HttpClient Http { get; set; }

        private bool isLoading = false;

        protected async override Task OnInitializedAsync()
        {
            isLoading = true;
            var result = (await Http.GetFromJsonAsync<ResultDataSet<IndexLibraryChartDto>>($"api/Chart/GetIndexLibraryColumn?indexId={this.Options}")).DataSet;
            
            columnDatas = result;
            if (columnDatas.Count > 0)
                await columnRef.ChangeData(columnDatas);
           
            donutDatas = result.Where(x => x.Year == result.Max(y => y.Year)).ToList();
            if (donutDatas.Count > 0)
                await donutRef.ChangeData(donutDatas);
            await base.OnInitializedAsync();
            isLoading = false;
        }


        IChartComponent columnRef;
        public List<IndexLibraryChartDto> columnDatas { get; set; }
        /// <summary>
        /// 柱状图
        /// </summary>
        readonly GroupedColumnConfig columnConfig = new()
        {
            ForceFit = true,
            XField = "year",
            YField = "actualValue",
            YAxis = new ValueAxis
            {
                Min = 0
            },
            Meta = new
            {
                year = new
                {
                    Alias = "年份"
                }
            },
            Label = new ColumnViewConfigLabel
            {
                Visible = true
            },
            GroupField = "orgName",
        };


        IChartComponent donutRef;

        public List<IndexLibraryChartDto> donutDatas { get; set; }
        /// <summary>
        /// 环图
        /// </summary>
        readonly DonutConfig donutConfig = new()
        {
            ForceFit = true,
            Radius = 0.8,
            AngleField = "actualValue",
            ColorField = "orgName"
        };
    }
}
