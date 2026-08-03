using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;

using TaskFlow.Application.DTO.Auth;
using TaskFlow.Application.Exceptions;
using TaskFlow.Application.Interfaces.Repositories;
using TaskFlow.Application.Interfaces.Services;
using TaskFlow.Application.Services;
using TaskFlow.Domain.Enities;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace TaskFlow.Tests.Auth
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock = new();
        private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
        private readonly Mock<IJwtTokenService> _jwtTokenServiceMock = new();
        private readonly Mock<IRefreshTokenService> _refreshTokenServiceMock = new();
        private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock = new();

        private readonly Mock<IValidator<RegisterDto>> _registerValidatorMock = new();
        private readonly Mock<IValidator<LoginDto>> _loginValidatorMock = new();

        private readonly AuthService _authService;
        public AuthServiceTests()
        {
            _authService = new AuthService(
                _userRepositoryMock.Object,
                _passwordHasherMock.Object,
                _jwtTokenServiceMock.Object,
                _refreshTokenServiceMock.Object,
                _refreshTokenRepositoryMock.Object,
                _registerValidatorMock.Object,
                _loginValidatorMock.Object
            );
        }
        [Fact]
        public async Task Register_Should_Create_User_When_Data_Is_Valid()
        {
            //Arrange

            var dto = new RegisterDto
            {
                UserName = "JohnDoe",
                Email = "john@Test.com",
                Password = "Password123!"
            };

            _registerValidatorMock
                .Setup(x => x.ValidateAsync(dto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _userRepositoryMock
                .Setup(x => x.ExistsByEmailAsync(dto.Email))
                .ReturnsAsync(false);

            _userRepositoryMock
                .Setup(x => x.ExistsByUserNameAsync(dto.UserName))
                .ReturnsAsync(false);

            _userRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<User>()))
                .Callback<User>(user => user.Id = 1) // Simulate setting
                .Returns(Task.CompletedTask);


            _passwordHasherMock
                .Setup(x => x.HashPassword(It.IsAny<string>()))
                .Returns("hashedPassword");

            _jwtTokenServiceMock
                .Setup(x => x.GenerateAccessToken(It.IsAny<User>()))
                .Returns(new JwtTokenResultDto
                {
                    Token = "jwt-token",
                    ExpiresAt = DateTimeOffset.UtcNow.AddHours(1)
                });

            _refreshTokenServiceMock
                .Setup(x => x.GenerateRefreshToken(1))
                .Returns(new RefreshToken
                {
                    Token = "refresh-token",
                });

            //Act
            var result = await _authService.RegisterAsync(dto);
            //Assert

            result.Should().NotBeNull();
            result.AccessToken.Should().Be("jwt-token");
            result.RefreshToken.Should().Be("refresh-token");

            _userRepositoryMock.Verify(
                x => x.AddAsync(It.IsAny<User>()),
                Times.Once);

            _refreshTokenRepositoryMock.Verify(
                x => x.AddAsync(It.Is<RefreshToken>(r =>
                r.Token == "refresh-token")),
                Times.Once);

            _jwtTokenServiceMock.Verify(
                x => x.GenerateAccessToken(It.IsAny<User>()),
                Times.Once);
        }
        [Fact]
        public async Task Register_Should_Throw_txception_When_Email_Already_Exists()
        {
            //Arrange

            var dto = new RegisterDto
            {
                UserName = "JohnDoe",
                Email = "john@test.com",
                Password = "Password123!"
            };

            _registerValidatorMock
                .Setup(x => x.ValidateAsync(
                    It.IsAny<RegisterDto>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _userRepositoryMock
                .Setup(x => x.ExistsByEmailAsync(dto.Email))
                .ReturnsAsync(true);
            //Act
            Func<Task> act = () => _authService.RegisterAsync(dto);
            //Assert
            await act.Should()
                .ThrowAsync<ConflictException>()
                .WithMessage("Email is already in use*");

            _userRepositoryMock.Verify(
                x => x.AddAsync(It.IsAny<User>()),
                Times.Never());

            _refreshTokenRepositoryMock.Verify(
                x => x.AddAsync(It.IsAny<RefreshToken>()),
                Times.Never());
        }
        [Fact]
        public async Task Register_Should_Throw_ConflictException_When_UserName_Already_Exists()
        {
            //Arrange
            var dto = new RegisterDto
            {
                UserName = "JohnDoe",
                Email = "john@test.com",
                Password = "Password123!"
            };

            _registerValidatorMock
                .Setup(x => x.ValidateAsync(
                    It.IsAny<RegisterDto>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _userRepositoryMock
                .Setup(x => x.ExistsByUserNameAsync(dto.UserName))
                .ReturnsAsync(true);

            //Act
            Func<Task> act = () => _authService.RegisterAsync(dto);
            //Assert
            await act.Should()
                .ThrowAsync<ConflictException>()
                .WithMessage("Username is already in use*");

            _userRepositoryMock.Verify(
                x => x.AddAsync(It.IsAny<User>()),
                Times.Never());
        }
        [Fact]
        public async Task Register_Should_Throw_BadRequestException_When_Validation_Fails()
        {
            //Arrange
            var dto = new RegisterDto
            {
                UserName = "",
                Email = "invalid-email",
                Password = "123"
            };

            var validationResult = new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure(nameof(RegisterDto.UserName), "UserName is required"),
                new ValidationFailure(nameof(RegisterDto.Email), "Invalid email"),
                new ValidationFailure(nameof(RegisterDto.Password), "Password is too short")
            });

            _registerValidatorMock
                .Setup(x => x.ValidateAsync(
                    It.IsAny<RegisterDto>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);
            //Act
            Func<Task> act = () => _authService.RegisterAsync(dto);
            //Assert
            await act.Should()
                .ThrowAsync<BadRequestException>();

            _userRepositoryMock.Verify(
                x => x.AddAsync(It.IsAny<User>()),
                Times.Never());
        }
        [Fact]
        public async Task Login_Should_Return_Tokens_When_Credentials_Are_Valid()
        {
            var dto = new LoginDto
            {
                Email = "john@test.com",
                Password = "Password123!"
            };

            var user = new User
            {
                Id = 1,
                UserName = "JohnDoe",
                Email = dto.Email,
                PasswordHash = "hashedPassword"
            };

            var oldRefreshToken = new List<RefreshToken>
            {
                new RefreshToken
                {
                    Token = "old-token",
                    IsRevoked = false,
                }
            };

            _loginValidatorMock
                .Setup(x => x.ValidateAsync(
                    It.IsAny<LoginDto>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _userRepositoryMock
                .Setup(x => x.GetByEmailAsync(dto.Email))
                .ReturnsAsync(user);

            _passwordHasherMock
                .Setup(x =>
                x.VerifyPassword(dto.Password, user.PasswordHash))
                .Returns(true);

            _refreshTokenRepositoryMock
                .Setup(x => x.GetActiveByUserIdAsync(user.Id))
                .ReturnsAsync(oldRefreshToken);

            _refreshTokenRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            _jwtTokenServiceMock
                .Setup(x => x.GenerateAccessToken(user))
                .Returns(new JwtTokenResultDto
                {
                    Token = "jwt-token",
                    ExpiresAt = DateTimeOffset.UtcNow.AddHours(1)
                });

          _refreshTokenServiceMock
             .Setup(x => x.GenerateRefreshToken(user.Id))
                  .Returns(new RefreshToken
                  {
                     Token = "refresh-token"
                  });

            _refreshTokenRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<RefreshToken>()))
                .Returns(Task.CompletedTask);

            //Act
            var result = await _authService.LoginAsync(dto);
            //Assert
            result.Should().NotBeNull();
            result.AccessToken.Should().Be("jwt-token");
            result.RefreshToken.Should().Be("refresh-token");

            oldRefreshToken[0].IsRevoked.Should().BeTrue();

            _refreshTokenRepositoryMock.Verify(
                x => x.SaveChangesAsync(),
                Times.Once);

            _refreshTokenRepositoryMock.Verify(
                x => x.AddAsync(It.Is<RefreshToken>(
                    r => r.Token == "refresh-token")),
                Times.Once);
        }
        [Fact]
        public async Task Login_Should_Throw_UnauthorizedException_When_Email_Does_Not_Exist()
        {
            var dto = new LoginDto
            {
                Email = "john@test.com",
                Password = "Password123!"
            };

            _loginValidatorMock
                .Setup(x => x.ValidateAsync(
                    It.IsAny<LoginDto>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _userRepositoryMock
                .Setup(x => x.GetByEmailAsync(dto.Email))
                .ReturnsAsync((User?)null);
            //Act
            Func<Task> act = () => _authService.LoginAsync(dto);
            //Assert
            await act.Should()
                .ThrowAsync<UnauthorizedException>()
                .WithMessage("Invalid email or password");

            _passwordHasherMock.Verify(
                x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()),
                Times.Never);

            _jwtTokenServiceMock.Verify(
                x => x.GenerateAccessToken(It.IsAny<User>()),
                Times.Never);
        }
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Refresh_Should_Throw_UnauthorizedException_When_Token_Is_Invalid(bool tokenExists)
        {
            //Arrange
            RefreshToken? token = null;

            if(tokenExists)
            {
                 token = new RefreshToken
                {
                    Token = "refresh-token",
                    IsRevoked = true
                };
            }
            
            _refreshTokenRepositoryMock
                .Setup( x => x.GetByTokenAsync(It.IsAny<string>()))
                .ReturnsAsync(token);

            var dto = new RefreshTokenRequestDto
            {
                RefreshToken = "refresh-token"
            };
            //Act
            Func<Task> act = () => _authService.RefreshAsync(dto);
            //Assert
            await act.Should()
                .ThrowAsync<UnauthorizedException>()
                .WithMessage("*Invalid refresh token");
        }
    }
}
