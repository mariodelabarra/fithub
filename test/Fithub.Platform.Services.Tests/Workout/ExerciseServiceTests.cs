using AutoMapper;
using Fithub.Platform.Domain.Workout;
using Fithub.Platform.Repositories.Workout;
using Fithub.Platform.Services.Workout;
using FitHub.Platform.Common.Service;
using FluentAssertions;
using Moq;

namespace Fithub.Platform.Services.Tests.Workout;

public class ExerciseServiceTests
{
    private readonly ExerciseService _exerciseService;

    private readonly Mock<IExerciseRepository> _exerciseRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IValidatorService> _validatorService = new();

    public ExerciseServiceTests()
    {
        _exerciseService = new(_exerciseRepositoryMock.Object, _mapperMock.Object, _validatorService.Object);
    }

    public class GetAllAsync : ExerciseServiceTests
    {
        [Fact]
        public async Task Should_Succeed()
        {
            //Arrange
            var expectedExercises = new List<Exercise>
            {
                new() { Name = "Push-up", Description = "A basic upper body exercise" },
                new() { Name = "Squat", Description = "A basic lower body exercise" }
            };
            _exerciseRepositoryMock.Setup(s => s.GetAllAsync()).ReturnsAsync(expectedExercises);

            //Act
            var result = await _exerciseService.GetAllAsync();

            //Assert
            result.Should().Equal(expectedExercises);
        }
    }
}
