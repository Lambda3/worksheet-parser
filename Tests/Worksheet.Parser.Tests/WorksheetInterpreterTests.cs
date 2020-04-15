using Bogus;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using Worksheet.Parser.Tests.Fakers;

namespace Worksheet.Parser.Tests
{
    public class WorksheetInterpreterTests
    {
        private readonly Faker faker = new Faker();

        [Test]
        public void ShouldParseItens()
        {
            var itens = new List<FakeClass>
            {
                FakeClass.CreateItem(),
                FakeClass.CreateItem(true),
                FakeClass.CreateItem(),
                FakeClass.CreateItem(true, true),
                FakeClass.CreateItem(),
                FakeClass.CreateItem(false, true),
            };

            var worksheet = new MyWorksheet(itens);
            var parser = new WorksheetInterpreter<FakeClass>(new ValueSetter(new Converter()), new MyWorksheetMap(), new MessageErrors());
            var rowsFake = parser.Parse(worksheet);

            itens.ForEach(i => i.Bonus = i.Bonus * 0.1M);
            var validationResult = new ValidationResult<FakeClass>();
            validationResult.AddItem(itens);

            rowsFake.Should().BeEquivalentTo(validationResult);
        }
    }
}
