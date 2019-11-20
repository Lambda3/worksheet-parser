using System;
using System.Text;

namespace Worksheet.Parser.Sample
{
    public class RowFake
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public int RegisterNumber { get; set; }
        public bool Enable { get; set; }
        public decimal Value { get; set; }
        public double? Bonus { get; set; }

        public override string ToString()
        {
            var message = new StringBuilder();
            var properties = GetType().GetProperties();
            foreach (var property in properties)
            {
                message.AppendLine($"{property.Name}: {property.GetValue(this)}");
            }

            return message.ToString();
        }
    }
}
