using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Security.Principal;
using Domain.Abstract;
using Domain;
using ToDoWeb.Infrastructure;
using ToDoWeb.Infrastructure.Models;
using AutoMapper;
using Newtonsoft.Json;

namespace ToDoWeb.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {
        IRepository repository;
        public TasksController(IRepository _repository)
        {
            this.repository = _repository;
        }
        // GET: Task
        public ActionResult List()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View(new TaskViewModel(repository.GetAllUsers()));
        }

        [HttpPost]
        public void Edit(TaskViewModel taskViewModel)
        {
            if (ModelState.IsValid)
            {
                //find auth user by name
                User user = repository.FindUser(HttpContext.User.Identity.Name);
                //change task
                if (repository.EditTask(Mapper.Map<Task>(taskViewModel)))
                {
                    //if good recreate scheduler task
                    if (taskViewModel.Status == Task.TaskStatus.New ||
                        taskViewModel.Status == Task.TaskStatus.Executing)
                        Scheduler.AddExpiredTaskJob(new System.Net.Mail.MailAddress(user.Email),
                            taskViewModel.TaskID.ToString(),
                            taskViewModel.Name,
                            taskViewModel.DTExec);
                    //TempData["message"] = string.Format("{0} has been edited", taskViewModel.Name);
                }
                else
                    LogFactory.GetLogService().Error(string.Format("There is no Task with such ID: {0}", taskViewModel.TaskID));
            }
            else
                LogFactory.GetLogService().Error("Model state is invalid");
        }

        [HttpPost]
        public ActionResult Create(TaskViewModel taskViewModel)
        {
            if (ModelState.IsValid)
            {
                //find user by name
                User user = repository.FindUser(HttpContext.User.Identity.Name);
                //add task to user
                repository.AddTask(user.ID, Mapper.Map<Task>(taskViewModel));
                //if status New or Execution then create scheduler task to send email
                if (taskViewModel.Status == Task.TaskStatus.New ||
                    taskViewModel.Status == Task.TaskStatus.Executing)
                    Scheduler.AddExpiredTaskJob(new System.Net.Mail.MailAddress(user.Email),
                        taskViewModel.TaskID.ToString(),
                        taskViewModel.Name,
                        taskViewModel.DTExec);
                //TempData["message"] = string.Format("{0} has been created", taskViewModel.Name);
                return Redirect(Url.Action("List", "Tasks"));
            }
            else
            {
                TempData["message"] = "Data is invalid";
                LogFactory.GetLogService().Error("Model state is invalid");
                return View(new TaskViewModel(repository.GetAllUsers()));
            }
        }

        public ActionResult UserName()
        {
            User user = repository.FindUser(HttpContext.User.Identity.Name);
            if (user != null)
                return PartialView(Mapper.Map<User, UserNameViewModel>(user));
            else
                return PartialView(new UserNameViewModel());
        }
    }
}