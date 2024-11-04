using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TodoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/todo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodos()
        {
            // Obtiene la lista de todas las tareas desde la base de datos
            return await _context.TODOItems.ToListAsync();
        }

        // GET: api/todo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(int id)
        {
            var todoItem = await _context.TODOItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound(); // 404 Not Found si la tarea no existe
            }

            return Ok(todoItem); // 200 OK, retorna la tarea encontrada
        }

        // POST: api/todo
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // 400 Bad Request si la validación falla
            }

            _context.TODOItems.Add(todoItem);
            await _context.SaveChangesAsync();

            // 201 Created con la ubicación del recurso creado
            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
        }

        // PUT: api/todo/5
       // PUT: api/todo/5
[HttpPut("{id}")]
public async Task<IActionResult> UpdateTaskStatus(int id, [FromBody] bool isComplete)
{
    var todoItem = await _context.TODOItems.FindAsync(id);
    if (todoItem == null)
    {
        return NotFound(); // 404 Not Found si la tarea no existe
    }

    // Actualizar solo el estado `isComplete`
    todoItem.IsCompleted = isComplete;

    try
    {
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
        return StatusCode(500, "Concurrency error while updating"); // 500 Internal Server Error
    }

    return NoContent(); // 204 No Content para indicar éxito sin cuerpo de respuesta
}


        // DELETE: api/todo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(int id)
        {
            var todoItem = await _context.TODOItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound(); // 404 Not Found si la tarea no existe
            }

            _context.TODOItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent(); // 204 No Content, indica una eliminación exitosa sin cuerpo de respuesta
        }

        // Método auxiliar para verificar si una tarea existe
        private bool TodoItemExists(int id)
        {
            return _context.TODOItems.Any(e => e.Id == id);
        }
    }
}
