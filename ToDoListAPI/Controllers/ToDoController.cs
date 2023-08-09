using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoListAPI.Models;
using ToDoListAPI.Services;
using ToDoListAPI.Services.Exceptions;

namespace ToDoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ToDoController : ControllerBase
    {
        private readonly ToDoService _toDoService;
        private readonly UserService _userService;

        public ToDoController(ToDoService toDoService, UserService userService)
        {
            _toDoService = toDoService;
            _userService = userService;
        }

        // GET: api/ToDo/1
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDo>> GetToDo(int id)
        {
            try
            {
                User dbUser = await _userService.GetUserByUsernameAsync(User.Identity.Name);

                ToDo dbToDo = await _toDoService.GetToDoByIdAsync(id);

                if (dbUser.Id != dbToDo.UserId)
                    return StatusCode(409, new { message = "Sem autorização para acessar essa tarefa." });

                return Ok(new { dbToDo.Id, dbToDo.Title, dbToDo.Description, dbToDo.UserId, dbToDo.Done });
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Erro ao pesquisar tarefa." });
            }
        }

        // POST: api/ToDo
        [HttpPost]
        public async Task<ActionResult<ToDo>> NewToDo(ToDo toDo)
        {
            try
            {
                User dbUser = await _userService.GetUserByUsernameAsync(User.Identity.Name);

                if (dbUser.Id != toDo.UserId)
                    return StatusCode(409, new { message = "Sem autorização para acessar essa tarefa." });

                toDo.User = dbUser;
                ModelState.Remove("User");
                if (ModelState.IsValid)
                    await _toDoService.InsertToDoAsync(toDo);
                else
                    return BadRequest(ModelState);

                return CreatedAtAction("GetToDo", new { message = "Tarefa criada com sucesso." }, toDo);
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

        // PUT: api/ToDos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateToDo(int id, ToDo toDo)
        {
            try
            {
                User dbUser = await _userService.GetUserByUsernameAsync(User.Identity.Name);

                ToDo dbToDo = await _toDoService.GetToDoByIdAsync(id);

                if (dbUser.Id != dbToDo.UserId)
                    return StatusCode(409, new { message = "Sem autorização para acessar essa tarefa." });

                dbToDo.Title = toDo.Title;
                dbToDo.Description = toDo.Description;

                ModelState.Remove("UserId");
                if (ModelState.IsValid)
                    await _toDoService.UpdateToDoAsync(dbToDo);
                else
                    return BadRequest(ModelState);

                return Created("GetToDo", new { dbToDo.Id, dbToDo.Title, dbToDo.Description, dbToDo.UserId, dbToDo.Done });
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

        // PUT: api/ToDos/setAsDone/5
        [HttpPut("setAsDone/{id}")]
        public async Task<IActionResult> SetAsDone(int id, [FromBody] bool done)
        {
            try
            {
                User dbUser = await _userService.GetUserByUsernameAsync(User.Identity.Name);

                ToDo dbToDo = await _toDoService.GetToDoByIdAsync(id);

                if (dbUser.Id != dbToDo.UserId)
                    return StatusCode(409, new { message = "Sem autorização para acessar essa tarefa." });

                dbToDo.Done = done;
                await _toDoService.UpdateToDoAsync(dbToDo);
                return Created("GetToDo", new { dbToDo.Id, dbToDo.Title, dbToDo.Description, dbToDo.UserId, dbToDo.Done });
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

        // DELETE: api/ToDos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDo(int id)
        {
            try
            {
                User dbUser = await _userService.GetUserByUsernameAsync(User.Identity.Name);

                ToDo dbToDo = await _toDoService.GetToDoByIdAsync(id);

                if (dbUser.Id != dbToDo.UserId)
                    return StatusCode(409, new { message = "Sem autorização para acessar essa tarefa." });

                await _toDoService.DeleteToDoAsync(id);
                return Ok(new { message = "Tarefa excluída com sucesso." });
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao excluir tarefa.");
            }
        }
    }
}