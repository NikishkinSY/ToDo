using AutoMapper;
using Domain;
using Domain.Abstract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ToDoWeb.Infrastructure;
using ToDoWeb.Infrastructure.Models;

namespace ToDoWeb.Controllers
{
    [Authorize]
    public class CreatedTasksController : ApiController
    {
        IRepository repository;
        public CreatedTasksController(IRepository _repository)
        {
            this.repository = _repository;
        }

        public IEnumerable<TaskViewModel> Get()
        {
            var createdTasks = repository.GetCreatedTasks(HttpContext.Current.User.Identity.Name);
            var createdTasksViewModels = Mapper.Map<IEnumerable<Task>, IEnumerable<TaskViewModel>>(createdTasks);
            return createdTasksViewModels;
        }

        [HttpPost]
        public void Delete(int taskID)
        {
            Task task = repository.DeleteTask(taskID);
            if (task != null)
                Scheduler.RemoveExpiredTaskJob(taskID.ToString());
            else
                LogFactory.GetLogService().Error(string.Format("There is no Task with such ID: {0}", taskID));
        }


    }
}
