using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoListAPI.Models;

namespace ToDoListAPI.Data
{
    public class ToDoListAPIContext : DbContext
    {
        public ToDoListAPIContext(DbContextOptions<ToDoListAPIContext> options)
            : base(options)
        {
        }

        public DbSet<ToDoListAPI.Models.User> User { get; set; } = default!;

        public DbSet<ToDoListAPI.Models.ToDo>? ToDo { get; set; }

        public DbSet<ToDoListAPI.Models.ToDoList>? ToDoList { get; set; }
        public DbSet<ToDoListAPI.Models.ToDoListCollaborator>? ToDoListCollaborator { get; set; }
    }
}