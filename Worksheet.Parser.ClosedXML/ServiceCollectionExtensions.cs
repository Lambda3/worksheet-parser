using Microsoft.Extensions.DependencyInjection;
using System;
using Worksheet.Parser.AspNetCore;

namespace Worksheet.Parser.ClosedXML
{
    public static class ServiceCollectionExtensions
    {
        public static void AddClosedXMLWorksheetParser(this IServiceCollection serviceCollection, Func<ClosedXMLParserBuilder, ClosedXMLParserBuilder> builder)
        {
            builder(new ClosedXMLParserBuilder(serviceCollection, new ParserBuilder(serviceCollection)));
            serviceCollection.AddWorksheetParser();
        }

    }
}
