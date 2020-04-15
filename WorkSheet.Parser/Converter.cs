using System;
using System.ComponentModel;

namespace Worksheet.Parser
{
    public class Converter
    {

        public virtual object Convert(Type destinationType, object value)
        {
            return value == null || (value != null  && string.IsNullOrEmpty(value.ToString()))  || value.GetType() == destinationType
                ? value
                : TypeDescriptor.GetConverter(destinationType).ConvertFrom(value);
        }
    }
}
