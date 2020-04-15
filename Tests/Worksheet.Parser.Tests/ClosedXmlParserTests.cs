using ClosedXML.Excel;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Worksheet.Parser.AspNetCore;
using Worksheet.Parser.ClosedXML;
using Worksheet.Parser.Tests.Fakers;

namespace Worksheet.Parser.Tests
{
    public partial class ClosedXmlParserTests
    {
        private const string worksheetName = "worksheet1";

        [Test]
        public void ShouldParseSheetUsingClosedXML()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddWorksheetParser<FakeClass>(x => x.WithParser<ClosedXmlParser<FakeClass>>().WithMap<MyWorksheetMap>());

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var expectedItens = new List<FakeClass>();
            for (var i = 0; i < 5; i++)
                expectedItens.Add(FakeClass.CreateItem(i % 2 == 0, i % 5 == 0));

            var parser = serviceProvider.GetService<WorksheetParser<FakeClass>>();

            var headers = new MyWorksheetMap().GetFields().Select(s => s.Name).ToList();
            using var worksheet = CreateStream(expectedItens, headers);
            var itens = parser.Parse(worksheet, worksheetName);

            expectedItens.ForEach(x => x.Bonus = x.Bonus * 0.1M);
            var validationResult = new ValidationResult<FakeClass>();
            validationResult.AddItem(expectedItens);
            itens.Should().BeEquivalentTo(validationResult);
        }

        [Test]
        public void ShouldReturnErrorsUsingClosedXML()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddWorksheetParser<FakeClass>(x => x.WithParser<ClosedXmlParser<FakeClass>>().WithMap<MyWorksheetMap>());

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var expectedItens = new List<FakeClass>
            {
                FakeClass.CreateItem(),
                FakeClass.CreateItem(false, false, false),
                FakeClass.CreateItem(false, false, true)
            };

            var parser = serviceProvider.GetService<WorksheetParser<FakeClass>>();

            expectedItens.ForEach(x => x.Bonus *= 0.1M);
            var validationResult = new ValidationResult<FakeClass>();
            var error = new Error("Field TX_NAME can´t not be null", 3, 2);
            validationResult.AddError(error);
            validationResult.AddItem(expectedItens.First());
            validationResult.AddItem(expectedItens.Last());

            var headers = new MyWorksheetMap().GetFields().Select(s => s.Name).ToList();
            using var worksheet = CreateStream(expectedItens, headers);
            var itens = parser.Parse(worksheet, worksheetName);
            itens.Should().BeEquivalentTo(validationResult);
        }

        [Test]
        public void ShouldReturnErrorsUsingClosedXMLAndCustomMessages()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddWorksheetParser<FakeClass>(x => x.WithParser<ClosedXmlParser<FakeClass>>().WithMap<MyWorksheetMap>().WithMessages<CustomMessages>());

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var expectedItens = new List<FakeClass>
            {
                FakeClass.CreateItem(),
                FakeClass.CreateItem(false, false, false),
                FakeClass.CreateItem(false, false, true)
            };

            var parser = serviceProvider.GetService<WorksheetParser<FakeClass>>();

            expectedItens.ForEach(x => x.Bonus *= 0.1M);
            var validationResult = new ValidationResult<FakeClass>();
            var error = new Error("Can't not be null field TX_NAME", 3, 2);
            validationResult.AddError(error);
            validationResult.AddItem(expectedItens.First());
            validationResult.AddItem(expectedItens.Last());

            var headers = new MyWorksheetMap().GetFields().Select(s => s.Name).ToList();
            using var worksheet = CreateStream(expectedItens, headers);
            var itens = parser.Parse(worksheet, worksheetName);
            itens.Should().BeEquivalentTo(validationResult);
        }

        [Test]
        public void ShouldCreateNewColumnsWithErrors()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddWorksheetParser<FakeClass>(x => x.WithParser<ClosedXmlParser<FakeClass>>().WithMap<MyWorksheetMap>());

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var expectedItens = new List<FakeClass>
            {
                FakeClass.CreateItem(),
                FakeClass.CreateItem(false, false, false),
                FakeClass.CreateItem(false, false, true)
            };

            var parser = serviceProvider.GetService<WorksheetParser<FakeClass>>();

            expectedItens.ForEach(x => x.Bonus *= 0.1M);
            var validationResult = new ValidationResult<FakeClass>();
            var error = new Error("Field TX_NAME can´t not be null", 3, 2);
            validationResult.AddError(error);
            validationResult.AddItem(expectedItens.First());
            validationResult.AddItem(expectedItens.Last());

            var headers = new MyWorksheetMap().GetFields().Select(s => s.Name).ToList();
            using var worksheet = CreateStream(expectedItens, headers);
            var itens = parser.Parse(worksheet, worksheetName);

            using var streamWithErros = parser.WriteErrorsWithSummary(worksheet, worksheetName, itens.Errors);

            using var workbookWithErrors = new XLWorkbook(streamWithErros);
            var reader = new ClosedXmlReader(workbookWithErrors, worksheetName);
            reader.CountColumns().Should().Be(headers.Count() + 1);
        }


        private static XLWorkbook CreateExcel(List<FakeClass> itens, List<string> cabecalho)
        {
            var excel = new XLWorkbook();

            var worksheet = excel.AddWorksheet(worksheetName);

            for (var i = 1; i <= cabecalho.Count; i++)
                worksheet.Cell(1, i).Value = cabecalho[i - 1];

            for (var i = 0; i < itens.Count; i++)
            {
                var linha = i + 2;
                var item = itens[i];
                worksheet.Cell(linha, 5).Value = item.Bonus.ToString();
                worksheet.Cell(linha, 4).Value = item.Enable.ToString();
                worksheet.Cell(linha, 3).Value = item.FinishDate.ToString();
                worksheet.Cell(linha, 1).Value = item.Id.ToString();
                worksheet.Cell(linha, 2).Value = item.Name?.ToString();
            }

            return excel;
        }

        private static MemoryStream CreateStream(List<FakeClass> itens, List<string> cabecalho) => CreateExcel(itens, cabecalho).GetStream();

        public class CustomMessages : MessageErrors
        {
            public override string EmptyRequiredField(string header) => $"Can't not be null field {header}";
        }
    }
}