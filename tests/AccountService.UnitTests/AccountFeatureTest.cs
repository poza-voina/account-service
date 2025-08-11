using AccountService.Api.Features.Account.CreateAccount;
using AccountService.Api.Features.Account.PatchAccount;
using AccountService.Api.ViewModels;
using AccountService.Infrastructure;
using AccountService.Infrastructure.Enums;
using AccountService.Infrastructure.Models;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Xunit;

namespace AccountService.UnitTests;

public class AccountFeatureTest : TestBase
{
    [Fact]
    public async Task CreateAccount_WhenValidData_Successful()
    {
        var expectedData = new AccountViewModel
        {
            Id = Guid.NewGuid(),
            OwnerId = Guid.Parse("d3b07384-d9a6-4b5e-bc8d-23f7c1a1a111"),
            Type = AccountType.Checking,
            Currency = "USD",
            Balance = 0,
            OpeningDate = DateTime.MinValue,
            ClosingDate = DateTime.Now.AddDays(100),
            Version = 0
        };

        var accountCommand = new CreateAccountCommand
        {
            OwnerId = Guid.Parse("d3b07384-d9a6-4b5e-bc8d-23f7c1a1a111"),
            Type = AccountType.Checking,
            Currency = "USD",
            ClosingDate = DateTime.Now.AddDays(100)
        };

        var mediator = GetService<IMediator>();

        var actualData = await mediator.Send(accountCommand, CancellationToken.None);

        actualData.Should().BeEquivalentTo
            (expectedData, options =>
                options.Excluding(x => x.Id)
                    .Excluding(x => x.OpeningDate)
                    .Excluding(x => x.ClosingDate)
                    .Excluding(x => x.Version));
    }

    [Fact]
    public async Task CreateAccount_WhenInValidData_ThrowValidationException()
    {
        var accountCommand = new CreateAccountCommand
        {
            OwnerId = Guid.Parse("d3b07384-d9a6-4b5e-bc8d-23f7c1a1a111"),
            Type = AccountType.Checking,
            Currency = "data",
            ClosingDate = DateTime.Now.AddDays(100)
        };

        var mediator = GetService<IMediator>();

        await FluentActions.Invoking(() => mediator.Send(accountCommand, CancellationToken.None))
            .Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task PatchAccount_WhenValidData_Success()
    {
        var expectedData = new AccountViewModel
        {
            Id = Guids[1],
            OwnerId = Guid.Parse("a5f5c3b2-1e74-4e6d-9c9d-8bfbec79a222"),
            Type = AccountType.Deposit,
            Currency = "USD",
            Balance = 5000.00m,
            InterestRate = 0,
            OpeningDate = DateTime.MinValue,
            ClosingDate = null,
            Version = 3
        };

        var context = GetService<ApplicationDbContext>();

        await AddDataAsync(context);

        var accountCommand = new PatchAccountCommand
        {
            Id = Guids[1],
            ClosingDate = null,
            InterestRate = 0,
            Version = 3
        };

        var mediator = GetService<IMediator>();

        var actualData = await mediator.Send(accountCommand, CancellationToken.None);

        actualData.Should().BeEquivalentTo(expectedData);
    }
}
