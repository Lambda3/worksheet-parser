using Bogus;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using Worksheet.Parser.Tests.Fakers;

namespace Worksheet.Parser.Tests
{
    public class ValidationResultTTests
    {
        private ValidationResult<FakeClass> validationResult;
        private readonly Faker faker;

        public ValidationResultTTests() => faker = new Faker();

        [SetUp]
        public void SetUp() => validationResult = new ValidationResult<FakeClass>();

        [Test]
        public void ShouldInitializeItensOnInstantiate() => validationResult.Itens.Should().BeEmpty();

        [Test]
        public void ShouldAddItem()
        {
            var fakeClass = CreateItem();
            var itens = new List<FakeClass> { fakeClass };
            validationResult.AddItem(fakeClass);
            validationResult.Itens.Should().BeEquivalentTo(itens);
        }

        [Test]
        public void ShouldAddItens()
        {
            var itens = new List<FakeClass> { CreateItem(), CreateItem(), CreateItem() };
            validationResult.AddItem(itens);
            validationResult.Itens.Should().BeEquivalentTo(itens);
        }

        [Test]
        public void ShouldAddItensResultIfIsSuccess()
        {
            var item = CreateItem();
            var validation = new ValidationResult<FakeClass>();
            validation.AddItem(item);

            validationResult.AddResult(validation);
            validationResult.Itens.Should().BeEquivalentTo(new List<FakeClass> { item });
        }

        [Test]
        public void ShouldNotAddItensResultIfIsNotSuccess()
        {
            var validationAdded = new ValidationResult<FakeClass>();
            validationAdded.AddItem(CreateItem());
            validationAdded.AddError(CreateError());

            validationResult.AddResult(validationAdded);
            validationResult.Itens.Should().BeEmpty();
        }

        [Test]
        public void ShouldAddErrorsIfIsNotSuccess()
        {
            var error = CreateError();
            var validationAdded = new ValidationResult<FakeClass>();
            validationAdded.AddError(error);

            validationResult.AddResult(validationAdded);
            validationResult.Errors.Should().BeEquivalentTo(new List<Error> { error });
        }

        private FakeClass CreateItem() => new FakeClass { Id = faker.Random.Int() };
        private Error CreateError() => new Error(faker.Random.Word(), faker.Random.Int(1, 50), faker.Random.Int(1, 50));
    }
}
