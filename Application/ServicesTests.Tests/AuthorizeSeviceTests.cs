﻿using Services;

namespace ServicesTests.Tests;

public class AuthorizeSeviceTests
{
    private readonly IAutorizeService _authorizeService;

    public AuthorizeSeviceTests(IAutorizeService authorizeService)
    {
        _authorizeService = authorizeService;
    }
    
    [Fact]
    public async Task RegisterAndLoginSuccessTest()
    {
        RegisterModel generated = DataGenerator.GenerateRegisterMode();
        generated.Password = "pa$$w0rd";
        User? registered = await _authorizeService.RegisterUserAsync(generated);

        AuthorizeModel authorizeModel = new()
        {
            UserId = registered!.UserId,
            EmailAddress = registered.EmailAddress,
            Password = "pa$$w0rd"
        };
        bool? authorized = await _authorizeService.AutorizeUserAsync(authorizeModel);

        Assert.True(authorized);
    }
    
    [Fact]
    public async Task RegisterAndLoginFailureTest()
    {
        RegisterModel generated = DataGenerator.GenerateRegisterMode();
        generated.Password = "pa$$w0rd";
        User? registered = await _authorizeService.RegisterUserAsync(generated);

        AuthorizeModel authorizeModel = new()
        {
            UserId = registered!.UserId,
            EmailAddress = registered.EmailAddress,
            Password = "passwird"
        };
        bool? authorized = await _authorizeService.AutorizeUserAsync(authorizeModel);

        Assert.False(authorized);
    }
    
    [Fact]
    public async Task ChangeAccountInfoTest()
    {
        RegisterModel generated = DataGenerator.GenerateRegisterMode();
        generated.Password = "pa$$w0rd";
        User? registered = await _authorizeService.RegisterUserAsync(generated);

        User changedCopy = new()
        {
            UserId = registered!.UserId,
            UserName = registered.UserName,
            EmailAddress = registered.EmailAddress,
            DateOfBirth = registered!.DateOfBirth,
            Phone = "+1 (245) 678 92",
            PasswordHash = "pa$$w0rd"
        };
        User? updated = await _authorizeService.UpdateUserAsync(registered!.UserId, changedCopy);

        Assert.Equal(registered.Phone, updated!.Phone);
    }
    [Fact]
    public async Task DeleteUserTest()
    {
        RegisterModel generated = DataGenerator.GenerateRegisterMode();
        generated.Password = "pa$$w0rd";
        User? registered = await _authorizeService.RegisterUserAsync(generated);

        bool? condition = await _authorizeService.DeleteUserAsync(registered!.UserId);

        Assert.True(condition);
    }
}
