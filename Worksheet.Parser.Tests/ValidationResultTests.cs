using Bogus;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace Worksheet.Parser.Tests
{
    public class ValidationResultTests
    {
        private ValidationResult validationResult;
        private readonly Faker faker;

        public ValidationResultTests() => faker = new Faker();

        [SetUp]
        public void SetUp() => validationResult = new ValidationResult();

        [Test]
        public void ShouldInitializeErrorsOnInstantiate() => validationResult.Errors.Should().BeEmpty();

        [Test]
        public void ShouldInitializeWithErrorMessageOnInstantiate()
        {
            var error = CreateError();
            var errors = new List<string> { error };
            new ValidationResult(error).Errors.Should().BeEquivalentTo(errors);
        }

        [Test]
        public void ShouldAddError()
        {
            var error = CreateError();
            var errors = new List<string> { error };
            validationResult.AddError(error);
            validationResult.Errors.Should().BeEquivalentTo(errors);
        }

        [Test]
        public void ShouldAddErrorList()
        {
            var errors = new List<string> { CreateError(), CreateError(), CreateError() };
            validationResult.AddError(errors);
            validationResult.Errors.Should().BeEquivalentTo(errors);
        }

        [Test]
        public void ShouldAddValidationResultWithErrors()
        {
            var errors = new List<string> { CreateError(), CreateError(), CreateError() };
            var validation = new ValidationResult();
            validation.AddError(errors);

            validationResult.AddResult(validation);
            validationResult.Errors.Should().BeEquivalentTo(errors);
        }

        [Test]
        public void ShouldAddValidationResultWithoutErrors()
        {
            validationResult.AddResult(new ValidationResult());
            validationResult.Errors.Should().BeEmpty();
        }

        [Test]
        public void ShouldBeSuccessTrueIfHasNoErrors() => validationResult.IsSuccess.Should().BeTrue();

        public void ShouldBeSuccessFalseIfHasErrors() => new ValidationResult(CreateError()).IsSuccess.Should().BeFalse();

        private string CreateError() => faker.Random.Word();
    }
}
