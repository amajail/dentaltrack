using DentalTrack.Domain.Entities;
using DentalTrack.Domain.ValueObjects;
using DentalTrack.Infrastructure.Repositories;
using FluentAssertions;

namespace DentalTrack.Infrastructure.Tests.Repositories;

public class UserRepositoryTests : TestBase
{
    private readonly UserRepository _repository;

    public UserRepositoryTests()
    {
        _repository = new UserRepository(Context);
    }

    [Fact]
    public async Task GetAllAsync_WithNoUsers_ShouldReturnEmptyList()
    {
        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllAsync_WithUsers_ShouldReturnAllUsers()
    {
        // Arrange
        var user1 = new User("john@example.com", "John", "Doe", UserRole.Doctor);
        var user2 = new User("jane@example.com", "Jane", "Smith", UserRole.Assistant);

        await Context.Users.AddRangeAsync(user1, user2);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(u => u.Email == "john@example.com");
        result.Should().Contain(u => u.Email == "jane@example.com");
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingUser_ShouldReturnUser()
    {
        // Arrange
        var user = new User("john@example.com", "John", "Doe", UserRole.Doctor);
        await Context.Users.AddAsync(user);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(user.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Email.Should().Be("john@example.com");
        result.FirstName.Should().Be("John");
        result.LastName.Should().Be("Doe");
        result.Role.Should().Be(UserRole.Doctor);
    }

    [Fact]
    public async Task GetByIdAsync_WithNonExistentUser_ShouldReturnNull()
    {
        // Act
        var result = await _repository.GetByIdAsync(Guid.NewGuid());

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByEmailAsync_WithExistingEmail_ShouldReturnUser()
    {
        // Arrange
        var user = new User("john@example.com", "John", "Doe", UserRole.Doctor);
        await Context.Users.AddAsync(user);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByEmailAsync("john@example.com");

        // Assert
        result.Should().NotBeNull();
        result!.FirstName.Should().Be("John");
        result.LastName.Should().Be("Doe");
        result.Role.Should().Be(UserRole.Doctor);
    }

    [Fact]
    public async Task GetByEmailAsync_WithNonExistentEmail_ShouldReturnNull()
    {
        // Act
        var result = await _repository.GetByEmailAsync("nonexistent@example.com");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByGoogleIdAsync_WithExistingGoogleId_ShouldReturnUser()
    {
        // Arrange
        var user = new User("john@example.com", "John", "Doe", UserRole.Doctor, "google123");
        await Context.Users.AddAsync(user);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByGoogleIdAsync("google123");

        // Assert
        result.Should().NotBeNull();
        result!.GoogleId.Should().Be("google123");
        result.Email.Should().Be("john@example.com");
    }

    [Fact]
    public async Task GetByGoogleIdAsync_WithNonExistentGoogleId_ShouldReturnNull()
    {
        // Act
        var result = await _repository.GetByGoogleIdAsync("nonexistent");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByRoleAsync_WithExistingRole_ShouldReturnUsersWithRole()
    {
        // Arrange
        var doctor1 = new User("doctor1@example.com", "John", "Doe", UserRole.Doctor);
        var doctor2 = new User("doctor2@example.com", "Jane", "Smith", UserRole.Doctor);
        var assistant = new User("assistant@example.com", "Bob", "Johnson", UserRole.Assistant);
        var admin = new User("admin@example.com", "Alice", "Brown", UserRole.Admin);

        await Context.Users.AddRangeAsync(doctor1, doctor2, assistant, admin);
        await Context.SaveChangesAsync();

        // Act
        var doctors = await _repository.GetUsersByRoleAsync(UserRole.Doctor.ToString());

        // Assert
        doctors.Should().HaveCount(2);
        doctors.Should().AllSatisfy(u => u.Role.Should().Be(UserRole.Doctor));
        doctors.Should().Contain(u => u.Email == "doctor1@example.com");
        doctors.Should().Contain(u => u.Email == "doctor2@example.com");
    }

    [Fact]
    public async Task GetByRoleAsync_WithNoUsersOfRole_ShouldReturnEmptyList()
    {
        // Arrange
        var assistant = new User("assistant@example.com", "Bob", "Johnson", UserRole.Assistant);
        await Context.Users.AddAsync(assistant);
        await Context.SaveChangesAsync();

        // Act
        var doctors = await _repository.GetUsersByRoleAsync(UserRole.Doctor.ToString());

        // Assert
        doctors.Should().BeEmpty();
    }

    [Fact]
    public async Task GetActiveUsersAsync_WithActiveAndInactiveUsers_ShouldReturnOnlyActiveUsers()
    {
        // Arrange
        var activeUser1 = new User("active1@example.com", "John", "Doe", UserRole.Doctor);
        var activeUser2 = new User("active2@example.com", "Jane", "Smith", UserRole.Assistant);
        var inactiveUser = new User("inactive@example.com", "Bob", "Johnson", UserRole.Admin);

        inactiveUser.Deactivate();

        await Context.Users.AddRangeAsync(activeUser1, activeUser2, inactiveUser);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetActiveUsersAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().AllSatisfy(u => u.IsActive.Should().BeTrue());
        result.Should().Contain(u => u.Email == "active1@example.com");
        result.Should().Contain(u => u.Email == "active2@example.com");
    }

    [Fact]
    public async Task GetActiveUsersAsync_WithNoActiveUsers_ShouldReturnEmptyList()
    {
        // Arrange
        var inactiveUser = new User("inactive@example.com", "Bob", "Johnson", UserRole.Admin);
        inactiveUser.Deactivate();
        await Context.Users.AddAsync(inactiveUser);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetActiveUsersAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task AddAsync_WithValidUser_ShouldAddUser()
    {
        // Arrange
        var user = new User("john@example.com", "John", "Doe", UserRole.Doctor);

        // Act
        await _repository.AddAsync(user);
        await Context.SaveChangesAsync();

        // Assert
        var addedUser = await Context.Users.FindAsync(user.Id);
        addedUser.Should().NotBeNull();
        addedUser!.Email.Should().Be("john@example.com");
    }

    [Fact]
    public async Task Update_WithValidUser_ShouldUpdateUser()
    {
        // Arrange
        var user = new User("john@example.com", "John", "Doe", UserRole.Doctor);
        await Context.Users.AddAsync(user);
        await Context.SaveChangesAsync();

        // Act
        user.UpdateProfile("Jane", "Smith");
        user.UpdateRole(UserRole.Admin);
        await _repository.UpdateAsync(user);
        await Context.SaveChangesAsync();

        // Assert
        var updatedUser = await Context.Users.FindAsync(user.Id);
        updatedUser.Should().NotBeNull();
        updatedUser!.FirstName.Should().Be("Jane");
        updatedUser.LastName.Should().Be("Smith");
        updatedUser.Role.Should().Be(UserRole.Admin);
    }

    [Fact]
    public async Task Delete_WithValidUser_ShouldRemoveUser()
    {
        // Arrange
        var user = new User("john@example.com", "John", "Doe", UserRole.Doctor);
        await Context.Users.AddAsync(user);
        await Context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(user);
        await Context.SaveChangesAsync();

        // Assert
        var deletedUser = await Context.Users.FindAsync(user.Id);
        deletedUser.Should().BeNull();
    }
}