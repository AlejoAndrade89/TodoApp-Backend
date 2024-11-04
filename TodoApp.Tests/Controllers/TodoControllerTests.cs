using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp.Controllers;
using TodoApp.Data;
using TodoApp.Models;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace TodoApp.Tests.Controllers
{
    public class TodoControllerTests
    {
        private readonly Mock<AppDbContext> _mockContext;
        private readonly TodoController _controller;

        public TodoControllerTests()
        {
            _mockContext = new Mock<AppDbContext>();
            _controller = new TodoController(_mockContext.Object);
        }

        [Fact]
        public async Task GetTodoItem_ReturnsNotFound_WhenItemDoesNotExist()
        {
            // Arrange
            _mockContext.Setup(context => context.TODOItems.FindAsync(It.IsAny<int>()))
                        .ReturnsAsync(default(TodoItem));

            // Act
            var result = await _controller.GetTodoItem(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetTodoItem_ReturnsOk_WithExistingItem()
        {
            // Arrange
            var todoItem = new TodoItem { Id = 1, Title = "Test Todo", Description = "Test Description", IsCompleted = false };
            _mockContext.Setup(context => context.TODOItems.FindAsync(1))
                        .ReturnsAsync(todoItem);

            // Act
            var result = await _controller.GetTodoItem(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(todoItem, okResult.Value);
        }

        [Fact]
        public async Task PostTodoItem_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Title", "Required");

            var todoItem = new TodoItem { Description = "Test Description" }; // Missing Title

            // Act
            var result = await _controller.PostTodoItem(todoItem);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task PostTodoItem_ReturnsCreatedAtAction_WhenItemIsCreated()
        {
            // Arrange
            var todoItem = new TodoItem { Id = 1, Title = "Test Todo", Description = "Test Description" };
            _mockContext.Setup(context => context.TODOItems.Add(todoItem)).Verifiable();
            _mockContext.Setup(context => context.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = await _controller.PostTodoItem(todoItem);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal("GetTodoItem", createdAtActionResult.ActionName);
            Assert.Equal(todoItem, createdAtActionResult.Value);
        }

        [Fact]
        public async Task PutTodoItem_ReturnsNoContent_WhenItemIsUpdated()
        {
            // Arrange
            var todoItem = new TodoItem { Id = 1, Title = "Updated Title", Description = "Updated Description" };
            _mockContext.Setup(context => context.TODOItems.FindAsync(todoItem.Id))
                        .ReturnsAsync(todoItem);
            _mockContext.Setup(context => context.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = await _controller.PutTodoItem(todoItem.Id, todoItem);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutTodoItem_ReturnsBadRequest_WhenIdMismatch()
        {
            // Arrange
            var todoItem = new TodoItem { Id = 1, Title = "Test Todo", Description = "Test Description" };

            // Act
            var result = await _controller.PutTodoItem(2, todoItem);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteTodoItem_ReturnsNoContent_WhenItemIsDeleted()
        {
            // Arrange
            var todoItem = new TodoItem { Id = 1, Title = "Delete Todo" };
            _mockContext.Setup(context => context.TODOItems.FindAsync(todoItem.Id))
                        .ReturnsAsync(todoItem);
            _mockContext.Setup(context => context.TODOItems.Remove(todoItem)).Verifiable();

            // Act
            var result = await _controller.DeleteTodoItem(todoItem.Id);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockContext.Verify(context => context.TODOItems.Remove(todoItem), Times.Once);
        }

        [Fact]
        public async Task DeleteTodoItem_ReturnsNotFound_WhenItemDoesNotExist()
        {
            // Arrange
            _mockContext.Setup(context => context.TODOItems.FindAsync(It.IsAny<int>()))
                        .ReturnsAsync((TodoItem)null);

            // Act
            var result = await _controller.DeleteTodoItem(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
