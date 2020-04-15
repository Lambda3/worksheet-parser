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
            var errors = new List<Error> { error };
            new ValidationResult(error).Errors.Should().BeEquivalentTo(errors);
        }

        [Test]
        public void ShouldAddError()
        {
            var error = CreateError();
            var errors = new List<Error> { error };
            validationResult.AddError(error);
            validationResult.Errors.Should().BeEquivalentTo(errors);
        }

        [Test]
        public void ShouldAddErrorList()
        {
            var errors = new List<Error> { CreateError(), CreateError(), CreateError() };
            validationResult.AddError(errors);
            validationResult.Errors.Should().BeEquivalentTo(errors);
        }

        [Test]
        public void ShouldAddValidationResultWithErrors()
        {
            var errors = new List<Error> { CreateError(), CreateError(), CreateError() };
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

        private Error CreateError() => new Error(faker.Random.Word(), faker.Random.Int(1, 50), faker.Random.Int(1, 50));
    }
}
