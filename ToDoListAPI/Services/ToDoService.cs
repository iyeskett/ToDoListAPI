using Microsoft.EntityFrameworkCore;
using ToDoListAPI.Data;
using ToDoListAPI.Models;
using ToDoListAPI.Services.Exceptions;

namespace ToDoListAPI.Services
{
    public class ToDoService
    {
        private readonly ToDoListAPIContext _context;

        public ToDoService(ToDoListAPIContext context)
        {
            _context = context;
        }

        public async Task<ToDo> GetToDoByIdAsync(int id) => await _context.ToDo.FirstOrDefaultAsync(_ => _.Id == id) ?? throw new NotFoundException("Tarefa não encontrada");

        public List<ToDo> GetToDoByUserIdAsync(int userId) => _context.ToDo.Where(_ => _.UserId == userId).ToList();

        public List<ToDo> GetToDoByToDoListIdAsync(int toDoListId) => _context.ToDo.Where(_ => _.ToDoListId == toDoListId).ToList();

        public async Task InsertToDoAsync(ToDo toDo)
        {
            await _context.ToDo.AddAsync(toDo);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateToDoAsync(ToDo toDo)
        {
            ToDo dbToDo = await _context.ToDo.FirstOrDefaultAsync(_ => _.Id == toDo.Id) ?? throw new NotFoundException("Tarefa não encontrada.");

            dbToDo.Title = toDo.Title;
            dbToDo.Description = toDo.Description;
            dbToDo.Done = toDo.Done;

            _context.Update(dbToDo);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteToDoAsync(int id)
        {
            ToDo dbToDo = await _context.ToDo.FirstOrDefaultAsync(_ => _.Id == id) ?? throw new NotFoundException("Tarefa não encontrada.");

            _context.Remove(dbToDo);
            await _context.SaveChangesAsync();
        }
    }
}