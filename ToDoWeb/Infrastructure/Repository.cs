using Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToDoWeb.Infrastructure
{
    public abstract class Repository
    {
        protected IRepository repository;
        public Repository(IRepository repository)
        {
            this.repository = repository;
        }
    }
}