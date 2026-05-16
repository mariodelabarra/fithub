using AutoMapper;
using FitHub.Platform.Common.Service;
using FitHub.Platform.Workout.Domain.Tests;
using FitHub.Platform.Workout.Repository;
using FluentAssertions;
using Moq;

namespace FitHub.Platform.Workout.Service.Tests
{
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
                var expectedExercises = new ExerciseFaker().Generate(5);
                _exerciseRepositoryMock.Setup(s => s.GetAllAsync()).ReturnsAsync(expectedExercises);

                //Act
                var result = await _exerciseService.GetAllAsync();

                //Assert
                result.Should().Equal(expectedExercises);
            }
        }
    }
}
