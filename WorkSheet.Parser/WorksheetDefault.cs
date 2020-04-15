using System.Collections.Generic;
using System.IO;

namespace Worksheet.Parser
{
    public abstract class WorksheetDefault<T> where T : class, new()
    {
        protected readonly WorksheetParser<T> parser;
        protected readonly MessageErrors messageErrors;

        public WorksheetDefault(WorksheetParser<T> parser, MessageErrors messageErrors)
        {
            this.parser = parser;
            this.messageErrors = messageErrors;
        }

        protected abstract WorksheetReader GetReader(string path, string worksheetName);

        protected abstract WorksheetReader GetReader(Stream stream, string worksheetName);

        protected abstract MemoryStream Write(WorksheetReader reader);

        public virtual ValidationResult<T> Parse(string path, string worksheetName)
        {
            using var reader = GetReader(path, worksheetName);
            return parser.Parse(reader);
        }
        public virtual ValidationResult<T> Parse(Stream stream, string worksheetName)
        {
            using var reader = GetReader(stream, worksheetName);
            return parser.Parse(reader);
        }

        public virtual MemoryStream AddErrors(Stream stream, string worksheetName, List<Error> erros)
        {
            var reader = GetReader(stream, worksheetName);
            erros.ForEach(e => reader.AddError(e));
            var streamWithErrors = new MemoryStream();
            return Write(reader);
        }

        public virtual MemoryStream AddFirstColumnsWithErrors(Stream stream, string worksheetName, List<Error> erros)
        {
            var reader = GetReader(stream, worksheetName);
            erros.ForEach(e => reader.AddError(e));
            reader.AddFirstColumnWithErrors(erros, messageErrors);
            return Write(reader);
        }
    }
}
