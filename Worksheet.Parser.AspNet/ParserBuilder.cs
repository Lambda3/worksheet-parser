using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Worksheet.Parser.AspNetCore
{
    public class ParserBuilder<T> where T : class, new()
    {
        protected readonly IServiceCollection serviceCollection;

        public ParserBuilder(IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<WorksheetInterpreter<T>>();
            this.serviceCollection = serviceCollection;
        }

        public virtual ParserBuilder<T> WithParser<Q>() where Q : WorksheetParser<T>
        {
            serviceCollection.AddSingleton<WorksheetParser<T>, Q>();
            return this;
        }

        public virtual ParserBuilder<T> WithMap<Q>() where Q : WorksheetMap<T>
        {
            serviceCollection.TryAddSingleton<WorksheetMap<T>, Q>();
            return this;
        }

        public virtual ParserBuilder<T> WithConverter<C>() where C : Converter
        {
            serviceCollection.AddSingleton<Converter, C>();
            return this;
        }

        public virtual ParserBuilder<T> WithValueSetter<V>() where V : ValueSetter
        {
            serviceCollection.AddSingleton<ValueSetter, V>();
            return this;
        }

        public virtual ParserBuilder<T> WithMessages<M>() where M : MessageErrors
        {
            serviceCollection.AddSingleton<MessageErrors, M>();
            return this;
        }
    }
}
