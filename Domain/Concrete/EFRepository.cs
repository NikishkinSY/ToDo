using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Abstract;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;

namespace Domain.Concrete
{
    public class EFRepository : EFDbContext, IRepository
    {
        public EFRepository()
        {
            Database.SetInitializer<EFRepository>(new DropCreateDatabaseIfModelChanges<EFRepository>());
        }

        //private EFDbContext context = new EFDbContext();
        #region user
        public bool Login(string name, string password)
        {
            return Users.Count(s => s.Name == name && s.Password == password) > 0;
        }
        public void SaveUser(User user)
        {
            Users.Add(user);
            SaveChanges();
        }
        public User FindUser(string name)
        {
            return Users.FirstOrDefault(s => s.Name == name);
        }
        public IEnumerable<User> GetAllUsers()
        {
            return Users;
        }
        public bool EditUser(User user)
        {
            User dbEntry = Users.FirstOrDefault(s => s.ID == user.ID);
            if (dbEntry != null)
            {
                dbEntry.Name = user.Name;
                dbEntry.Email = user.Email;
                dbEntry.PhoneNumber = user.PhoneNumber;
                dbEntry.Photo = user.Photo;
                SaveChanges();
                return true;
            }
            return false;
        }
        #endregion

        #region Tasks
        public IEnumerable<Task> GetCreatedTasks(string name)
        {
            User user = FindUser(name);
            var tasks = user != null ? user.CreateTasks : null;
            if (tasks != null)
                foreach (Task task in tasks)
                {
                    Entry(task).Reference(s => s.ResponsableUser).Load();
                }
            return tasks;
        }
        public IEnumerable<Task> GetResponsableTasks(string name)
        {
            var tasks = Tasks.Where(s => s.ResponsableUser.Name == name)
                .Include(s => s.ResponsableUser)
                .ToList();
            //foreach (Task task in Tasks)
            //{
            //    Entry(task).Reference(s => s.ResponsableUser).Load();
            //    Entry(task).Reference(s => s.CreateUser).Load();
            //}
            return tasks;
        }
        public void AddTask(int userID, Task task)
        {
            User userCreate = Users.FirstOrDefault(s => s.ID == userID);
            User userResponsable = Users.FirstOrDefault(s => s.ID == task.ResponsableUser.ID);
            if (userCreate != null && userResponsable != null)
            {
                task.ResponsableUser = userResponsable;
                userCreate.AddTask(task);

                //Entry(task.CreateUser).State = System.Data.Entity.EntityState.Unchanged;
                //Entry(task.ResponsableUser).State = System.Data.Entity.EntityState.Unchanged;
                var test = SaveChanges();
            }
            //user.AddTask(task);
            //User user = Users.FirstOrDefault(s => s.Name == userName);
            //user.CreateTasks.Add(task);
            //var test = SaveChanges();

        }
        public bool EditTask(Task task)
        {
            Task dbEntry = Tasks.Find(task.TaskID);
            if (dbEntry != null)
            {
                dbEntry.Name = task.Name;
                dbEntry.Text = task.Text;
                dbEntry.Comment = task.Comment;
                dbEntry.Status = task.Status;
                dbEntry.DTCreate = task.DTCreate;
                dbEntry.DTExec = task.DTExec;
                SaveChanges();
                return true;
            }
            return false;
        }
        public Task DeleteTask(int taskID)
        {
            Task dbEntry = Tasks.Find(taskID);
            if (dbEntry != null)
            {
                Tasks.Remove(dbEntry);
                SaveChanges();
            }
            return dbEntry;
        }
        #endregion
    }
}