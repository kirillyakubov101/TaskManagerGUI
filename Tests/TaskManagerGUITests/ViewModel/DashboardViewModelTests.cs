using FluentAssertions;
using Moq;
using SharedModels;
using TaskManagerGUI.Interfaces;
using Xunit;

namespace TaskManagerGUI.ViewModel.Tests;

public class DashboardViewModelTests
{
    [Fact()]
    public async void PopulateUserTaskList_CorrectUse_ShouldSucceed()
    {
        // Arrange
        var mockAuthHandler = new Mock<IAuthHandler>();
        var mockNavigationService = new Mock<INavigationService>();
        var mockLoginEnterHandler = new Mock<ILoginEnterHandler>();
        var mockServiceProvider = new Mock<IServiceProvider>();
        var mockDeleteTaskHandler = new Mock<IDeleteTaskHander>();

        var sampleTasks = new List<UserTaskDto>
        {
            new UserTaskDto { Id = 1, Title = "Task 1", Description = "Description 1" },
            new UserTaskDto { Id = 2, Title = "Task 2", Description = "Description 2" }
        };

        mockLoginEnterHandler.Setup(x => x.GetAllUserTasks()).ReturnsAsync(sampleTasks);

        var viewModel = new DashboardViewModel(
           mockAuthHandler.Object,
           mockNavigationService.Object,
           mockLoginEnterHandler.Object,
           mockServiceProvider.Object,
           mockDeleteTaskHandler.Object
       );

        // Act
       await viewModel.PopulateUserTaskList();

        // Assert

        viewModel.Tasks.Should().HaveCount(2);
        viewModel.Tasks.Should().Contain(t => t.Title == "Task 1" && t.Description == "Description 1"); // Verify Task 1's title and description
        viewModel.Tasks.Should().Contain(t => t.Title == "Task 2" && t.Description == "Description 2"); // Verify Task 2's title and description
    }
}