using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ToDo.Entity;
using ToDo.Shared;
using System.Linq.Dynamic.Core;

namespace ToDo.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TaskController : ControllerBase
    {
        TodoContext Context;

        public TaskController(TodoContext context)
        {
            Context = context;
        }

        // 1、	列出当天的所有代办工作
        [HttpGet]
        public List<TaskDto> GetToDayTask()
        {
            var result = Context.Task.Where(x => x.PlanTime == DateTime.Now.Date);
            return QueryToDto(result).ToList();
        }

        [NonAction]
        private IQueryable<TaskDto> QueryToDto(IQueryable<Entity.Task> query)
        {
            return query.Select(x => new TaskDto()
            {
                TaskId = x.TaskId,
                Title = x.Title,
                Description = x.Description,
                PlanTime = x.PlanTime,
                Deadline = x.Deadline,
                IsImportant = x.IsImportant,
                IsFinish = x.IsFinish,
            });
        }

        //2、	添加代办
        [HttpPost]
        public Guid SaveTask(TaskDto dto)
        {
            Entity.Task entity;
            if (dto.TaskId == Guid.Empty)
            {
                entity = new Entity.Task();
                entity.TaskId = Guid.NewGuid();
                Context.Add(entity);
            }
            else
            {
                entity = Context.Task.FirstOrDefault(x => x.TaskId == dto.TaskId);
            }
            entity.Title = dto.Title;
            entity.Description = dto.Description;
            entity.PlanTime = dto.PlanTime;
            entity.Deadline = dto.Deadline;
            entity.IsImportant = dto.IsImportant;
            entity.IsFinish = dto.IsFinish;
            Context.SaveChanges();
            return entity.TaskId;
        }

        //3、	编辑待办
        //获得待办信息
        [HttpGet]
        public TaskDto GetTaskDto(Guid taskId)
        {
            var result = Context.Task.Where(x => x.TaskId == taskId);
            return QueryToDto(result).FirstOrDefault();
        }

        //4、	修改重要程度
        [HttpPost]
        public void SetImportant(SetImportantReq req)
        {
            var entity = Context.Task.FirstOrDefault(x => x.TaskId == req.TaskId);
            entity.IsImportant = req.IsImportant;
            Context.SaveChanges();
        }

        //5、	修改完成状态
        [HttpPost]
        public void SetFinish(SetFinishReq req)
        {
            var entity = Context.Task.FirstOrDefault(x => x.TaskId == req.TaskId);
            entity.IsFinish = req.IsFinish;
            Context.SaveChanges();
        }

        //6、	删除代办
        [HttpDelete]
        public void DelTask(Guid taskId)
        {
            Context.Task.Remove(Context.Task.Find(taskId));
            Context.SaveChanges();
        }

        //7、	查询代办
        [HttpPost]
        public GetSearchRsp GetSearch(GetSearchReq req)
        {
            if (req.PageIndex == 0) req.PageIndex = 1;
            var query = Context.Task.Where(x => x.Title.Contains(req.QueryTitle ?? ""));

            foreach (var sort in req.Sorts)
            {
                if (sort.SortOrder == "descend")
                    query = query.OrderBy(sort.SortField + " DESC");
                else
                    query = query.OrderBy(sort.SortField);

            }

            var result = new GetSearchRsp()
            {
                Data = QueryToDto(query.Skip(--req.PageIndex * req.PageSize).Take(req.PageSize)).ToList(),
                Total = query.Count(),
            };

            return result;
        }
    }
}
