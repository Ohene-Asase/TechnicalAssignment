using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechnicalAssignment.Models;
using TechnicalAssignment.Repositories;

namespace TechnicalAssignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public UsersController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            var users = _userRepository.GetUsers();
            if (users == null)
            {
                return NotFound();
            }
            return Ok(users);
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetUserById(int id)
        {
            var user = _userRepository.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost]
        public ActionResult<User> CreateUser(User user)
        {
            _userRepository.AddUser(user);

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _userRepository.UpdateUser(user);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            _userRepository.DeleteUser(id);

            return NoContent();
        }
    }
}
