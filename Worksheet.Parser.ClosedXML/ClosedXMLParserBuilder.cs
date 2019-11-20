using Microsoft.Extensions.DependencyInjection;
using Worksheet.Parser.AspNetCore;

namespace Worksheet.Parser.ClosedXML
{
    public class ClosedXMLParserBuilder
    {
        private readonly IServiceCollection serviceCollection;
        private readonly ParserBuilder parser;

        public ClosedXMLParserBuilder(IServiceCollection serviceCollection, ParserBuilder parser)
        {
            this.serviceCollection = serviceCollection;
            this.parser = parser;
        }

        public ClosedXMLParserBuilder ToClass<T>() where T : class, new()
        {
            serviceCollection.AddSingleton<ClosedXMLWorksheetParser<T>>();
            parser.ToClass<T>();
            return this;
        }
        
        public virtual ClosedXMLParserBuilder WithMap<T, Q>() where T : WorksheetMap<Q> where Q : class, new()
        {
            serviceCollection.AddSingleton<ClosedXMLWorksheetParser<Q>>();
            parser.WithMap<T, Q>();
            return this;
        }

        public virtual ClosedXMLParserBuilder WithConverter<C>() where C : Converter
        {
            parser.WithConverter<C>();
            return this;
        }

        public virtual ClosedXMLParserBuilder WithValueSetter<V>() where V : ValueSetter
        {
            parser.WithValueSetter<V>();
            return this;
        }

        public virtual ClosedXMLParserBuilder WithMessages(MessageErrors messageErrors)
        {
            parser.WithMessages(messageErrors);
            return this;
        }

    }
}
