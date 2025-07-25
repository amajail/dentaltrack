using DentalTrack.Domain.Entities;
using DentalTrack.Domain.ValueObjects;
using FluentAssertions;

namespace DentalTrack.Domain.Tests.Entities;

public class TreatmentTests
{
    private readonly Guid _validPatientId = Guid.NewGuid();
    private readonly TreatmentType _validType = TreatmentType.Filling;
    private readonly string _validTitle = "Dental Filling";

    [Fact]
    public void Constructor_WithValidData_ShouldCreateTreatment()
    {
        // Arrange
        var startDate = DateTime.UtcNow;

        // Act
        var treatment = new Treatment(
            _validPatientId,
            _validType,
            _validTitle,
            "Description",
            150.00m,
            startDate);

        // Assert
        treatment.PatientId.Should().Be(_validPatientId);
        treatment.Type.Should().Be(_validType);
        treatment.Title.Should().Be(_validTitle);
        treatment.Description.Should().Be("Description");
        treatment.EstimatedCost.Should().Be(150.00m);
        treatment.StartDate.Should().Be(startDate);
        treatment.Status.Should().Be(TreatmentStatus.Planned);
        treatment.EndDate.Should().BeNull();
        treatment.ActualCost.Should().BeNull();
        treatment.Notes.Should().BeNull();
        treatment.Id.Should().NotBeEmpty();
        treatment.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Constructor_WithEmptyPatientId_ShouldThrowException()
    {
        // Act & Assert
        var action = () => new Treatment(Guid.Empty, _validType, _validTitle);
        action.Should().Throw<ArgumentException>()
            .WithMessage("Patient ID cannot be empty*");
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void Constructor_WithInvalidTitle_ShouldThrowException(string title)
    {
        // Act & Assert
        var action = () => new Treatment(_validPatientId, _validType, title);
        action.Should().Throw<ArgumentException>()
            .WithMessage("Title cannot be empty*");
    }

    [Fact]
    public void Constructor_WithoutStartDate_ShouldSetCurrentTime()
    {
        // Act
        var treatment = new Treatment(_validPatientId, _validType, _validTitle);

        // Assert
        treatment.StartDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Start_FromPlannedStatus_ShouldUpdateStatusAndStartDate()
    {
        // Arrange
        var treatment = new Treatment(_validPatientId, _validType, _validTitle);
        var originalStartDate = treatment.StartDate;

        // Act
        treatment.Start();

        // Assert
        treatment.Status.Should().Be(TreatmentStatus.InProgress);
        treatment.StartDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        treatment.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Theory]
    [InlineData(TreatmentStatus.InProgress)]
    [InlineData(TreatmentStatus.Completed)]
    [InlineData(TreatmentStatus.Cancelled)]
    public void Start_FromNonPlannedStatus_ShouldThrowException(TreatmentStatus currentStatus)
    {
        // Arrange
        var treatment = new Treatment(_validPatientId, _validType, _validTitle);
        if (currentStatus == TreatmentStatus.InProgress)
            treatment.Start();
        else if (currentStatus == TreatmentStatus.Completed)
        {
            treatment.Start();
            treatment.Complete();
        }
        else if (currentStatus == TreatmentStatus.Cancelled)
            treatment.Cancel();

        // Act & Assert
        var action = () => treatment.Start();
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("Treatment can only be started from Planned status");
    }

    [Fact]
    public void Complete_FromInProgressStatus_ShouldUpdateStatusAndEndDate()
    {
        // Arrange
        var treatment = new Treatment(_validPatientId, _validType, _validTitle);
        treatment.Start();

        // Act
        treatment.Complete(200.00m, "Treatment completed successfully");

        // Assert
        treatment.Status.Should().Be(TreatmentStatus.Completed);
        treatment.EndDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        treatment.ActualCost.Should().Be(200.00m);
        treatment.Notes.Should().Be("Treatment completed successfully");
        treatment.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Theory]
    [InlineData(TreatmentStatus.Planned)]
    [InlineData(TreatmentStatus.Completed)]
    [InlineData(TreatmentStatus.Cancelled)]
    public void Complete_FromNonInProgressStatus_ShouldThrowException(TreatmentStatus currentStatus)
    {
        // Arrange
        var treatment = new Treatment(_validPatientId, _validType, _validTitle);
        if (currentStatus == TreatmentStatus.Completed)
        {
            treatment.Start();
            treatment.Complete();
        }
        else if (currentStatus == TreatmentStatus.Cancelled)
            treatment.Cancel();

        // Act & Assert
        var action = () => treatment.Complete();
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("Treatment can only be completed from InProgress status");
    }

    [Theory]
    [InlineData(TreatmentStatus.Planned)]
    [InlineData(TreatmentStatus.InProgress)]
    public void Cancel_FromNonCompletedStatus_ShouldUpdateStatus(TreatmentStatus currentStatus)
    {
        // Arrange
        var treatment = new Treatment(_validPatientId, _validType, _validTitle);
        if (currentStatus == TreatmentStatus.InProgress)
            treatment.Start();

        // Act
        treatment.Cancel("Patient cancelled");

        // Assert
        treatment.Status.Should().Be(TreatmentStatus.Cancelled);
        treatment.Notes.Should().Be("Patient cancelled");
        treatment.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Cancel_FromCompletedStatus_ShouldThrowException()
    {
        // Arrange
        var treatment = new Treatment(_validPatientId, _validType, _validTitle);
        treatment.Start();
        treatment.Complete();

        // Act & Assert
        var action = () => treatment.Cancel();
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot cancel a completed treatment");
    }

    [Fact]
    public void UpdateDetails_WithValidData_ShouldUpdateFields()
    {
        // Arrange
        var treatment = new Treatment(_validPatientId, _validType, _validTitle);

        // Act
        treatment.UpdateDetails("Updated Title", "Updated Description", 300.00m);

        // Assert
        treatment.Title.Should().Be("Updated Title");
        treatment.Description.Should().Be("Updated Description");
        treatment.EstimatedCost.Should().Be(300.00m);
        treatment.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void UpdateDetails_WithInvalidTitle_ShouldThrowException(string title)
    {
        // Arrange
        var treatment = new Treatment(_validPatientId, _validType, _validTitle);

        // Act & Assert
        var action = () => treatment.UpdateDetails(title, "Description", 100m);
        action.Should().Throw<ArgumentException>()
            .WithMessage("Title cannot be empty*");
    }

    [Fact]
    public void GetDuration_WhenCompleted_ShouldReturnTimeDifference()
    {
        // Arrange
        var treatment = new Treatment(_validPatientId, _validType, _validTitle);
        treatment.Start();
        Thread.Sleep(10); // Small delay to ensure measurable duration
        treatment.Complete();

        // Act
        var duration = treatment.GetDuration();

        // Assert
        duration.Should().NotBeNull();
        duration.Value.Should().BePositive();
    }

    [Fact]
    public void GetDuration_WhenInProgress_ShouldReturnCurrentDuration()
    {
        // Arrange
        var treatment = new Treatment(_validPatientId, _validType, _validTitle);
        treatment.Start();

        // Act
        var duration = treatment.GetDuration();

        // Assert
        duration.Should().NotBeNull();
        duration.Value.Should().BePositive();
    }

    [Fact]
    public void GetDuration_WhenNotStarted_ShouldReturnNull()
    {
        // Arrange
        var treatment = new Treatment(_validPatientId, _validType, _validTitle);

        // Act
        var duration = treatment.GetDuration();

        // Assert
        duration.Should().BeNull();
    }

    [Theory]
    [InlineData(TreatmentStatus.InProgress, true)]
    [InlineData(TreatmentStatus.Planned, false)]
    [InlineData(TreatmentStatus.Completed, false)]
    [InlineData(TreatmentStatus.Cancelled, false)]
    public void IsActive_ShouldReturnCorrectValue(TreatmentStatus status, bool expected)
    {
        // Arrange
        var treatment = new Treatment(_validPatientId, _validType, _validTitle);
        if (status == TreatmentStatus.InProgress)
            treatment.Start();
        else if (status == TreatmentStatus.Completed)
        {
            treatment.Start();
            treatment.Complete();
        }
        else if (status == TreatmentStatus.Cancelled)
            treatment.Cancel();

        // Act & Assert
        treatment.IsActive().Should().Be(expected);
    }

    [Theory]
    [InlineData(TreatmentStatus.Completed, true)]
    [InlineData(TreatmentStatus.Planned, false)]
    [InlineData(TreatmentStatus.InProgress, false)]
    [InlineData(TreatmentStatus.Cancelled, false)]
    public void IsCompleted_ShouldReturnCorrectValue(TreatmentStatus status, bool expected)
    {
        // Arrange
        var treatment = new Treatment(_validPatientId, _validType, _validTitle);
        if (status == TreatmentStatus.InProgress)
            treatment.Start();
        else if (status == TreatmentStatus.Completed)
        {
            treatment.Start();
            treatment.Complete();
        }
        else if (status == TreatmentStatus.Cancelled)
            treatment.Cancel();

        // Act & Assert
        treatment.IsCompleted().Should().Be(expected);
    }

    [Fact]
    public void Collections_ShouldInitializeAsEmpty()
    {
        // Arrange & Act
        var treatment = new Treatment(_validPatientId, _validType, _validTitle);

        // Assert
        treatment.Photos.Should().NotBeNull().And.BeEmpty();
        treatment.Analyses.Should().NotBeNull().And.BeEmpty();
    }
}