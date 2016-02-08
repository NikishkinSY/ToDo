using Domain;
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
using Domain.Abstract;

namespace ToDoWeb.Controllers
{
    [Authorize]
    public class OtherController : ApiController
    {
        IRepository repository;
        public OtherController(IRepository _repository)
        {
            this.repository = _repository;
        }

        public IEnumerable<string> GetUserNames()
        {
            return repository.GetAllUsers().Select(s => s.Name);
        }

        public UserNameViewModel GetUserName()
        {
            User user = repository.FindUser(HttpContext.Current.User.Identity.Name);
            return AutoMapper.Mapper.Map<User, UserNameViewModel>(user);
        }
    }
}
