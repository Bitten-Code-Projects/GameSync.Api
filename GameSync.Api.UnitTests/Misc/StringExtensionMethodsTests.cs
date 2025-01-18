using GameSync.Api.Utilities;

namespace GameSync.Api.UnitTests.Misc;

public class StringExtensionMethodsTests
{
    [Fact]
    public void LogsSanitize_NullString_ReturnsNull()
    {
        // Arrange
        string input = null!;

        // Act
        var result = input!.LogsSanitize();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void LogsSanitize_EmptyString_ReturnsEmptyString()
    {
        // Arrange
        var input = string.Empty;

        // Act
        var result = input.LogsSanitize();

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void LogsSanitize_StringWithSensitiveData_MasksData()
    {
        // Arrange
        var input = "{\"password\":\"secret123\"}";

        // Act
        var result = input.LogsSanitize();

        // Assert
        Assert.Equal("{\"password\":\"***\"}", result);
    }

    [Fact]
    public void LogsSanitize_StringWithMultipleSensitiveFields_MasksAllFields()
    {
        // Arrange
        var input = "{\"password\":\"secret123\",\"newPassword\":\"newsecret123\"}";

        // Act
        var result = input.LogsSanitize();

        // Assert
        Assert.Equal("{\"password\":\"***\",\"newPassword\":\"***\"}", result);
    }

    [Fact]
    public void LogsSanitize_StringWithSensitiveAndInsensitiveFields_MasksSensitiveFields()
    {
        // Arrange
        var input = "{\"password\":\"secret123\",\"username\":\"john\"}";

        // Act
        var result = input.LogsSanitize();

        // Assert
        Assert.Equal("{\"password\":\"***\",\"username\":\"john\"}", result);
    }

    [Fact]
    public void LogsSanitize_StringWithWhitespace_RemovesWhitespace()
    {
        // Arrange
        var input = "{\n\t\"password\" : \"secret123\"\r}";

        // Act
        var result = input.LogsSanitize();

        // Assert
        Assert.Equal("{\"password\":\"***\"}", result);
    }

    [Fact]
    public void LogsSanitize_StringWithoutSensitiveData_ReturnsUnchanged()
    {
        // Arrange
        var input = "{\"username\":\"john\"}";

        // Act
        var result = input.LogsSanitize();

        // Assert
        Assert.Equal("{\"username\":\"john\"}", result);
    }

    [Fact]
    public void LogsSanitize_StringWithSpacesInFieldValue_MasksCorrectly()
    {
        // Arrange
        var input = "{\"password\":\"my secret password 123\"}";

        // Act
        var result = input.LogsSanitize();

        // Assert
        Assert.Equal("{\"password\":\"***\"}", result);
    }

    [Fact]
    public void LogsSanitize_StringWithSpecialCharacters_HandlesCorrectly()
    {
        // Arrange
        var input = "{\"password\":\"!@#$%^&*()\"}";

        // Act
        var result = input.LogsSanitize();

        // Assert
        Assert.Equal("{\"password\":\"***\"}", result);
    }

    [Theory]
    [InlineData("{\"password\":\"secret\"}", "{\"password\":\"***\"}")]
    [InlineData("{\"password\" : \"secret\"}", "{\"password\":\"***\"}")]
    [InlineData("{\"PASSWORD\":\"secret\"}", "{\"PASSWORD\":\"secret\"}")]  // Case sensitive
    [InlineData("{\"pass word\":\"secret\"}", "{\"pass word\":\"secret\"}")]  // Space in field name
    public void LogsSanitize_VariousFormats_HandlesCorrectly(string input, string expected)
    {
        // Act
        var result = input.LogsSanitize();

        // Assert
        Assert.Equal(expected, result);
    }
}
