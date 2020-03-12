namespace Worksheet.Parser
{
    public class Error
    {
        public Error(string message, int row, int column)
        {
            Message = message;
            Row = row;
            Column = column;
        }

        public string Message { get; }
        public int Row { get; }
        public int Column { get; }
    }
}
