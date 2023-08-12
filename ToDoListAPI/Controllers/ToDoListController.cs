using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoListAPI.Data;
using ToDoListAPI.Migrations;
using ToDoListAPI.Models;
using ToDoListAPI.Services;
using ToDoListAPI.Services.Exceptions;

namespace ToDoListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ToDoListController : ControllerBase
    {
        private readonly ToDoListService _toDoListService;
        private readonly ToDoService _toDoService;
        private readonly UserService _userService;

        public ToDoListController(ToDoListService toDoListService, UserService userService, ToDoService toDoService)
        {
            _toDoListService = toDoListService;
            _userService = userService;
            _toDoService = toDoService;
        }

        // GET: api/ToDoLists/1
        [HttpGet("All/{userId}")]
        public async Task<ActionResult<IEnumerable<ToDoList>>> GetToDoLists(int userId)
        {
            try
            {
                User dbUser = await _userService.GetUserByIdAsync(userId);

                if (dbUser.Username != User.Identity.Name)
                    return StatusCode(403, new { message = "Sem autorização." });

                IEnumerable<object> toDoLists = _toDoListService.GetToDoListsByUserId(userId);

                return Ok(new { ToDoLists = toDoLists });
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Erro ao pesquisar listas de tarefas." });
            }
        }

        // GET: api/ToDoList/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoList>> GetToDoList(int id)
        {
            try
            {
                ToDoList dbToDoList = await _toDoListService.GetToDoListByIdAsync(id);

                User dbUser = await _userService.GetUserByIdAsync(dbToDoList.User.Id);

                if (dbUser.Username != User.Identity.Name)
                    return StatusCode(403, new { message = "Sem autorização." });

                var dbToDos = _toDoService.GetToDoByToDoListIdAsync(dbToDoList.Id)
                    .Select(_ => new
                    {
                        _.Id,
                        _.Title,
                        _.Description
                    });

                return Ok(
                    new
                    {
                        dbToDoList.Id,
                        dbToDoList.Name,
                        dbToDoList.UserId,
                        dbToDoList.Closed,
                        ToDos = dbToDos
                    });
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Erro ao pesquisar lista de tarefas." });
            }
        }

        // PUT: api/ToDoList/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateToDoList(int id, ToDoList toDoList)
        {
            try
            {
                ToDoList dbToDoList;

                dbToDoList = await _toDoListService.GetToDoListByIdAsync(id);

                User dbUser = await _userService.GetUserByIdAsync(dbToDoList.User.Id);

                if (dbUser.Username != User.Identity.Name)
                    return StatusCode(403, new { message = "Sem autorização." });

                toDoList.Id = id;
                dbToDoList = await _toDoListService.UpdateToDoListAsync(toDoList);

                return Ok(
                    new
                    {
                        dbToDoList.Id,
                        dbToDoList.Name,
                        User = new User { Id = dbToDoList.UserId, Username = dbToDoList.User.Username, Email = dbToDoList.User.Email },
                        dbToDoList.Closed
                    });
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Erro ao pesquisar lista de tarefas." });
            }
        }

        // POST: api/ToDoList
        [HttpPost]
        public async Task<ActionResult<ToDoList>> NewToDoList(ToDoList toDoList)
        {
            try
            {
                User dbUser = await _userService.GetUserByUsernameAsync(User.Identity.Name);

                toDoList.User = dbUser;
                toDoList.UserId = dbUser.Id;
                ModelState.Remove("Collaborators");
                if (ModelState.IsValid)
                    await _toDoListService.InsertToDoListAsync(toDoList);
                else
                    return BadRequest(ModelState);

                return StatusCode(201, new { message = "Tarefa criada com sucesso." });
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Erro ao pesquisar listas de tarefas." });
            }
        }

        // DELETE: api/ToDoList/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoList(int id)
        {
            try
            {
                User dbUser = await _userService.GetUserByUsernameAsync(User.Identity.Name);

                ToDoList dbToDoList = await _toDoListService.GetToDoListByIdAsync(id);

                if (dbUser.Id != dbToDoList.UserId)
                    return StatusCode(403, new { message = "Sem autorização para acessar essa lista de tarefas." });

                await _toDoListService.DeleteToDoListAsync(id);
                return Ok(new { message = "Lista de tarefas excluída com sucesso." });
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao excluir lista de tarefas.");
            }
        }

        // PUT: api/ToDos/setAsClosed/5
        [HttpPut("setAsClosed/{id}")]
        public async Task<IActionResult> SetAsClosed(int id, bool closed)
        {
            try
            {
                User dbUser = await _userService.GetUserByUsernameAsync(User.Identity.Name);

                ToDoList dbToDoList = await _toDoListService.GetToDoListByIdAsync(id);

                if (dbUser.Id != dbToDoList.UserId)
                    return StatusCode(403, new { message = "Sem autorização para acessar essa tarefa." });

                dbToDoList.Closed = closed;
                await _toDoListService.UpdateToDoListAsync(dbToDoList);
                return Created("GetToDoList",
                    new
                    {
                        dbToDoList.Id,
                        dbToDoList.Name,
                        User = new User { Id = dbToDoList.UserId, Username = dbToDoList.User.Username, Email = dbToDoList.User.Email },
                        dbToDoList.Closed
                    });
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Erro ao atualizar tarefa." });
            }
        }

        [HttpGet("Collaborator/{toDoListId}")]
        public async Task<ActionResult> GetCollaborators(int toDoListId)
        {
            User dbUser = await _userService.GetUserByUsernameAsync(User.Identity.Name);
            ToDoList dbToDoList = await _toDoListService.GetToDoListByIdAsync(toDoListId);

            if (dbToDoList.UserId != dbUser.Id)
                return StatusCode(403, new { message = "Sem autorização." });

            List<User> collaborators = await _toDoListService.GetCollaboratorsAsync(dbToDoList.Id);

            return Ok(collaborators.Select(_ => new
            {
                _.Id,
                _.Username,
                _.Email
            }));
        }

        [HttpPost("Collaborator/{toDoListId}/{userId}")]
        public async Task<ActionResult> AddCollaborator(int toDoListId, int userId)
        {
            try
            {
                User dbUser = await _userService.GetUserByUsernameAsync(User.Identity.Name);
                ToDoList dbToDoList = await _toDoListService.GetToDoListByIdAsync(toDoListId);

                if (dbToDoList.UserId != dbUser.Id)
                    return StatusCode(403, new { message = "Sem autorização." });

                dbUser = await _userService.GetUserByIdAsync(userId);

                bool added = await _toDoListService.AddCollaboratorAsync(toDoListId, userId);
                if (!added)
                    return StatusCode(409, new { message = $"O usuário {dbUser.Username} #{dbUser.Id} já é um colaborador da lista de tarefas {dbToDoList.Name} #{dbToDoList.Id}" });

                return Ok(new { message = $"Colaborador {dbUser.Username} #{dbUser.Id} adicionado a lista de tarefas {dbToDoList.Name} #{dbToDoList.Id}" });
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Erro ao atualizar tarefa." });
            }
        }

        [HttpDelete("Collaborator/{toDoListId}/{userId}")]
        public async Task<ActionResult> DeleteCollaborator(int toDoListId, int userId)
        {
            try
            {
                User dbUser = await _userService.GetUserByUsernameAsync(User.Identity.Name);
                ToDoList dbToDoList = await _toDoListService.GetToDoListByIdAsync(toDoListId);

                if (dbToDoList.UserId != dbUser.Id)
                    return StatusCode(403, new { message = "Sem autorização." });

                await _toDoListService.DeleteCollaboratorAsync(toDoListId, userId);

                dbUser = await _userService.GetUserByIdAsync(userId);

                return Ok(new { message = $"Colaborador {dbUser.Username} #{dbUser.Id} removido da lista de tarefas {dbToDoList.Name} #{dbToDoList.Id}" });
            }
            catch (NotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Erro ao atualizar tarefa." });
            }
        }

        [HttpPost("ToDo/{toDoListId}/{toDoId}")]
        public async Task<ActionResult> AddToDo(int toDoListId, int toDoId)
        {
            try
            {
                ToDo dbToDo = await _toDoService.GetToDoByIdAsync(toDoId);
                ToDoList dbToDoList = await _toDoListService.GetToDoListByIdAsync(toDoListId);
                User dbUser = await _userService.GetUserByIdAsync(dbToDo.UserId);

                if (User.Identity.Name != dbUser.Username)
                    return StatusCode(403, new { message = "Sem autorização." });

                if (dbToDo.ToDoListId != null)
                    return StatusCode(409, new { message = $"A tarefa {dbToDo.Title} #{dbToDo.Id} já está em uma lista de tarefas" });
                if (dbToDoList.Closed)
                    return StatusCode(409, new { message = $"A lista de tarefas {dbToDoList.Name} #{dbToDoList.Id} está fechada" });

                await _toDoListService.AddToDoAsync(toDoId, toDoListId);

                return Ok(new { message = $"Tarefa {dbToDo.Title} #{dbToDo.Id} adicionada a lista de tarefas {dbToDoList.Name} #{dbToDoList.Id}" });
            }
            catch (NotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Erro ao adicionar tarefa nas lista de tarefas." });
            }
        }

        [HttpDelete("ToDo/{toDoListId}/{toDoId}")]
        public async Task<ActionResult> RemoveToDo(int toDoListId, int toDoId)
        {
            try
            {
                ToDo dbToDo = await _toDoService.GetToDoByIdAsync(toDoId);
                ToDoList dbToDoList = await _toDoListService.GetToDoListByIdAsync(toDoListId);
                User dbUser = await _userService.GetUserByIdAsync(dbToDo.UserId);

                if (User.Identity.Name != dbUser.Username)
                    return StatusCode(403, new { message = "Sem autorização." });

                await _toDoListService.DeleteToDoAsync(toDoId, toDoListId);

                return Ok(new { message = $"Tarefa {dbToDo.Title} #{dbToDo.Id} removida da lista de tarefas {dbToDoList.Name} #{dbToDoList.Id}" });
            }
            catch (NotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Erro ao adicionar tarefa nas lista de tarefas." });
            }
        }
    }
}