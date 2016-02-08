using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Domain;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Domain.Abstract;

namespace ToDoWeb.Infrastructure.Models
{
    public class TaskViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int TaskID { get; set; }
        [Required(ErrorMessage = "Name is empty")]
        public string Name { get; set; }
        public string Text { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string Comment { get; set; }
        [Required(ErrorMessage = "Status is empty")]
        [EnumDataTypeAttribute(typeof(Task.TaskStatus))]
        [JsonConverter(typeof(StringEnumConverter))]
        public Task.TaskStatus Status { get; set; }
        [HiddenInput(DisplayValue = false)]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime DTCreate { get; set; }
        [Required(ErrorMessage = "Datetime end is empty")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime DTExec { get; set; }
        [Required(ErrorMessage = "Who is responsable")]
        public string ResponsableUser { get; set; }
        [HiddenInput(DisplayValue = false)]
        public IEnumerable<string> Users { get; set; }



        public TaskViewModel()
        {
            this.Name = "1";
            this.DTCreate = DateTime.Now;
            this.DTExec = DateTime.Now;
            //this.Users = (from user in DBFabric.Repository.GetAllUsers() select user.Name).ToList();
        }
        public TaskViewModel(IEnumerable<User> Users)
        {
            this.Name = "1";
            this.DTCreate = DateTime.Now;
            this.DTExec = DateTime.Now;
            this.Users = (from user in Users select user.Name).ToList();
        }
    }
}
