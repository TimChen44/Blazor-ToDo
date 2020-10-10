using AntDesign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo.Shared;

namespace ToDo.Client.Pages
{
    public class TaskService
    {
        public DrawerService DrawerSvr { get; set; }

        public TaskService(DrawerService drawerSvr)
        {
            DrawerSvr = drawerSvr;
        }

        public async Task<TaskDto> EditTask(TaskDto taskDto)
        {
            var config = new DrawerOptions()
            {
                Title = taskDto.Title,
                Width = 450,
            };
            var result = await DrawerSvr.CreateDialogAsync<TaskInfo, TaskDto, TaskDto>(config, taskDto);

            return result;
        }

        public void ReplaceItem(List<TaskDto> target, TaskDto newItem)
        {
            var index = target.FindIndex(x => x.TaskId == newItem.TaskId);
            target[index] = newItem;
        }
    }
}
