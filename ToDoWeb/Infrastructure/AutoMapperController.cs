using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Domain;
using ToDoWeb.Infrastructure.Models;
using System.Web.Mvc;
using Domain.Abstract;
using Domain.Concrete;

namespace ToDoWeb.Infrastructure
{
    /// <summary>
    /// transfer same properties one object to another
    /// </summary>
    public class AutoMapperController
    {
        public AutoMapperController()
        {
            AddAutoMap();
        }

        private void AddAutoMap()
        {
            Func<Task, object> resolverTask = Task =>
            {
                return Task.ResponsableUser != null ? Task.ResponsableUser.Name : string.Empty;
            };
            Func<TaskViewModel, object> resolverTaskViewModel = TaskViewModel =>
            {
                using (EFRepository efRepository = new EFRepository())
                {
                    var ResponsableUser = efRepository.GetAllUsers().FirstOrDefault(s => s.Name == TaskViewModel.ResponsableUser);
                    return ResponsableUser;
                }
            };



            Mapper.CreateMap<RegisterViewModel, User>();
            Mapper.CreateMap<TaskViewModel, Task>();
            Mapper.CreateMap<Task, TaskViewModel>()
                .ForMember(dest => dest.ResponsableUser, opt => opt.ResolveUsing(resolverTask));
            Mapper.CreateMap<TaskViewModel, Task>()
                .ForMember(dest => dest.ResponsableUser, opt => opt.ResolveUsing(resolverTaskViewModel));

            Mapper.CreateMap<EditUserViewModel, User>();
            Mapper.CreateMap<User, EditUserViewModel>();

            Mapper.CreateMap<UserNameViewModel, User>();
            Mapper.CreateMap<User, UserNameViewModel>();
        }
    }
}
