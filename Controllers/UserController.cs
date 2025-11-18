using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Models;

namespace UserManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private static List<User> _users = new()
    {
        new User { Id = 1, Name = "John Doe", Email = "john@example.com", Phone = "1234567890" },
        new User { Id = 2, Name = "Jane Smith", Email = "jane@example.com", Phone = "0987654321" }
    };
    private static int _nextId = 3;

    // GET: api/user
    [HttpGet]
    public ActionResult<IEnumerable<User>> GetUsers()
    {
        return Ok(_users);
    }

    // GET: api/user/5
    [HttpGet("{id}")]
    public ActionResult<User> GetUser(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);

        if (user == null)
        {
            return NotFound(new { error = $"User with ID {id} not found" });
        }

        return Ok(user);
    }

    // POST: api/user
    [HttpPost]
    public ActionResult<User> CreateUser([FromBody] User user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Check for duplicate email
        if (_users.Any(u => u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase)))
        {
            return BadRequest(new { error = "A user with this email already exists" });
        }

        user.Id = _nextId++;
        user.CreatedAt = DateTime.UtcNow;
        _users.Add(user);

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    // PUT: api/user/5
    [HttpPut("{id}")]
    public ActionResult<User> UpdateUser(int id, [FromBody] User updatedUser)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = _users.FirstOrDefault(u => u.Id == id);

        if (user == null)
        {
            return NotFound(new { error = $"User with ID {id} not found" });
        }

        // Check for duplicate email (excluding current user)
        if (_users.Any(u => u.Id != id && u.Email.Equals(updatedUser.Email, StringComparison.OrdinalIgnoreCase)))
        {
            return BadRequest(new { error = "A user with this email already exists" });
        }

        user.Name = updatedUser.Name;
        user.Email = updatedUser.Email;
        user.Phone = updatedUser.Phone;

        return Ok(user);
    }

    // DELETE: api/user/5
    [HttpDelete("{id}")]
    public ActionResult DeleteUser(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);

        if (user == null)
        {
            return NotFound(new { error = $"User with ID {id} not found" });
        }

        _users.Remove(user);
        return NoContent();
    }
}
