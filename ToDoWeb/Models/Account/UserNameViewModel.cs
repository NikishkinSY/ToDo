using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ToDoWeb.Infrastructure.Models
{
    public class UserNameViewModel
    {
        [Required]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public byte[] Photo { get; set; }
        public string Base64Photo
        {
            get { return Convert.ToBase64String(Photo); }
            set { Photo = Convert.FromBase64String(value); }
        }
    }
}
