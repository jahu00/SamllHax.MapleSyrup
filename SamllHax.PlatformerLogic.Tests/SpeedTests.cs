using FluentAssertions;
using Xunit;

namespace SamllHax.PlatformerLogic.Tests
{
    public class Tests
    {
        [Theory]
        [InlineData(0, 1, 1, 10, 1, "Acceleration")]
        [InlineData(1, 1, 1, 10, 2, "Acceleration with start speed")]
        [InlineData(-1, 1, 1, 10, 0, "Acceleration with opposite start speed")]
        [InlineData(0, 0.5, 1, 10, 0.5, "Acceleration with fraction delta")]
        [InlineData(0, 1, 10, 1, 1, "Acceleration overflowing max speed")]
        [InlineData(1, 2, 2, 10, 5, "Acceleration with all options")]
        [InlineData(-10, 1, 1, 0, -9, "Deacceleration to 0")]
        public void AccelerateWithNormalizedMaxSpeed(float startSpeed, float delta, float acceleration, float maxSpeed, float expectedResult, string description)
        {
            // Arrange
            var speed = new Speed();
            speed.Set(startSpeed);
            var oppositeSpeed = new Speed();
            oppositeSpeed.Set(-1 * startSpeed);

            // Act
            speed.Accelerate(delta: delta, acceleration: acceleration, maxSpeed: maxSpeed, normalizeMaxSpeed: true);
            oppositeSpeed.Accelerate(delta: delta, acceleration: acceleration * -1, maxSpeed: maxSpeed, normalizeMaxSpeed: true);

            // Assert
            speed.Value.Should().Be(expectedResult, description);
            oppositeSpeed.Value.Should().Be(expectedResult * -1, "Opposite: " + description);
        }
    }
}