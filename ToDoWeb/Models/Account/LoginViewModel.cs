using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoWeb.Infrastructure.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Name is empty")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Password is empty")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

}
