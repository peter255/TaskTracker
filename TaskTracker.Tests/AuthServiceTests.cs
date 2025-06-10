using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using TaskTracker.Application.DTOs;
using TaskTracker.Application.Interfaces.Services;
using TaskTracker.Domain.Entities;
using TaskTracker.Infrastructure.Persistence;
using TaskTracker.Infrastructure.Services;

namespace TaskTracker.Tests;

public class AuthServiceTests
{
    private readonly Mock<IConfiguration> _mockConfig;
    private readonly AppDbContext _context;
    private readonly IAuthService _authService;

    public AuthServiceTests()
    {
        // Setup in-memory database
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
        _context = new AppDbContext(options);

        // Setup configuration mock
        _mockConfig = new Mock<IConfiguration>();
        _mockConfig.Setup(x => x["Jwt:Key"]).Returns("SuperSecretKey_MustBe32CharsMin!!");
        _mockConfig.Setup(x => x["Jwt:Issuer"]).Returns("TestIssuer");
        _mockConfig.Setup(x => x["Jwt:Audience"]).Returns("TestAudience");

        _authService = new AuthService(_context, _mockConfig.Object);
    }

    [Fact]
    public async Task RegisterAsync_ValidUser_ReturnsAuthResult()
    {
        // Arrange
        var registerDto = new RegisterDto
        {
            Username = "testuser",
            Email = "test@example.com",
            Password = "password123"
        };

        // Act
        var result = await _authService.RegisterAsync(registerDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(registerDto.Username, result.Username);
        Assert.Equal(registerDto.Email, result.Email);
        Assert.NotNull(result.Token);
        Assert.NotEmpty(result.Token);

        // Verify user was added to database
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == registerDto.Username);
        Assert.NotNull(user);
        Assert.Equal(registerDto.Email, user.Email);
    }

    [Fact]
    public async Task LoginAsync_ValidCredentials_ReturnsAuthResult()
    {
        // Arrange
        var password = "password123";
        var user = new User
        {
            Username = "testuser",
            Email = "test@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
        };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var loginDto = new LoginDto
        {
            Username = "testuser",
            Password = password
        };

        // Act
        var result = await _authService.LoginAsync(loginDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Username, result.Username);
        Assert.Equal(user.Email, result.Email);
        Assert.NotNull(result.Token);
        Assert.NotEmpty(result.Token);
    }

    [Fact]
    public async Task LoginAsync_InvalidUsername_ReturnsNull()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            Username = "nonexistentuser",
            Password = "password123"
        };

        // Act
        var result = await _authService.LoginAsync(loginDto);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task LoginAsync_InvalidPassword_ReturnsNull()
    {
        // Arrange
        var user = new User
        {
            Username = "testuser",
            Email = "test@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("correctpassword")
        };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var loginDto = new LoginDto
        {
            Username = "testuser",
            Password = "wrongpassword"
        };

        // Act
        var result = await _authService.LoginAsync(loginDto);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task RegisterAsync_DuplicateUsername_ThrowsException()
    {
        // Arrange
        var existingUser = new User
        {
            Username = "testuser",
            Email = "existing@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123")
        };
        await _context.Users.AddAsync(existingUser);
        await _context.SaveChangesAsync();

        var registerDto = new RegisterDto
        {
            Username = "testuser", // Same username
            Email = "new@example.com",
            Password = "password123"
        };

        // Act & Assert
        await Assert.ThrowsAsync<DbUpdateException>(() => _authService.RegisterAsync(registerDto));
    }
} 