using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TaskFlow.Application.DTO.Auth;
using TaskFlow.Infrastructure.Persistence;
using TaskFlow.IntegrationTests.Infrastructure;


namespace TaskFlow.IntegrationTests.Auth
{
    public class AuthEndpointsTests : IntegrationTestBase, IClassFixture<CustomWebApplicationFactory>
    {


        public AuthEndpointsTests(CustomWebApplicationFactory factory) : base(factory)
        {

        }

        [Fact]
        public async Task Swagger_Should_Return_Ok()
        {
            //act
            var response = await Client.GetAsync("/swagger/index.html");
            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task Register_Should_Create_User_In_Database()
        {
            // Arrange

            var dto = new RegisterDto
            {
                UserName = "JohnDoe",
                Email = "john@test.com",
                Password = "Password123!"
            };

            //act
            var response = await Client.PostAsJsonAsync(
                "/api/auth/register",
                dto);
            //Assert HTTP

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var authResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>();

            authResponse.Should().NotBeNull();
            authResponse!.AccessToken.Should().NotBeNullOrWhiteSpace();
            authResponse.RefreshToken.Should().NotBeNullOrWhiteSpace();
            //Assert Database
            using var scope = Factory.Services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<TaskFlowDbContext>();

            var user = await db.Users
                .FirstOrDefaultAsync(x => x.Email == dto.Email);

            user.Should().NotBeNull();
            user!.UserName.Should().Be(dto.UserName);
        }

        [Fact]
        public async Task Register_Should_Return_Conflict_When_Email_Already_Exists()
        {
            //Arrange
            var dto = new RegisterDto
            {
                UserName = "John",
                Email = "john@test.com",
                Password = "Password123!"
            };

            await Client.PostAsJsonAsync("/api/auth/register", dto);
            //Act
            var response = await Client.PostAsJsonAsync("/api/auth/register", dto);
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
            
            var error = await response.Content.ReadAsStringAsync();

            error.Should().Contain("Email is already in use");
        }
        [Fact]
        public async Task Login_Should_Return_Unauthorized_When_Password_Is_Invalid()
        {
            //Arrange
            var registerDto = new RegisterDto
            {
                UserName = "JohnDoe",
                Email = "john@test.com",
                Password = "Password123!"
            };
            await Client.PostAsJsonAsync("/api/auth/register", registerDto);

            var loginDto = new LoginDto
            {
                Email = registerDto.Email,
                Password = "WrongPassword123!"
            };
            //Act
            var response = await Client.PostAsJsonAsync("/api/auth/login", loginDto);
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            var error = await response.Content.ReadAsStringAsync();

            error.Should().Contain("Invalid email or password");
        }
        [Fact]
        public async Task Login_Should_Return_Tokens_When_Credentials_Are_Valid()
        {
            //Arrange
            var registerDto = new RegisterDto
            {
                UserName = "JohnDoe",
                Email = "john@test.com",
                Password = "Password123!"
            };
            await Client.PostAsJsonAsync("/api/auth/register", registerDto);

            var loginDto = new LoginDto
            {
                Email = registerDto.Email,
                Password = registerDto.Password
            };

            //Act
            var response = await Client.PostAsJsonAsync("/api/auth/login", loginDto);
            //Assert Http
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var authResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>();

            authResponse.Should().NotBeNull();
            authResponse!.AccessToken.Should().NotBeNullOrWhiteSpace();
            authResponse.RefreshToken.Should().NotBeNullOrWhiteSpace();
            // Assert Database

            var scope = Factory.Services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<TaskFlowDbContext>();

            var refreshToken = await db.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == authResponse.RefreshToken);

            refreshToken.Should().NotBeNull();
            refreshToken!.IsRevoked.Should().BeFalse();



        }
    }
}
