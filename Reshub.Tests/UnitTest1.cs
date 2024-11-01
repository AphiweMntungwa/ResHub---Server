namespace Reshub.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        // Arrange
        var expected = 5;
        var actual = 2 + 3;

        // Act and Assert
        Assert.Equal(expected, actual);
    }

}