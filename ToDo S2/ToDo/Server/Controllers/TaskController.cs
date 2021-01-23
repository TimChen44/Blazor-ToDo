using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ToDo.Shared;

namespace ToDo.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TaskController : ControllerBase
    {
        [HttpGet]
        public List<TaskDto> GetToDayTask()
        {
            return RandomData();
        }


        [HttpGet]
        public List<TaskDto> GetSearch(string title)
        {
            Thread.Sleep(1000);
            return RandomData();
        }

        [NonAction]
        private List<TaskDto> RandomData()
        {
            var taskDtos = new List<TaskDto>();
            Random r = new Random();
            for (int i = 1; i < 10; i++)
            {
                Console.WriteLine(r.Next(0, 1));
                taskDtos.Add(new TaskDto
                {
                    Title = $"测试数据{i} {"".PadRight(r.Next(10), 'X')}",
                    Description = $"详细描述{i} {"".PadRight(r.Next(40), 'X')}",
                    PlanTime = DateTime.Now.Date.AddDays(r.Next(0, 4) * -1),
                    Deadline = ((r.Next(0, 2) == 0) ? (DateTime?)DateTime.Now.Date.AddDays(r.Next(-1, 3)) : null),
                    IsImportant = Convert.ToBoolean(r.Next(0, 2)),
                    TaskId = Guid.NewGuid(),

                });
            }
            return taskDtos;
        }
    }
}
