using DentalTrack.Domain.Entities;
using DentalTrack.Domain.ValueObjects;
using FluentAssertions;

namespace DentalTrack.Domain.Tests.Entities;

public class UserTests
{
    [Fact]
    public void Constructor_WithValidParameters_ShouldCreateUser()
    {
        // Arrange
        var email = "test@example.com";
        var firstName = "John";
        var lastName = "Doe";
        var role = UserRole.Doctor;
        var googleId = "google123";

        // Act
        var user = new User(email, firstName, lastName, role, googleId);

        // Assert
        user.Email.Should().Be(email);
        user.FirstName.Should().Be(firstName);
        user.LastName.Should().Be(lastName);
        user.Role.Should().Be(role);
        user.GoogleId.Should().Be(googleId);
        user.IsActive.Should().BeTrue();
        user.LastLoginAt.Should().BeNull();
        user.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Constructor_WithoutGoogleId_ShouldCreateUser()
    {
        // Arrange
        var email = "test@example.com";
        var firstName = "John";
        var lastName = "Doe";
        var role = UserRole.Assistant;

        // Act
        var user = new User(email, firstName, lastName, role);

        // Assert
        user.Email.Should().Be(email);
        user.FirstName.Should().Be(firstName);
        user.LastName.Should().Be(lastName);
        user.Role.Should().Be(role);
        user.GoogleId.Should().BeNull();
        user.IsActive.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Constructor_WithInvalidEmail_ShouldThrowArgumentException(string invalidEmail)
    {
        // Arrange & Act & Assert
        var action = () => new User(invalidEmail, "John", "Doe", UserRole.Doctor);
        action.Should().Throw<ArgumentException>()
            .WithMessage("Email cannot be empty*");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Constructor_WithInvalidFirstName_ShouldThrowArgumentException(string invalidFirstName)
    {
        // Arrange & Act & Assert
        var action = () => new User("test@example.com", invalidFirstName, "Doe", UserRole.Doctor);
        action.Should().Throw<ArgumentException>()
            .WithMessage("First name cannot be empty*");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Constructor_WithInvalidLastName_ShouldThrowArgumentException(string invalidLastName)
    {
        // Arrange & Act & Assert
        var action = () => new User("test@example.com", "John", invalidLastName, UserRole.Doctor);
        action.Should().Throw<ArgumentException>()
            .WithMessage("Last name cannot be empty*");
    }

    [Fact]
    public void UpdateProfile_WithValidParameters_ShouldUpdateProfile()
    {
        // Arrange
        var user = new User("test@example.com", "John", "Doe", UserRole.Doctor);
        var originalUpdatedAt = user.UpdatedAt;
        var newFirstName = "Jane";
        var newLastName = "Smith";

        // Act
        user.UpdateProfile(newFirstName, newLastName);

        // Assert
        user.FirstName.Should().Be(newFirstName);
        user.LastName.Should().Be(newLastName);
        user.UpdatedAt.Should().BeAfter(originalUpdatedAt ?? DateTime.MinValue);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void UpdateProfile_WithInvalidFirstName_ShouldThrowArgumentException(string invalidFirstName)
    {
        // Arrange
        var user = new User("test@example.com", "John", "Doe", UserRole.Doctor);

        // Act & Assert
        var action = () => user.UpdateProfile(invalidFirstName, "Doe");
        action.Should().Throw<ArgumentException>()
            .WithMessage("First name cannot be empty*");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void UpdateProfile_WithInvalidLastName_ShouldThrowArgumentException(string invalidLastName)
    {
        // Arrange
        var user = new User("test@example.com", "John", "Doe", UserRole.Doctor);

        // Act & Assert
        var action = () => user.UpdateProfile("John", invalidLastName);
        action.Should().Throw<ArgumentException>()
            .WithMessage("Last name cannot be empty*");
    }

    [Fact]
    public void UpdateRole_WithNewRole_ShouldUpdateRole()
    {
        // Arrange
        var user = new User("test@example.com", "John", "Doe", UserRole.Assistant);
        var originalUpdatedAt = user.UpdatedAt;
        var newRole = UserRole.Doctor;

        // Act
        user.UpdateRole(newRole);

        // Assert
        user.Role.Should().Be(newRole);
        user.UpdatedAt.Should().BeAfter(originalUpdatedAt ?? DateTime.MinValue);
    }

    [Fact]
    public void RecordLogin_ShouldUpdateLastLoginAt()
    {
        // Arrange
        var user = new User("test@example.com", "John", "Doe", UserRole.Doctor);
        var originalUpdatedAt = user.UpdatedAt;

        // Act
        user.RecordLogin();

        // Assert
        user.LastLoginAt.Should().NotBeNull();
        user.LastLoginAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        user.UpdatedAt.Should().BeAfter(originalUpdatedAt ?? DateTime.MinValue);
    }

    [Fact]
    public void Deactivate_ShouldSetIsActiveToFalse()
    {
        // Arrange
        var user = new User("test@example.com", "John", "Doe", UserRole.Doctor);
        var originalUpdatedAt = user.UpdatedAt;

        // Act
        user.Deactivate();

        // Assert
        user.IsActive.Should().BeFalse();
        user.UpdatedAt.Should().BeAfter(originalUpdatedAt ?? DateTime.MinValue);
    }

    [Fact]
    public void Activate_ShouldSetIsActiveToTrue()
    {
        // Arrange
        var user = new User("test@example.com", "John", "Doe", UserRole.Doctor);
        user.Deactivate(); // First deactivate
        var originalUpdatedAt = user.UpdatedAt;

        // Act
        user.Activate();

        // Assert
        user.IsActive.Should().BeTrue();
        user.UpdatedAt.Should().BeAfter(originalUpdatedAt ?? DateTime.MinValue);
    }

    [Fact]
    public void GetFullName_ShouldReturnFormattedName()
    {
        // Arrange
        var user = new User("test@example.com", "John", "Doe", UserRole.Doctor);

        // Act
        var fullName = user.GetFullName();

        // Assert
        fullName.Should().Be("John Doe");
    }

    [Theory]
    [InlineData(UserRole.Doctor, true, false, false)]
    [InlineData(UserRole.Admin, false, true, false)]
    [InlineData(UserRole.Assistant, false, false, true)]
    public void RoleCheckMethods_ShouldReturnCorrectValues(UserRole role, bool expectedIsDoctor, bool expectedIsAdmin, bool expectedIsAssistant)
    {
        // Arrange
        var user = new User("test@example.com", "John", "Doe", role);

        // Act & Assert
        user.IsDoctor().Should().Be(expectedIsDoctor);
        user.IsAdmin().Should().Be(expectedIsAdmin);
        user.IsAssistant().Should().Be(expectedIsAssistant);
    }

    [Fact]
    public void User_ShouldInheritFromBaseEntity()
    {
        // Arrange & Act
        var user = new User("test@example.com", "John", "Doe", UserRole.Doctor);

        // Assert
        user.Should().BeAssignableTo<BaseEntity>();
        user.Id.Should().NotBeEmpty();
        user.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }
}