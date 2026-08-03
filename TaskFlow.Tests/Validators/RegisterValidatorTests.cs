using FluentAssertions;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTO.Auth;
using TaskFlow.Application.Validators.Auth;

namespace TaskFlow.Tests.Validators
{
    public class RegisterValidatorTests
    {

        private readonly RegisterValidator _validator = new();

        public static IEnumerable<object[]> InvalidEmails =>
            new List<object[]>
            {
                 new object[] { "abc" },
                 new object[] { "gmail.com" },
                 new object[] { "@gmail.com" },
                 new object[] { "test" },
                 new object[] { "" },
                 new object[] { " " },
                 new object[] { null! }
            };


        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Should_Have_Error_When_UserName_Is_Empty(string userName)
        {
            // Arrange
            var dto = new RegisterDto
            {
                UserName = userName,
                Email = "john@test.com",
                Password = "Password123!"
            };
            //Act
            var result = _validator.TestValidate(dto);
            //Assert
            result.ShouldHaveValidationErrorFor(x => x.UserName);
        }
        [Theory]
        [InlineData("Jonh")]
        [InlineData("Vilinka")]
        [InlineData("User222")]
        public void Should_Not_Have_Error_When_Username_IS_Valid(string userName)
        {
            // Arrange
            var dto = new RegisterDto
            {
                UserName = userName,
                Email = "john@test.com",
                Password = "Password123!"
            };
            //Act
            var result = _validator.TestValidate(dto);
            //Assert
            result.ShouldNotHaveValidationErrorFor(x => x.UserName);
        }
        [Theory]
        [InlineData("A")]
        [InlineData("AA")]
        public void Should_Have_Error_When_Username_Length_Is_Invalid(string userName)
        {
            // Arrange
            var dto = new RegisterDto
            {
                UserName = userName,
                Email = "john@test.com",
                Password = "Password123!"
            };
            //Act
            var result = _validator.TestValidate(dto);
            //Assert
            result.ShouldHaveValidationErrorFor(x => x.UserName);

        }


        [Theory]
        [MemberData(nameof(InvalidEmails))]
        public void Should_Have_Error_When_Email_Is_Invalid(string email)
        {
            Console.WriteLine(email);
            // Arrange
            var dto = new RegisterDto
            {
                UserName = "John",
                Email = email,
                Password = "Password123!"
            };
            //Act
            var result = _validator.TestValidate(dto);
            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }
    }
}