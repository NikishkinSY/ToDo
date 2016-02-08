using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Task
    {
        public enum TaskStatus
        {
            New,
            Executing,
            Complete,
            Canceled
        }

        [Key]
        public int TaskID { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string Comment { get; set; }
        public TaskStatus Status { get; set; }
        public DateTime DTCreate { get; set; }
        public DateTime DTExec { get; set; }

        [NotMapped]
        public User CreateUser { get; set; }
        public User ResponsableUser { get; set; }
    }


}
