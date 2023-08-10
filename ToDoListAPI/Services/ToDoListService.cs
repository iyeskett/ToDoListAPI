using Microsoft.EntityFrameworkCore;
using ToDoListAPI.Data;
using ToDoListAPI.Models;
using ToDoListAPI.Services.Exceptions;

namespace ToDoListAPI.Services
{
    public class ToDoListService
    {
        private readonly ToDoListAPIContext _context;

        public ToDoListService(ToDoListAPIContext context)
        {
            _context = context;
        }

        public async Task<ToDoList> GetToDoListByIdAsync(int id) => await _context.ToDoList
            .Include(_ => _.User)
            .FirstOrDefaultAsync(_ => _.Id == id) ?? throw new NotFoundException("Lista de tarefas não encontrada");

        public IEnumerable<object> GetToDoListsByUserId(int userId) => _context.ToDoList
            .Select(_ =>
            new
            {
                _.Id,
                _.Name,
                User = new User { Id = _.UserId, Username = _.User.Username, Email = _.User.Email },
                _.Closed
            })
            .Where(_ => _.User.Id == userId).ToList() ?? throw new NotFoundException("Lista de tarefas não encontrada");

        public async Task InsertToDoListAsync(ToDoList toDoList)
        {
            await _context.ToDoList.AddAsync(toDoList);
            await _context.SaveChangesAsync();
        }

        public async Task<ToDoList> UpdateToDoListAsync(ToDoList toDoList)
        {
            ToDoList dbToDoList = await _context.ToDoList.FirstOrDefaultAsync(_ => _.Id == toDoList.Id) ?? throw new NotFoundException("Lista de tarefas não encontrada.");

            dbToDoList.Name = toDoList.Name;
            dbToDoList.Closed = toDoList.Closed;

            _context.ToDoList.Update(dbToDoList);
            await _context.SaveChangesAsync();

            return dbToDoList;
        }

        public async Task DeleteToDoListAsync(int toDoListId)
        {
            ToDoList dbToDoList = await _context.ToDoList.FirstOrDefaultAsync(_ => _.Id == toDoListId) ?? throw new NotFoundException("Lista de tarefas não encontrada.");

            _context.ToDoList.Remove(dbToDoList);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AddCollaboratorAsync(int toDoListId, int userId)
        {
            try
            {
                ToDoList dbToDoList = await _context.ToDoList.FirstOrDefaultAsync(_ => _.Id == toDoListId) ?? throw new NotFoundException("Lista de tarefas não encontrada");
                User dbUser = await _context.User.FirstOrDefaultAsync(_ => _.Id == userId) ?? throw new NotFoundException("Usuário não encontrado.");

                ToDoListCollaborator toDoListCollaborator = new() { ToDoListId = dbToDoList.Id, UserId = dbUser.Id };

                ToDoListCollaborator? dbToDoListCollaborator = await _context.ToDoListCollaborator.FirstOrDefaultAsync(_ => _.UserId == userId);

                if (dbToDoListCollaborator != null)
                    return false;

                await _context.ToDoListCollaborator.AddAsync(toDoListCollaborator);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task DeleteCollaboratorAsync(int toDoListId, int userId)
        {
            ToDoList dbToDoList = await _context.ToDoList.FirstOrDefaultAsync(_ => _.Id == toDoListId) ?? throw new NotFoundException("Lista de tarefas não encontrada");
            User dbUser = await _context.User.FirstOrDefaultAsync(_ => _.Id == userId) ?? throw new NotFoundException("Usuário não encontrado.");

            ToDoListCollaborator dbToDoListCollaborator = await _context.ToDoListCollaborator.FirstOrDefaultAsync(_ => _.ToDoListId == dbToDoList.Id &&
            _.UserId == dbUser.Id) ?? throw new NotFoundException($"Colaborador {dbUser.Username} #{dbUser.Id} não encontrado na lista de tarefas {dbToDoList.Name} #{dbToDoList.Id}");

            _context.ToDoListCollaborator.Remove(dbToDoListCollaborator);
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetCollaboratorsAsync(int toDoListId)
        {
            var toDoListcollaborators = await _context.ToDoListCollaborator.Where(_ => _.ToDoListId == toDoListId).ToListAsync();
            List<User> users = new();

            foreach (var item in toDoListcollaborators)
            {
                User? user = await _context.User.FirstOrDefaultAsync(_ => _.Id == item.UserId);

                if (user != null)
                    users.Add(user);
            }

            return users;
        }

        public async Task AddToDoAsync(int toDoId, int toDoListId)
        {
            ToDo dbToDo = await _context.ToDo.FirstOrDefaultAsync(_ => _.Id == toDoId) ?? throw new NotFoundException("Tarefa não encontrada.");
            ToDoList dbToDoList = await _context.ToDoList.FirstOrDefaultAsync(_ => _.Id == toDoListId) ?? throw new NotFoundException("Lista de tarefas não encontrada.");

            ToDosList toDoLists = new() { ToDoId = dbToDo.Id, ToDoListId = dbToDoList.Id };

            await _context.ToDoLists.AddAsync(toDoLists);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteToDoAsync(int toDoId, int toDoListId)
        {
            ToDo dbToDo = await _context.ToDo.FirstOrDefaultAsync(_ => _.Id == toDoId) ?? throw new NotFoundException("Tarefa não encontrada.");
            ToDoList dbToDoList = await _context.ToDoList.FirstOrDefaultAsync(_ => _.Id == toDoListId) ?? throw new NotFoundException("Lista de tarefas não encontrada.");

            ToDosList dbToDoLists = await _context.ToDoLists.FirstOrDefaultAsync() ??
                throw new NotFoundException($"Tarefa {dbToDo.Title} #{dbToDo.Id} não encontrada na lista de tarefas {dbToDoList.Name} #{dbToDoList.Id}");

            await _context.ToDoLists.AddAsync(dbToDoLists);
            await _context.SaveChangesAsync();
        }
    }
}