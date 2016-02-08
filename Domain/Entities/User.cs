using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mail;

namespace Domain
{
    public class User
    {
        [Key]
        public int ID { get; set; }
        [StringLength(50)]
        [Index(IsUnique = true)]
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public byte[] Photo { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public virtual ICollection<Task> CreateTasks { get; set; }
        public virtual ICollection<Task> ResponsableTasks { get; set; }

        public User()
        {
            this.CreateTasks = new HashSet<Task>();
            this.ResponsableTasks = new HashSet<Task>();
            this.Photo = new byte[] { };
        }

        public void AddTask(Task task)
        {
            task.CreateUser = this;
            if (!this.CreateTasks.Contains(task))
                this.CreateTasks.Add(task);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
