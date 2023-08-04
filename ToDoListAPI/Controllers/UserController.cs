using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ToDoListAPI.Data;
using ToDoListAPI.Models;
using ToDoListAPI.Services;
using ToDoListAPI.Services.Exceptions;

namespace ToDoListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            try
            {
                User user = await _userService.GetUserByIdAsync(id);

                return StatusCode(200, new { user.Id, user.Email, user.Username });
            }
            catch (NotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao buscar usuário");
            }
        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<User>> NewUser(User user)
        {
            try
            {
                if (ModelState.IsValid)
                    await _userService.InsertUserAsync(user);
                else
                    return BadRequest(ModelState);

                return CreatedAtAction("GetUser", new { id = user.Id }, user);
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao criar usuário");
            }
        }

        // PUT: api/User
        [HttpPut]
        public async Task<ActionResult<User>> UpdateUser(User user)
        {
            try
            {
                if (ModelState.IsValid)
                    await _userService.UpdateUserAsync(user);
                else
                    return BadRequest(ModelState);

                return CreatedAtAction("NewUser", user);
            }
            catch (NotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao atualizar usuário");
            }
        }

        // DELETE: api/User
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                await _userService.DeleteUserByIdAsync(id);
                return Ok();
            }
            catch (NotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao excluir usuário");
            }
        }
    }
}