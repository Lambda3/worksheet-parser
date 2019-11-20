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

        public virtual ParserBuilder ToClass<T>() where T : class, new()
        {
            serviceCollection.TryAddSingleton<WorksheetParser<T>>();
            var maper =
                AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(t => t.GetTypes())
                    .FirstOrDefault(x => x
                        .IsSubclassOf(typeof(WorksheetMap<T>)) && !x.IsAbstract);
            serviceCollection.AddSingleton(typeof(WorksheetMap<T>), maper);
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
            serviceCollection.AddSingleton<C>();
            return this;
        }

        public virtual ParserBuilder WithValueSetter<V>() where V : ValueSetter
        {
            serviceCollection.AddSingleton<V>();
            return this;
        }

        public virtual ParserBuilder WithMessages(MessageErrors messageErrors)
        {
            serviceCollection.AddSingleton<MessageErrors>();
            return this;
        }
    }
}
