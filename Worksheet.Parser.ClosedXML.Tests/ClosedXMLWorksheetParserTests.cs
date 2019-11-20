using ClosedXML.Excel;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Worksheet.Parser.Tests.Shared;

namespace Worksheet.Parser.ClosedXML.Tests
{
    public class ClosedXMLWorksheetParserTests
    {
        private const string worksheetName = "worksheet1";

        [Test]
        public void ShouldParseSheetUsingClosedXML()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddClosedXMLWorksheetParser(x => x.WithMap<MyWorksheetMap, FakeClass>());

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var expectedItens = new List<FakeClass>();
            for (var i = 0; i < 1; i++)
                expectedItens.Add(FakeClass.CreateItem(i % 2 == 0, i % 5 == 0));


            var headers = new MyWorksheetMap().GetFields().Select(s => s.Name).ToList();
            using var workbook = CreateExcel(expectedItens, headers);
            var worksheet = workbook.Worksheet(worksheetName);
            var parser = serviceProvider.GetService<ClosedXMLWorksheetParser<FakeClass>>();

            expectedItens.ForEach(x => x.Bonus = x.Bonus * 0.1M);
            var validationResult = new ValidationResult<FakeClass>();
            validationResult.AddItem(expectedItens);

            var itens = parser.Parse(worksheet);
            itens.Should().BeEquivalentTo(validationResult);
        }

        private static XLWorkbook CreateExcel(List<FakeClass> itens, List<string> cabecalho)
        {
            var excel = new XLWorkbook();

            var worksheet = excel.AddWorksheet(worksheetName);

            for (var i = 1; i <= cabecalho.Count; i++)
            {
                worksheet.Cell(1, i).Value = cabecalho[i - 1];
            }

            for (var i = 0; i < itens.Count; i++)
            {
                var linha = i + 2;
                var item = itens[i];
                worksheet.Cell(linha, 5).Value = item.Bonus.ToString();
                worksheet.Cell(linha, 4).Value = item.Enable.ToString();
                worksheet.Cell(linha, 3).Value = item.FinishDate.ToString();
                worksheet.Cell(linha, 1).Value = item.Id.ToString();
                worksheet.Cell(linha, 2).Value = item.Name.ToString();
            }

            return excel;
        }

    }
}