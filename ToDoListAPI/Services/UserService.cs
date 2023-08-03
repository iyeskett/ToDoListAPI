using Microsoft.EntityFrameworkCore;
using ToDoListAPI.Data;
using ToDoListAPI.Models;

namespace ToDoListAPI.Services
{
    public class UserService
    {
        private ToDoListAPIContext _context;

        public UserService(ToDoListAPIContext context)
        {
            _context = context;
        }

        public async Task InsertUser(User user) => await _context.User.AddAsync(user);

        public async Task GetUser(int id) => await _context.User.FirstOrDefaultAsync(user => user.Id == id);
    }
}