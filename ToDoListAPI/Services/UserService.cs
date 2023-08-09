using Microsoft.EntityFrameworkCore;
using ToDoListAPI.Data;
using ToDoListAPI.Models;
using ToDoListAPI.Services.Exceptions;

namespace ToDoListAPI.Services
{
    public class UserService
    {
        private ToDoListAPIContext _context;

        public UserService(ToDoListAPIContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByIdAsync(int id) => await _context.User.FirstOrDefaultAsync(user => user.Id == id) ?? throw new NotFoundException("Usuário não encontrado");

        public async Task<User> GetUserByEmailAsync(string email) => await _context.User.FirstOrDefaultAsync(user => user.Email == email) ?? throw new NotFoundException("Usuário não encontrado");

        public async Task<User> GetUserByUsernameAsync(string username) => await _context.User.FirstOrDefaultAsync(user => user.Username == username) ?? throw new NotFoundException("Usuário não encontrado");

        public async Task<bool> IsDuplicateEmail(string email) => await _context.User.FirstOrDefaultAsync(user => user.Email == email) != null;

        public async Task<bool> IsDuplicateUsername(string username) => await _context.User.FirstOrDefaultAsync(user => user.Username == username) != null;

        public async Task InsertUserAsync(User user)
        {
            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            User dbUser = await _context.User.FirstOrDefaultAsync(u => u.Id == user.Id) ?? throw new NotFoundException("Usuário não encontrado");
            dbUser.Email = user.Email;
            dbUser.Password = user.Password;
            dbUser.Username = user.Username;

            _context.User.Update(dbUser);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserByIdAsync(int id)
        {
            User dbUser = await _context.User.FirstOrDefaultAsync(u => u.Id == id) ?? throw new NotFoundException("Usuário não encontrado");

            _context.User.Remove(dbUser);
            await _context.SaveChangesAsync();
        }
    }
}