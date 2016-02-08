using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstract
{
    public interface IRepository
    {
        bool Login(string Name, string Password);
        void SaveUser(User user);
        User FindUser(string UserName);
        bool EditUser(User user);
        IEnumerable<User> GetAllUsers();

        IEnumerable<Task> GetCreatedTasks(string Name);
        IEnumerable<Task> GetResponsableTasks(string Name);
        void AddTask(int userID, Task task);
        bool EditTask(Task task);
        Task DeleteTask(int TaskID);
    }
}
