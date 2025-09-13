using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Api.Controllers.Task;
using TaskManager.Application.DTOs.Task;
using TaskManager.Application.Interfaces.Services;
using TaskManager.Domain.Base;
using Xunit;

namespace TaskManager.Tests.Controllers
{
    public class TasksControllerTests
    {
        private readonly Mock<ITareaService> _tareaServiceMock;
        private readonly TareaController _controller;

        public TasksControllerTests()
        {
            _tareaServiceMock = new Mock<ITareaService>();
            _controller = new TareaController(_tareaServiceMock.Object);
        }

        [Fact]
        public async Task GetAllTask_ReturnsOk_WhenSuccess()
        {
            var expected = OperationResult.Success("ok", new List<TareaDto>());
            _tareaServiceMock.Setup(s => s.GetAllTareaAsync()).ReturnsAsync(expected);

            var result = await _controller.Get();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expected, okResult.Value);
        }

        [Fact]
        public async Task GetAllTask_ReturnsBadRequest_WhenFailure()
        {
            var expected = OperationResult.Failure("Error retrieving tasks");
            _tareaServiceMock.Setup(s => s.GetAllTareaAsync()).ReturnsAsync(expected);

            var result = await _controller.Get();

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(expected, badRequest.Value);
        }

        [Fact]
        public async Task GetTaskById_ReturnsOk_WhenSuccess()
        {
            var expected = OperationResult.Success("ok", new TareaDto { Id = 1 });
            _tareaServiceMock.Setup(s => s.GetTareaByIdAsync(1)).ReturnsAsync(expected);

            var result = await _controller.Get(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expected, okResult.Value);
        }

        [Fact]
        public async Task AddTask_ReturnsOk_WhenSuccess()
        {
            var dto = new[] { new TareaAddDto { Description = "Test Task", DueDate = default, Status = "Pending" } };
            var expected = OperationResult.Success("Created", dto);
            _tareaServiceMock.Setup(s => s.CreateTareaAsync(dto)).ReturnsAsync(expected);

            var result = await _controller.Post(dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expected, okResult.Value);
        }

        [Fact]
        public async Task AddTaskHighPriority_ReturnsOk_WhenSuccess()
        {
            var expected = OperationResult.Success("Created", new object());
            _tareaServiceMock.Setup(s => s.CreateTaskHighPriority("High Priority")).ReturnsAsync(expected);

            var result = await _controller.PostHighPriority("High Priority");

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expected, okResult.Value);
        }

        [Fact]
        public async Task UpdateTask_ReturnsOk_WhenSuccess()
        {
            var dto = new TareaUpdateDto
            {
                Id = 1,
                Description = "Updated",
                DueDate = default,
                Status = "Pending"
            };
            var expected = OperationResult.Success("Updated", dto);
            _tareaServiceMock.Setup(s => s.UpdateTareaAsync(dto)).ReturnsAsync(expected);

            var result = await _controller.Put(1, dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expected, okResult.Value);
        }

        [Fact]
        public async Task UpdateTask_ReturnsBadRequest_WhenIdMismatch()
        {
            var dto = new TareaUpdateDto
            {
                Id = 2,
                Description = "Updated",
                DueDate = default,
                Status = "Pending"
            };
            var result = await _controller.Put(1, dto);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("no match", badRequest.Value.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task DeleteTask_ReturnsOk_WhenSuccess()
        {
            var expected = OperationResult.Success("Deleted", new object());
            _tareaServiceMock.Setup(s => s.DeleteTareaAsync(1)).ReturnsAsync(expected);

            var result = await _controller.Delete(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expected, okResult.Value);
        }

        [Fact]
        public async Task FindByDueDate_ReturnsOk_WhenSuccess()
        {
            var date = DateOnly.FromDateTime(DateTime.Today);
            var expected = OperationResult.Success("Found", new List<TareaDto>());
            _tareaServiceMock.Setup(s => s.FindByDueDateAsync(date)).ReturnsAsync(expected);

            var result = await _controller.FindByDueDate(date);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expected, okResult.Value);
        }

        [Fact]
        public async Task FindByStatus_ReturnsOk_WhenSuccess()
        {
            var expected = OperationResult.Success("Found", new List<TareaDto>());
            _tareaServiceMock.Setup(s => s.FindByStatusAsync("Pendiente")).ReturnsAsync(expected);

            var result = await _controller.FindByStatus("Pendiente");

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expected, okResult.Value);
        }
    }
}
