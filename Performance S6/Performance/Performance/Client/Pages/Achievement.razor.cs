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
    public partial class Achievement
    {
        [Inject] public HttpClient Http { get; set; }

        [Inject] public MessageService MsgSvr { get; set; }

        private EditQuery editQuery = new EditQuery();
        public class EditQuery
        {
            public string Year { get; set; }

            public string Organization { get; set; }
            public Guid OrganizationId { get; set; }

        }


        protected async override Task OnInitializedAsync()
        {
            await LoadOrgOptions();
            LoadYearOptions();
            await base.OnInitializedAsync();
        }

        #region 企业/部门选择

        private List<SelectOptionCore> orgOptions = new List<SelectOptionCore>();

        private async Task LoadOrgOptions()
        {
            orgOptions = (await Http.GetFromJsonAsync<ResultDataSet<SelectOptionCore>>($"api/Org/GetOrgOptions")).DataSet;
        }

        private List<string> yearOptions = new List<string>();

        private void LoadYearOptions()
        {
            for (int i = DateTime.Now.Year; i > DateTime.Now.Year - 10; i--)
            {
                yearOptions.Add(i.ToString());
            }
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
            editQuery.OrganizationId = new Guid(((SelectOptionCore)option.Value).Value);
        }

        #endregion

        #region  数据采集

        public bool isLoading { get; set; }

        public List<AchievementEditDto> datas { get; set; }

        public async Task OnLoad()
        {
            datas = (await Http.GetFromJsonAsync<ResultDataSet<AchievementEditDto>>($"api/Achievement/GetEdit?year={editQuery.Year}&orgId={editQuery.OrganizationId}")).DataSet;
        }

        public async Task OnSave()
        {
            var result = await Http.PostFromJsonAsync<List<AchievementEditDto>, ResultMsg>($"api/Achievement/SaveEdit", datas);
            if (result.IsSuccess = true)
            {
                datas = null;
                MsgSvr.Success("保存成功");
            }
            else
            {
                MsgSvr.Error("保存失败");
            }
        }

        public void OnCancel()
        {
            datas = null;
        }

        #endregion

    }


}
