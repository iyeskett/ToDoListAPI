using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoListAPI.Models;
using ToDoListAPI.Services;
using ToDoListAPI.Services.Exceptions;

namespace ToDoListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        [AllowAnonymous]
        public async Task<ActionResult<User>> NewUser(User user)
        {
            try
            {
                bool isDuplicate = await _userService.IsDuplicateEmail(user.Email);
                if (isDuplicate)
                    return Conflict(new { message = "Já existe uma conta com esse email" });

                isDuplicate = await _userService.IsDuplicateUsername(user.Username);
                if (isDuplicate)
                    return Conflict(new { message = "Já existe uma conta com esse usuário" });

                user.Role = "general";
                ModelState.Remove("Role");
                if (ModelState.IsValid)
                    await _userService.InsertUserAsync(user);
                else
                    return BadRequest(ModelState);

                return Created("GetUser", new { user.Id, user.Email, user.Username });
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
        [Authorize(Roles = "admin")]
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

        // POST: api/User/Login
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(UserLogin user)
        {
            try
            {
                User dbUser = await _userService.GetUserByEmailAsync(user.Email);

                if (dbUser.Password != user.Password)
                    return Unauthorized(new { message = "Senha incorreta" });

                // Generate token
                var token = TokenService.GenerateToken(dbUser);

                return Ok(new { dbUser.Username, token });
            }
            catch (NotFoundException e)
            {
                return NotFound(new { message = e.Message });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}