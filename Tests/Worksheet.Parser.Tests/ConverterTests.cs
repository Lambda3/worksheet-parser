using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Worksheet.Parser.Tests
{
    public class ConverterTestes
    {
        private readonly Converter converter;

        public ConverterTestes() => converter = new Converter();

        [Test]
        public void ShouldReturnNullIfValueIsNUll() => converter.Convert(A.Dummy<Type>(), null).Should().BeNull();

        private static readonly List<object> equalsType = new List<object>
        {
            1,
            new DateTime(2019, 12, 30),
            "teste",
            10.35,
            true
        };

        [TestCaseSource(nameof(equalsType))]
        public void ShouldReturnValueIfDestinationTypeIsEqualParameterType(object value) => converter.Convert(value.GetType(), value).Should().Be(value);

        private static readonly List<(object, object)> convertiblesValues = new List<(object, object)>
        {
            ("1", 1),
            ("12/30/2019", new DateTime(2019, 12, 30)),
            ("10.35", 10.35),
            ("true", true)
        };

        [TestCaseSource(nameof(convertiblesValues))]
        public void ShouldConvertValuesWithDifferentTypes((object value, object expectedValue) parameter) 
            => converter.Convert(parameter.expectedValue.GetType(), parameter.value).Should().Be(parameter.expectedValue);
    }
}