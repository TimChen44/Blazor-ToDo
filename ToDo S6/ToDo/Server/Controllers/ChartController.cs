using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo.Entity;
using ToDo.Shared;

namespace ToDo.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ChartController
    {
        TodoContext Context;

        public ChartController(TodoContext context)
        {
            Context = context;
        }


        //每日待办数量
        public List<ChartAmountDto> GetAmountDto()
        {
            return Context.Task.GroupBy(x => new { x.PlanTime, x.IsImportant }).Select(x => new ChartAmountDto()
            {
                Day = x.Key.PlanTime.ToString("yy-MM-dd"),
                Type = x.Key.IsImportant ? "普通" : "重要",
                Value = x.Count(),
            }).ToList();

        }
    }
}
