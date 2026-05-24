using Fithub.Platform.Domain.Agglestone;
using Fithub.Platform.Services.Agglestone;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Fithub.Platform.Services.Tests.Agglestone;

public class AgglestoneUserServiceTests
{
    private const string TenantId = "test-tenant";
    private const string UserId = "user-123";

    private readonly Mock<IHttpClientFactory> _httpClientFactoryMock = new();
    private readonly Mock<IMemoryCache> _cacheMock = new();
    private readonly Mock<ICacheEntry> _cacheEntryMock = new();
    private readonly AgglestoneUserService _sut;

    public AgglestoneUserServiceTests()
    {
        var settingsMock = new Mock<IOptions<AgglestoneSettings>>();
        settingsMock.Setup(s => s.Value).Returns(new AgglestoneSettings { TenantId = TenantId });
        _cacheMock.Setup(c => c.CreateEntry(It.IsAny<object>())).Returns(_cacheEntryMock.Object);

        _sut = new AgglestoneUserService(_httpClientFactoryMock.Object, _cacheMock.Object, settingsMock.Object);
    }

    private void SetupCacheMiss()
    {
        object? nullValue = null;
        _cacheMock.Setup(c => c.TryGetValue(It.IsAny<object>(), out nullValue)).Returns(false);
    }

    private void SetupCacheHit(AgglestoneUserDto user)
    {
        object? cached = user;
        _cacheMock.Setup(c => c.TryGetValue(It.IsAny<object>(), out cached)).Returns(true);
    }

    private void SetupHttpHandler(HttpResponseMessage response)
    {
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://auth.agglestone.com/")
        };
        _httpClientFactoryMock.Setup(f => f.CreateClient("agglestone")).Returns(httpClient);
    }

    private static StringContent CreateJsonContent<T>(T value) =>
        new(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");

    public class GetUserAsync : AgglestoneUserServiceTests
    {
        [Fact]
        public async Task Should_ReturnCachedUser_WhenCacheHit()
        {
            // Arrange
            var expectedUser = new AgglestoneUserDto(UserId, "cached@example.com", "Cached User");
            SetupCacheHit(expectedUser);

            // Act
            var result = await _sut.GetUserAsync(UserId);

            // Assert
            result.Should().Be(expectedUser);
        }

        [Fact]
        public async Task Should_NotCallHttpClientFactory_WhenCacheHit()
        {
            // Arrange
            var cachedUser = new AgglestoneUserDto(UserId, "cached@example.com", "Cached User");
            SetupCacheHit(cachedUser);

            // Act
            await _sut.GetUserAsync(UserId);

            // Assert
            _httpClientFactoryMock.Verify(f => f.CreateClient(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Should_ReturnMappedUser_WhenCacheMissAndHttpSucceeds()
        {
            // Arrange
            SetupCacheMiss();
            SetupHttpHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = CreateJsonContent(new { Id = "api-user-id", Email = "api@example.com", DisplayName = "API User" })
            });

            // Act
            var result = await _sut.GetUserAsync(UserId);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be("api-user-id");
            result.Email.Should().Be("api@example.com");
            result.DisplayName.Should().Be("API User");
        }

        [Theory]
        [InlineData(HttpStatusCode.NotFound)]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.Unauthorized)]
        public async Task Should_ReturnNull_WhenHttpResponseIsNotSuccess(HttpStatusCode statusCode)
        {
            // Arrange
            SetupCacheMiss();
            SetupHttpHandler(new HttpResponseMessage(statusCode));

            // Act
            var result = await _sut.GetUserAsync(UserId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Should_ReturnNull_WhenHttpSuccessButDeserializationReturnsNull()
        {
            // Arrange
            SetupCacheMiss();
            SetupHttpHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("null", Encoding.UTF8, "application/json")
            });

            // Act
            var result = await _sut.GetUserAsync(UserId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Should_UseUserIdFallback_WhenApiIdIsNull()
        {
            // Arrange
            SetupCacheMiss();
            SetupHttpHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = CreateJsonContent(new { Id = (string?)null, Email = "api@example.com", DisplayName = "API User" })
            });

            // Act
            var result = await _sut.GetUserAsync(UserId);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(UserId);
        }

        [Fact]
        public async Task Should_UseEmptyStringForEmail_WhenApiEmailIsNull()
        {
            // Arrange
            SetupCacheMiss();
            SetupHttpHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = CreateJsonContent(new { Id = "api-user-id", Email = (string?)null, DisplayName = "API User" })
            });

            // Act
            var result = await _sut.GetUserAsync(UserId);

            // Assert
            result.Should().NotBeNull();
            result!.Email.Should().Be(string.Empty);
        }

        [Fact]
        public async Task Should_MapNullDisplayName_WhenApiDisplayNameIsNull()
        {
            // Arrange
            SetupCacheMiss();
            SetupHttpHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = CreateJsonContent(new { Id = "api-user-id", Email = "api@example.com", DisplayName = (string?)null })
            });

            // Act
            var result = await _sut.GetUserAsync(UserId);

            // Assert
            result.Should().NotBeNull();
            result!.DisplayName.Should().BeNull();
        }

        [Fact]
        public async Task Should_SetUserInCache_AfterSuccessfulHttpResponse()
        {
            // Arrange
            SetupCacheMiss();
            SetupHttpHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = CreateJsonContent(new { Id = "api-user-id", Email = "api@example.com", DisplayName = "API User" })
            });

            // Act
            await _sut.GetUserAsync(UserId);

            // Assert
            _cacheMock.Verify(c => c.CreateEntry($"agglestone:user:{UserId}"), Times.Once);
        }

        [Fact]
        public async Task Should_NotSetCache_WhenHttpResponseIsNotSuccess()
        {
            // Arrange
            SetupCacheMiss();
            SetupHttpHandler(new HttpResponseMessage(HttpStatusCode.NotFound));

            // Act
            await _sut.GetUserAsync(UserId);

            // Assert
            _cacheMock.Verify(c => c.CreateEntry(It.IsAny<object>()), Times.Never);
        }
    }
}
