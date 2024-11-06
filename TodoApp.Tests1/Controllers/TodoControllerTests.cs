using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp.Controllers;
using TodoApp.Data;
using TodoApp.Models;
using Xunit;

namespace TodoApp.Tests1.Controllers
{
    public class TodoControllerTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly TodoController _controller;

        public TodoControllerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _controller = new TodoController(_context);
        }

        [Fact]
        public async Task GetTodos_ReturnsAllTodos()
        {
            _context.TODOItems.Add(new TodoItem { Title = "Todo 1", Description = "Description 1", IsCompleted = false });
            _context.TODOItems.Add(new TodoItem { Title = "Todo 2", Description = "Description 2", IsCompleted = true });
            await _context.SaveChangesAsync();

            var result = await _controller.GetTodos();

            var actionResult = Assert.IsType<ActionResult<IEnumerable<TodoItem>>>(result);
            var items = Assert.IsType<List<TodoItem>>(actionResult.Value);
            Assert.Equal(2, items.Count);
        }

        [Fact]
        public async Task GetTodoItem_ReturnsTodoItem_WhenIdExists()
        {
            var todo = new TodoItem { Title = "Test Todo", Description = "Description", IsCompleted = false };
            _context.TODOItems.Add(todo);
            await _context.SaveChangesAsync();

            var result = await _controller.GetTodoItem(todo.Id);

            var actionResult = Assert.IsType<ActionResult<TodoItem>>(result);
            var okResult = actionResult.Result as OkObjectResult;
            Assert.NotNull(okResult); // Ensure OkObjectResult is not null
            var item = okResult!.Value as TodoItem;
            Assert.NotNull(item); // Ensure the result's Value is not null
            Assert.Equal(todo.Id, item!.Id);
            Assert.Equal("Test Todo", item.Title);
        }

        [Fact]
        public async Task GetTodoItem_ReturnsNotFound_WhenIdDoesNotExist()
        {
            var result = await _controller.GetTodoItem(999);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PostTodoItem_CreatesNewTodo()
        {
            var newTodo = new TodoItem { Title = "New Todo", Description = "New Description", IsCompleted = false };

            var result = await _controller.PostTodoItem(newTodo);

            var actionResult = Assert.IsType<ActionResult<TodoItem>>(result);
            var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var createdTodo = Assert.IsType<TodoItem>(createdResult.Value);
            Assert.Equal(newTodo.Title, createdTodo.Title);
            Assert.Equal(newTodo.Description, createdTodo.Description);
        }

        [Fact]
        public async Task UpdateTodoItem_UpdatesExistingTodo()
        {
            var todo = new TodoItem { Title = "Original Title", Description = "Original Description", IsCompleted = false };
            _context.TODOItems.Add(todo);
            await _context.SaveChangesAsync();

            var updatedTodo = new TodoItem { Id = todo.Id, Title = "Updated Title", Description = "Updated Description", IsCompleted = true };

            var result = await _controller.UpdateTodoItem(todo.Id, updatedTodo);

            Assert.IsType<NoContentResult>(result);
            var item = await _context.TODOItems.FindAsync(todo.Id);
            Assert.NotNull(item);
            Assert.Equal("Updated Title", item!.Title);
            Assert.Equal("Updated Description", item.Description);
            Assert.True(item.IsCompleted);
        }

        [Fact]
        public async Task DeleteTodoItem_DeletesTodo_WhenIdExists()
        {
            var todo = new TodoItem { Title = "Todo to Delete", Description = "Description", IsCompleted = false };
            _context.TODOItems.Add(todo);
            await _context.SaveChangesAsync();

            var result = await _controller.DeleteTodoItem(todo.Id);

            Assert.IsType<NoContentResult>(result);
            var item = await _context.TODOItems.FindAsync(todo.Id);
            Assert.Null(item);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
