using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ToDoWeb.Infrastructure.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Name is empty")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Password is empty")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "E-mail is empty")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }
    }
}
