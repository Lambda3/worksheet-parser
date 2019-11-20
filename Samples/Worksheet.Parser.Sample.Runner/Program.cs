using System;
using System.Diagnostics;
using Worksheet.Parser.Sample;

namespace Worksheet.Parser.Samples.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var itensCount = 1000000;
            Console.WriteLine("Configuring map");
            var worksheet = new MyWorksheet(itensCount);

            var converter = new WorksheetParser<RowFake>(new ValueSetter(new Converter()), new MyWorksheetMap(), new MessageErrors());

            Console.WriteLine("Start read and parse");
            var stopWatch = Stopwatch.StartNew();
            var result = converter.Parse(worksheet);
            stopWatch.Stop();
            Console.WriteLine("Finish read and parse");
            Console.WriteLine($"Processed {result.Itens.Count} in {stopWatch.ElapsedMilliseconds}");
            //var itens = result.Itens;
            //foreach (var item in itens)
            //{
            //    Console.WriteLine(item.ToString());
            //    Console.WriteLine();
            //}
        }
    }
}
