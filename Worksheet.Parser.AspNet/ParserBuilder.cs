using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;

namespace Worksheet.Parser.AspNetCore
{
    public class ParserBuilder
    {
        protected readonly IServiceCollection serviceCollection;

        public ParserBuilder(IServiceCollection serviceCollection) => this.serviceCollection = serviceCollection;

        public virtual ParserBuilder WithWorksheet<T, Q>() where T : class, new() where Q : WorksheetDefault<T>
        {
            serviceCollection.AddSingleton<WorksheetDefault<T>, Q>();
            ToClass<T>();
            return this;
        }

        public virtual ParserBuilder ToClass<T>() where T : class, new()
        {
            serviceCollection.TryAddSingleton<WorksheetParser<T>>();
            var maper =
                AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(t => t.GetTypes())
                    .FirstOrDefault(x => x
                        .IsSubclassOf(typeof(WorksheetMap<T>)) && !x.IsAbstract);
            serviceCollection.TryAddSingleton(typeof(WorksheetMap<T>), maper);
            return this;
        }


        public virtual ParserBuilder WithMap<T, Q>() where T : WorksheetMap<Q> where Q : class, new()
        {
            serviceCollection.TryAddSingleton<WorksheetParser<Q>>();
            serviceCollection.TryAddSingleton<WorksheetMap<Q>, T>();
            return this;
        }

        public virtual ParserBuilder WithConverter<C>() where C : Converter
        {
            serviceCollection.AddSingleton<Converter, C>();
            return this;
        }

        public virtual ParserBuilder WithValueSetter<V>() where V : ValueSetter
        {
            serviceCollection.AddSingleton<ValueSetter, V>();
            return this;
        }

        public virtual ParserBuilder WithMessages<M>() where M : MessageErrors
        {
            serviceCollection.AddSingleton<MessageErrors, M>();
            return this;
        }
    }
}
