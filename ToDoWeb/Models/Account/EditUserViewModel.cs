using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ToDoWeb.Infrastructure.Models
{
    public class EditUserViewModel
    {
        [Required]
        [HiddenInput(DisplayValue = false)]
        public string ID { get; set; }

        [Required(ErrorMessage = "Name is empty")]
        [HiddenInput(DisplayValue = false)]
        public string Name { get; set; }

        [Display(Name = "Email address")]
        [Required(ErrorMessage = "E-mail is empty")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }

        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Photo")]
        public byte[] Photo { get; set; }
    }
}