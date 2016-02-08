using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using ToDoWeb.Infrastructure;
using Domain.Concrete;
using System.Data.Entity;
using Domain.Abstract;
using Domain;
using System.Globalization;
using Ninject;

namespace ToDoWeb
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory());
            //init automapper
            AutoMapperController MapController = new AutoMapperController();
            //start scheluder
            Scheduler.Start();
            //create tasks jobs

            using (EFRepository efRepository = new EFRepository())
            {
                foreach (User user in efRepository.GetAllUsers())
                {
                    foreach (Task task in user.CreateTasks)
                    {
                        Scheduler.AddExpiredTaskJob(new System.Net.Mail.MailAddress(user.Email),
                            task.TaskID.ToString(),
                            task.Name,
                            task.DTExec);
                    }
                }
            }
            //init log4net
            LogFactory.InitializeLogServiceFactory(log4net.LogManager.GetLogger("ToDo"));

            //Task task2 = new Task();
            //task2.DTCreate = DateTime.Now;
            //task2.DTExec = DateTime.Now;
            //DBFabric.Repository.AddTask(DBFabric.Repository.GetAllUsers().First(), task2);
        }

        protected void Application_Error(object sender, EventArgs args)
        {
            Exception ex = ((HttpApplication)sender).Context.Server.GetLastError();
            LogFactory.GetLogService().Error(ex);
        }
    }
}