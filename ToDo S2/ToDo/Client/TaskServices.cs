using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ToDo.Shared;

namespace ToDo.Client
{
    public class TaskServices
    {

        public HttpClient Http { get; set; }

        public TaskServices(HttpClient http)
        {
            Http = http;
        }

        public async Task<List<TaskDto>> LoadToDay()
        {
            return await Http.GetFromJsonAsync<List<TaskDto>>("api/Task/GetToDayTask");
      
        }
        public async Task<List<TaskDto>> LoadStar()
        {
            return await Http.GetFromJsonAsync<List<TaskDto>>("api/Task/GetStarTask");

        }

        public async Task<List<TaskDto>> LoadSearch(string title)
        {
            return await Http.GetFromJsonAsync<List<TaskDto>>("api/Task/GetSearch");

        }
    }
}
