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

                if (user == null)
                    return NotFound();

                return StatusCode(200, new { user.Id, user.Email, user.Username });
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
                    await _userService.InsertUser(user);
                else
                    return BadRequest(ModelState);

                return CreatedAtAction("GetUser", new { id = user.Id }, user);
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao criar usuário");
            }
        }
    }
}