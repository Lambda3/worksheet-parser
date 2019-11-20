using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Worksheet.Parser.AspNetCore
{
    public static class ServiceCollectionExtensions
    {
        public static void AddWorksheetParser(this IServiceCollection serviceCollection, Func<ParserBuilder, ParserBuilder> builder)
        {
            builder(new ParserBuilder(serviceCollection));
            serviceCollection.AddWorksheetParser();
        }

        public static void AddWorksheetParser(this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<MessageErrors>();
            serviceCollection.TryAddSingleton<Converter>();
            serviceCollection.TryAddSingleton<ValueSetter>();
        }

    }
}
