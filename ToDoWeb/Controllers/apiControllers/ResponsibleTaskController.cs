using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ToDoWeb.Infrastructure;
using Newtonsoft.Json;
using System.Web;
using ToDoWeb.Infrastructure.Models;
using AutoMapper;
using Domain.Abstract;
using Domain;

namespace ToDoWeb.Controllers
{
    [Authorize]
    public class ResponsableTaskController : ApiController
    {
        IRepository repository;
        public ResponsableTaskController(IRepository _repository)
        {
            this.repository = _repository;
        }
        public IEnumerable<TaskViewModel> Get()
        {
            var responsableTasks = repository.GetResponsableTasks(HttpContext.Current.User.Identity.Name);
            var responsableTaskViewModels = Mapper.Map<IEnumerable<Task>, IEnumerable<TaskViewModel>>(responsableTasks);
            return responsableTaskViewModels;
        }

    }
}
