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
    public class TaskController : ControllerBase
    {
        TodoContext Context;

        public TaskController(TodoContext todoContext)
        {
            Context = todoContext;
        }

        [HttpGet]
        public List<TaskDto> GetToDayTask()
        {
            var result = Context.Task.Where(x => x.PlanTime == DateTime.Now.Date);
            return QueryToDto(result).ToList();
        }

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

        [HttpGet]
        public TaskDto GetTaskDto(Guid taskId)
        {
            var result = Context.Task.Where(x => x.TaskId == taskId);
            return QueryToDto(result).FirstOrDefault();
        }

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

        [HttpPost]
        public void SetStar(SetStarReq req)
        {
            var entity = Context.Task.FirstOrDefault(x => x.TaskId == req.TaskId);
            entity.IsImportant = req.IsStar;
            Context.SaveChanges();
        }

        [HttpPost]
        public void SetFinish(SetFinishReq req)
        {
            var entity = Context.Task.FirstOrDefault(x => x.TaskId == req.TaskId);
            entity.IsFinish = req.IsFinish;
            Context.SaveChanges();
        }

        [HttpDelete]
        public void DelTaskDto(Guid taskId)
        {
            var entity = Context.Task.FirstOrDefault(x => x.TaskId == taskId);
            Context.Remove(entity);
            Context.SaveChanges();
        }

        [HttpGet]
        public GetSearchRsp GetSearch(string title, int pageIndex, int pageSize)
        {
            if (pageIndex == 0) pageIndex = 1;
            var query = Context.Task.Where(x => x.Title.Contains(title ?? ""));

            var result = new GetSearchRsp()
            {
                Data = QueryToDto(query.Skip(--pageIndex * pageSize)).Take(pageSize).ToList(),
                Total = query.Count(),
            };
            return result;
        }
    }
}
