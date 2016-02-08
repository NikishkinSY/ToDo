using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Quartz;
using Quartz.Impl;
using System.Net.Mail;

namespace ToDoWeb.Infrastructure
{
    public static class Scheduler
    {
        static IScheduler scheduler;
        enum JobType
        {
            User,
            Task

        }
        static Scheduler()
        {
            // Grab the Scheduler instance from the Factory 
            scheduler = StdSchedulerFactory.GetDefaultScheduler();
        }
        public static void Start()
        {
            scheduler.Start();
        }
        public static void Stop()
        {
            scheduler.Shutdown();
        }


        #region Job
        static JobDataMap CreateJobDataMap(MailAddress email, string subject, string body)
        {
            IDictionary<string, object> data = new Dictionary<string, object>();
            data.Add("Email", email);
            data.Add("Subject", subject);
            data.Add("Body", body);
            return new JobDataMap(data);
        }
        static ITrigger CreateTrigger(DateTime dt)
        {
            // computer a time that is on the next round minute
            DateTimeOffset runTime = new DateTimeOffset(dt);

            // Trigger the job to run on the next round minute
            ITrigger trigger = TriggerBuilder.Create()
                .StartAt(runTime)
                .Build();

            return trigger;
        }
        /// <summary>
        /// create ID job key
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        static string CreateIDJobKey(JobType type, string id)
        {
            return String.Format("{0}:{1}", type, id);
        }
        /// <summary>
        /// create job key
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        static JobKey CreateJobKey(JobType type, string id)
        {
            return new JobKey(CreateIDJobKey(type, id));
        }
        #endregion

        #region Task
        public static void AddExpiredTaskJob(MailAddress email, string taskID, string taskName, DateTime taskDT)
        {
            if (scheduler.CheckExists(CreateJobKey(JobType.Task, taskID)))
                RemoveExpiredTaskJob(taskID);

            IJobDetail job = CreateExpiredTaskJob(email, taskID, taskName);

            // Trigger the job to run on the next round minute
            ITrigger trigger = CreateTrigger(taskDT);

            // Tell quartz to schedule the job using our trigger
            if (scheduler.InStandbyMode || scheduler.IsStarted)
                scheduler.ScheduleJob(job, trigger);
        }
        static IJobDetail CreateExpiredTaskJob(MailAddress email, string taskID, string taskName)
        {
            JobDataMap jdm = CreateJobDataMap(
                email,
                String.Format("Job '{0}'", taskName),
                String.Format("Job '{0}' timeout", taskName));

            return JobBuilder.Create<SendEmailJob>()
                .WithIdentity(CreateIDJobKey(JobType.Task, taskID))
                .UsingJobData(jdm)
                .Build();
        }
        public static bool RemoveExpiredTaskJob(string jobID)
        {
            return scheduler.DeleteJob(CreateJobKey(JobType.Task, jobID));
        }
        #endregion


        #region User
        public static void AddRegisterUserJob(MailAddress email, string userName)
        {
            if (scheduler.CheckExists(CreateJobKey(JobType.User, userName)))
                RemoveRegisterUserJob(userName);

            IJobDetail job = CreateRegisterUserJob(email, userName);

            // Trigger the job to run on the next round minute
            ITrigger trigger = CreateTrigger(DateTime.Now);

            // Tell quartz to schedule the job using our trigger
            if (scheduler.InStandbyMode || scheduler.IsStarted)
                scheduler.ScheduleJob(job, trigger);
        }
        static IJobDetail CreateRegisterUserJob(MailAddress email, string userName)
        {
            JobDataMap jdm = CreateJobDataMap(
                email,
                "Registration",
                String.Format("{0}! Thank you for registration!", userName));

            return JobBuilder.Create<SendEmailJob>()
                .WithIdentity(CreateIDJobKey(JobType.Task, userName))
                .UsingJobData(jdm)
                .Build();
        }
        public static bool RemoveRegisterUserJob(string jobID)
        {
            return scheduler.DeleteJob(CreateJobKey(JobType.User, jobID));
        }
        #endregion
    }

    public class SendEmailJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                MailAddress Email = (MailAddress)context.MergedJobDataMap.Get("Email");
                string Subject = (string)context.MergedJobDataMap.Get("Subject");
                string Body = (string)context.MergedJobDataMap.Get("Body");
                EmailService.SendMail(Email, Subject, Body);
            }
            catch (Exception ex)
            {
                LogFactory.GetLogService().Error(ex);
            }
        }
    }
}
