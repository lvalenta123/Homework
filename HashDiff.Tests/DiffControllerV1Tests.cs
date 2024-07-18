using FluentAssertions;
using HashDiff.Controllers;
using HashDiff.Models;
using HashDiff.Repositories;
using HashDiff.Services;
using HashDiff.Tests.TestDoubles;
using Microsoft.AspNetCore.Mvc;

namespace HashDiff.Tests;

/// <summary>
/// Simple tests that were used as a guideline for development
/// </summary>
public class DiffControllerV1Tests : IDisposable
{
    private static DIContainer diContainer = new();

    [Theory]
    [InlineData("eyJpbnB1dCI6InRlc3RWYWx1ZSJ9", true)]
    [InlineData("@@@", false)]
    public void DiffLeft_AcceptsOnlyValidBase64(string base64string, bool shouldPass)
    {
        var controller = CreateController();
        var result = controller.PostLeftAsync(1, base64string).GetAwaiter().GetResult();

        if (shouldPass)
            result.Should().BeOfType<CreatedAtActionResult>();
        else
            result.Should().BeOfType<BadRequestObjectResult>();
    }
    
    [Theory]
    [InlineData("eyJpbnB1dCI6InRlc3RWYWx1ZSJ9", "testValue")]
    public void DiffLeft_ReturnsCreatedEntity(string base64string, string expectedValue)
    {
        var controller = CreateController();
        var result = controller.PostLeftAsync(1, base64string).GetAwaiter().GetResult();
        
        result.Should().BeOfType<CreatedAtActionResult>();
        result.As<CreatedAtActionResult>().Value.As<Diff>().LeftPart.Should().Be(expectedValue);;
    }
    
    [Theory]
    [InlineData("eyJpbnB1dCI6InRlc3RWYWx1ZSJ9")]
    public void DiffLeft_SavingTwiceReturnsError(string base64string)
    {
        var repository = (DiffTestRepository)diContainer.GetService<IDiffRepository>();
        var controller = CreateController();
        var response1 = controller.PostLeftAsync(1, base64string).GetAwaiter().GetResult();
        var response2 = controller.PostLeftAsync(1, base64string).GetAwaiter().GetResult();
        
        response1.Should().BeOfType<CreatedAtActionResult>();
        response2.Should().BeOfType<BadRequestResult>();
    }
    
    [Theory]
    [InlineData("eyJpbnB1dCI6InRlc3RWYWx1ZSJ9", true)]
    [InlineData("@@@", false)]
    public void DiffRight_AcceptsOnlyValidBase64(string base64string, bool shouldPass)
    {
        var controller = CreateController();
        var result = controller.PostRightAsync(1, base64string).GetAwaiter().GetResult();

        if (shouldPass)
            result.Should().BeOfType<CreatedAtActionResult>();
        else
            result.Should().BeOfType<BadRequestObjectResult>();
    }
    
    [Theory]
    [InlineData("eyJpbnB1dCI6InRlc3RWYWx1ZSJ9", "testValue")]
    public void DiffRight_ReturnsCreatedEntity(string base64string, string expectedValue)
    {
        var controller = CreateController();
        var result = controller.PostRightAsync(1, base64string).GetAwaiter().GetResult();

        result.Should().BeOfType<CreatedAtActionResult>();
        result.As<CreatedAtActionResult>().Value.As<Diff>().RightPart.Should().Be(expectedValue);
    }
    
    [Theory]
    [InlineData("eyJpbnB1dCI6InRlc3RWYWx1ZSJ9")]
    public void DiffRight_SavingTwiceReturnsError(string base64string)
    {
        var controller = CreateController();
        var response1 = controller.PostRightAsync(1, base64string).GetAwaiter().GetResult(); //TODO await!
        var response2 = controller.PostRightAsync(1, base64string).GetAwaiter().GetResult();
        
        response1.Should().BeOfType<CreatedAtActionResult>();
        response2.Should().BeOfType<BadRequestResult>();
    }
    
    [Theory]
    [InlineData("eyJpbnB1dCI6InRlc3RWYWx1ZSJ9", "eyJpbnB1dCI6InRlc3RWYWx1ZSJ9")]
    public async Task GetDiff_SameStrings_ReturnsOk(string leftPart, string rightPart)
    {
        var controller = CreateController();
        _ = await controller.PostLeftAsync(1, leftPart);
        _ = await controller.PostRightAsync(1, rightPart);

        var result = await controller.GetDiffAsync(1);

        result.Should().BeOfType<OkObjectResult>();
        result.As<OkObjectResult>().Value.As<string>().Should().Be("Inputs were equal");
    }

    [Theory]
    [InlineData("eyJpbnB1dCI6Ik1lw59zdGV0dGVuIn0=", "eyJpbnB1dCI6Ik1lc3NzdGV0dGVuIn0=")]
    public async Task GetDiff_DifferentLengths_ReturnsOk(string leftPart, string rightPart)
    {
        var controller = CreateController();
        _ = await controller.PostLeftAsync(1, leftPart);
        _ = await controller.PostRightAsync(1, rightPart);
        
        var result = await controller.GetDiffAsync(1);
        
        result.Should().BeOfType<OkObjectResult>();
        result.As<OkObjectResult>().Value.As<string>().Should().Be("Inputs are of different size");
    }
    
    
    [Theory]
    [InlineData("eyJpbnB1dCI6InRlc3RJbnB1dDEifQ==", "eyJpbnB1dCI6InRlc3RJbnB1dDIifQ==", new[] {9})]
    public async Task GetDiff_SameLenghtsDifferentContents_ReturnsDiffs(string leftPart, string rightPart, int[] diffIndexes)
    {
        var controller = CreateController();
        _ = await controller.PostLeftAsync(1, leftPart);
        _ = await controller.PostRightAsync(1, rightPart);

        var result = await controller.GetDiffAsync(1);

        result.Should().BeOfType<OkObjectResult>();
        var diffResults = result.As<OkObjectResult>().Value.As<DiffIndexes[]>(); 
        diffResults.Should().HaveCount(diffIndexes.Length);
        diffResults.Select(e => e.Index).Should().BeEquivalentTo(diffIndexes);

    }
    
    private static DiffContollerV1 CreateController()
    {
        return new DiffContollerV1(diContainer.GetService<IDiffService>());
    }

    public void Dispose()
    {
        var repository = (DiffTestRepository)diContainer.GetService<IDiffRepository>();
        repository.Clear();
    }
}