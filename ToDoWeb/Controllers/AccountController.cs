using AutoMapper;
using Domain;
using Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ToDoWeb.Infrastructure;
using ToDoWeb.Infrastructure.Models;
using static System.Net.WebRequestMethods;

namespace ToDoWeb.Controllers
{
    public class AccountController : Controller
    {
        IRepository repository;
        public AccountController(IRepository _repository)
        {
            this.repository = _repository;
        }
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                bool result = repository.Login(model.Name, model.Password);
                if (result)
                {
                    //set auth to cookie
                    FormsAuthentication.SetAuthCookie(model.Name, false);
                    return Redirect(returnUrl ?? Url.Action("List", "Tasks"));
                }
                else
                {
                    ModelState.AddModelError("Incorrect", "Incorrect login or password");
                    LogFactory.GetLogService().Warn(String.Format("Incorrect login or password ({0})", model.Name));
                    return View();
                }
            }
            else
            {
                LogFactory.GetLogService().Error("Model state is invalid");
                return View();
            }
        }

        [Authorize]
        public ActionResult Exit()
        {
            FormsAuthentication.SignOut();
            return Redirect(Url.Action("Login", "Account"));
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel userViewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //save new user
                    repository.SaveUser(AutoMapper.Mapper.Map<User>(userViewModel));
                    //set auth to cookie
                    FormsAuthentication.SetAuthCookie(userViewModel.Name, false);
                    //send mail about registration
                    Scheduler.AddRegisterUserJob(new System.Net.Mail.MailAddress(userViewModel.Email),
                            userViewModel.Name);
                    return Redirect(returnUrl ?? Url.Action("List", "Tasks"));
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("Used", String.Format("{0} is used", userViewModel.Name));
                    LogFactory.GetLogService().Error(ex);
                    return View();
                }
            }
            else
            {
                LogFactory.GetLogService().Error("Model state is invalid");
                return View();
            }
        }

        [Authorize]
        public ActionResult PhotoImage()
        {
            User user = repository.FindUser(HttpContext.User.Identity.Name);
            if (user != null && user.Photo != null)
                return File(user.Photo, "image/jpeg");
            return null;
        }

        [Authorize]
        public ActionResult Edit()
        {
            User user = repository.FindUser(HttpContext.User.Identity.Name);
            return user != null ? View(Mapper.Map<EditUserViewModel>(user)) : View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(EditUserViewModel editUserViewModel, HttpPostedFileBase photoImage)
        {
            if (ModelState.IsValid)
            {
                User user = Mapper.Map<User>(editUserViewModel);
                if (photoImage != null)
                    user.Photo = StreamHelper.ReadToEnd(photoImage.InputStream);

                //change task
                if (repository.EditUser(user))
                {
                    //if good recreate scheduler task
                    foreach (Task task in user.CreateTasks)
                        if (task.Status == Task.TaskStatus.New ||
                            task.Status == Task.TaskStatus.Executing)
                            Scheduler.AddExpiredTaskJob(new System.Net.Mail.MailAddress(user.Email),
                                task.TaskID.ToString(),
                                task.Name,
                                task.DTExec);
                }
                else
                    LogFactory.GetLogService().Error(string.Format("There is no User with such Name: {0}", editUserViewModel.Name));
            }
            else
                LogFactory.GetLogService().Error("Model state is invalid");

            return Redirect(Url.Action("Edit", "Account"));
        }
    }
}